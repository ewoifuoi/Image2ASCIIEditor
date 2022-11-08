using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace Image2ASCIIEditor.Common;

public class PointF
{
    public Rectangle rect;
    public double R;
    public double G;
    public double B;

    public PointF()
    {
    }

    public PointF(double r, double g, double b)
    {
        R = r;
        G = g;
        B = b;
    }
}

public class Cluster
{
    Random r;
    public Cluster()
    {
        r = new Random();
    }

    
    private double Distance(PointF p1, PointF p2)
    {
        return 
            Math.Sqrt
            ( Math.Pow(p1.R - p2.R,2)
            + Math.Pow(p1.G - p2.G,2)
            + Math.Pow(p1.B - p2.B,2));
    }

    /// <summary>
    /// 生成随机数
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private double randVal(double min, double max)
    {
        return (double)(r.NextDouble() * (max - min) + min);
    }

    /// <summary>
    /// 为给定的数据集构建一个包含k个随机质心的集合
    /// </summary>
    /// <param name="dataSet">数据点集</param>
    /// <param name="k">簇个数</param>
    /// <returns>返回值为质心的坐标列表</returns>
    private List<PointF> randCnt(ref List<PointF> dataSet, int k)
    {
        List<PointF> centoids = new List<PointF>();

        double minR = dataSet[0].R;
        double minG = dataSet[0].G;
        double minB = dataSet[0].B;
        double maxR = dataSet[0].R;
        double maxG = dataSet[0].G;
        double maxB = dataSet[0].B;

        // 确定随机质心的取值范围
        for(int i = 1; i < dataSet.Count; i++)
        {
            if(minR > dataSet[i].R)
            {
                minR = dataSet[i].R;
            }
            if (minG > dataSet[i].G)
            {
                minG = dataSet[i].G;
            }
            if (minB > dataSet[i].B)
            {
                minB = dataSet[i].B;
            }
            if(maxR < dataSet[i].R)
            {
                maxR = dataSet[i].R;
            }
            if (maxG < dataSet[i].G)
            {
                maxG = dataSet[i].G;
            }
            if (maxB < dataSet[i].B)
            {
                maxB = dataSet[i].B;
            }

        }

        // 随机质心
        for(int i = 0; i < k; i++)
        {
            PointF p = new PointF();
            p.R = randVal(minR, maxR);
            p.G = randVal(minG, maxG);
            p.B = randVal(minB, maxB);
            centoids.Add(p);
        }
        
        return centoids;
    }

    public void KMeans(List<PointF> dataSet, int k, ref List<PointF> centroid, ref double[,] clusterAssment)
    {
        int m = dataSet.Count; // 数据点数
        //clusterAssment = new double[m, 2];  // 簇分配结果矩阵，一列记录簇索引，一列存储误差
        //centroid = randCnt(ref dataSet, k);
        bool clusterChanged = true;

        // 计算质心- 分配- 重新计算 反复迭代
        while (clusterChanged)
        {
            clusterChanged = false;
            for (int i = 0; i < m; i++) //遍历一遍所有数据点
            {
                double minDis = Double.MaxValue;
                int minIndex = -1;

                // 寻找最近的质心
                for (int j = 0; j < k; j++)
                {
                    double dist = Distance(dataSet[i], centroid[j]);
                    if (dist < minDis)
                    {
                        minDis = dist; // 最近
                        minIndex = j;
                    }
                }

                if (clusterAssment[i, 0] != minIndex)
                {
                    clusterChanged = true;
                }

                clusterAssment[i, 0] = minIndex;
                clusterAssment[i, 1] = minDis * minDis;
            }

            // 更新质心位置
            for (int cent = 0; cent < k; cent++)
            {
                double avgR = 0, avgG = 0, avgB = 0, cnt = 0;
                for (int i = 0; i < m; i++)
                {
                    if (clusterAssment[i, 0] == cent)
                    {
                        avgR += dataSet[i].R;
                        avgG += dataSet[i].G;
                        avgB += dataSet[i].B;
                        cnt++;
                    }
                }
                centroid[cent].R = avgR / cnt;
                centroid[cent].G = avgG / cnt;
                centroid[cent].B = avgB / cnt;
            }
        }
    }

    // 二分k均值聚类算法
    public void biKeans(ref List<PointF> dataSet, int k, ref double[,] clusterAssment, ref List<PointF> centList)
    {
        int m = dataSet.Count;
        //double[,] clusterAssment = new double[m, k];   // 第一列存储簇分配结果，第二列存储平方误差
        //List<Point> centList = new List<Point>();      // 存储所有质心

        // 找到第一个质心
        double centroidR = 0, centroidG = 0, centroidB = 0;
        for (int i = 0; i < m; i++)
        {
            centroidR += dataSet[i].R;
            centroidG += dataSet[i].G;
            centroidB += dataSet[i].B;
        }
        PointF cent = new PointF(centroidR / m, centroidG / m, centroidB / m);
        centList.Add(cent);

        // 计算数据集中所有点到质心的误差
        for (int j = 0; j < m; j++)
        {
            clusterAssment[j, 1] = Distance(dataSet[j], cent);
        }

        // 不停地对每个簇进行划分，直到得到想要的簇的数目
        while (centList.Count < k)
        {
            double lowestSSE = Double.MaxValue;
            int bestCentertoSplit = 0;
            double[,] bestClusAss = null;
            List<PointF> bestNewCenter = null;
            int bestN = 0;
            int cnt = centList.Count;

            // 尝试划分每一簇
            for (int i = 0; i < cnt; i++)
            {
                // 得到位于当前簇中的点
                List<PointF> ptsInCurrCluster = new List<PointF>();
                for (int j = 0; j < m; j++)
                {
                    if (clusterAssment[j, 0] == i)
                    {
                        ptsInCurrCluster.Add(dataSet[j]);
                    }
                }
                int n = ptsInCurrCluster.Count;

                // 生成两个簇
                List<PointF> centroidMat = randCnt(ref ptsInCurrCluster, 2);
                double[,] splitClustAss = new double[ptsInCurrCluster.Count, 2];
                KMeans(ptsInCurrCluster, 2, ref centroidMat, ref splitClustAss);

                // 误差和
                double sseSplit = 0, sseNotSplit = 0;
                for (int j = 0; j < n; j++)
                    sseSplit += splitClustAss[j, 1];
                for (int j = 0; j < m; j++)
                    if (clusterAssment[j, 0] != i)
                        sseNotSplit += clusterAssment[j, 1];

                // 记录最佳划分
                if (sseSplit + sseNotSplit < lowestSSE)
                {
                    bestCentertoSplit = i; ;
                    bestNewCenter = centroidMat;
                    bestClusAss = splitClustAss;
                    bestN = n;
                }
            }

            // 将要划分的簇中的点的簇分配结果进行修改
            for (int i = 0; i < bestN; i++)
            {
                if (bestClusAss[i, 0] == 0)
                    bestClusAss[i, 0] = bestCentertoSplit;
                else
                    bestClusAss[i, 0] = centList.Count;
            }

            // 修改质心列表
            centList[bestCentertoSplit] = bestNewCenter[0];
            centList.Add(bestNewCenter[1]);

            // 修改平方误差
            int kk = 0;
            for (int i = 0; i < m; i++)
            {
                if (clusterAssment[i, 0] == bestCentertoSplit)
                {
                    clusterAssment[i, 0] = bestClusAss[kk, 0];
                    clusterAssment[i, 1] = bestClusAss[kk, 1];
                    ++kk;
                }
            }
        }
    }

    public List<List<Rectangle>> Execute(List<List<Rectangle>> rect, int k)
    {
        // 将矩形中的颜色取出, 并将其添加到新的pointF 列表中
        List<PointF> points = new List<PointF>();
        for(int i = 0; i < rect.Count; i++)
        {
            for(int j = 0; j < rect[i].Count; j++)
            {
                SolidColorBrush scb = rect[i][j].Fill as SolidColorBrush;
                if(scb.Color != Colors.Transparent)
                {
                    PointF p = new PointF() { rect = rect[i][j], R = scb.Color.R, G = scb.Color.G, B = scb.Color.B };
                    points.Add(p);
                }
                
            }
        }
        double[,] clusterAssment = new double[points.Count, 2];   // 第一列存储簇分配结果，第二列存储平方误差
        List<PointF> centList = new List<PointF>();// 用来保存聚类中心

        // 生成聚类
        biKeans(ref points, k, ref clusterAssment, ref centList);
        int[] type = new int[points.Count];// 用来保存 每个数据点属于哪个聚类
        for (int i = 0; i < points.Count; i++) // 根据聚类中心对数据点进行分组
        {
            double minDis = Double.MaxValue;
            for (int j = 0; j < k; j++)
            {
                double dist = Distance(points[i], centList[j]);
                if (dist < minDis)
                {
                    minDis = dist; // 最近
                    type[i] = j;
                }
            }
        }

        List<List<Rectangle>> result = new List<List<Rectangle>>();
        for (int i = 0; i < k; i++) result.Add(new List<Rectangle>());
        for(int i = 0; i < points.Count; i++)
        {
            result[type[i]].Add(points[i].rect);
        }
        return result;
    }

}
