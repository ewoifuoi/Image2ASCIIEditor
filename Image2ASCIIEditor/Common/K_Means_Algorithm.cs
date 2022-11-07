using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Media.Devices;
using Windows.UI;

namespace Image2ASCIIEditor.Common;


public class FPoint
{
    public Rectangle rect;
    public int R;
    public int G;
    public int B;
}

public class K_Means_Algorithm
{
    private static int K = 0;
    private List<FPoint> points;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p">直接将导入图片界面的矩形矩阵引用传入</param>
    /// <param name="k"></param>
    public K_Means_Algorithm(List<List<Rectangle>>p, int k)
    {
        K = k;
        points = new List<FPoint>();
        for(int i = 0; i < p.Count; i++)
        {
            for(int j = 0; j < p[i].Count; j++)
            {
                SolidColorBrush scb = p[i][j].Fill as SolidColorBrush;
                points.Add(new FPoint() { rect = p[i][j], R = scb.Color.R, G = scb.Color.G, B = scb.Color.B });
            }
        }
    }
    
    /// <summary>
    /// 计算新的聚类中心
    /// </summary>
    /// <param name="m"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public FPoint Center_Point(int m, int[] type)
    {
        int count = 0;
        FPoint sum = new FPoint() {R=0,G=0,B=0};
        for(int i = 0; i < points.Count; i++)
        {
            if (type[i] == m)
            {
                
                sum.R = points[i].R + sum.R;
                sum.G = points[i].G + sum.G;
                sum.B = points[i].B + sum.B;
                count++;
            }
        }
        if(count > 0)
        {
            sum.R = sum.R / count;
            sum.G = sum.G / count;
            sum.B = sum.B / count;
        }

        return sum;
    }

    /// <summary>
    /// 比较两个聚类中心是否相等
    /// </summary>
    /// <returns></returns>
    private bool Compare(FPoint a, FPoint b)
    {
        if(a.R == b.R && a.G == b.G && a.B == b.B)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Order(ref int[] type, FPoint[] z)
    {
        int temp = 0;
        for(int i = 0; i < points.Count; i++)
        {
            for(int j = 0; j < K; j++)
            {
                
                if (Distance(points[i], z[temp]) > Distance(points[i], z[j]))
                {
                    temp = j;
                }
            }
            type[i] = temp;
        }
    }

    /// <summary>
    /// 计算两点的欧式距离
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    private int Distance(FPoint p1, FPoint p2)
    {
        return ((p1.R - p2.R) * (p1.R - p2.R) + (p1.G - p2.G) * (p1.G - p2.G) + (p1.B - p2.B) * (p1.B - p2.B));
    }

    /// <summary>
    /// 进行聚类
    /// </summary>
    /// <returns></returns>
    public List<List<Rectangle>> Execute()
    {
        int[] type = new int[points.Count];

        FPoint[] z = new FPoint[K];
        FPoint[] z0 = new FPoint[K];

        for(int i = 0; i < K; i++)
        {
            z[i] = points[i];
        }

        List<List<Rectangle>> result = new List<List<Rectangle>>();

        int test = 0;
        int loop = 0;
        while(test != K)
        {
            Order(ref type, z);
            for(int i = 0; i < K; i++)
            {
                z[i] = Center_Point(i, type);
                if (Compare(z[i], z0[i]))
                {
                    test = test + 1;
                }
                else
                {
                    z0[i] = z[i];
                }
            }
            loop = loop + 1;

            List<Rectangle> p = new List<Rectangle>();
            for(int j = 0; j < K; j++)
            {
                for(int i = 0; i < points.Count; i++)
                {
                    if (type[i] == j)
                    {
                        p.Add(points[i].rect);
                    }
                }
            }
            if(p.Count > 0)
            {
                result.Add(p);
            }
        }
        return result;
    }
}
