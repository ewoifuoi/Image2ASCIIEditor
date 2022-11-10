using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Image2ASCIIEditor.Common;
using Console = Image2ASCIIEditor.Common.Console;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Windows.ApplicationModel.Activation;
using System.Drawing;

namespace Image2ASCIIEditor.Models;
public class StringStreamModel
{
    public static List<List<char>> charsList;
    public static List<List<int>> colorList;

    public List<List<char>> OriginalStream = new List<List<char>>();

    public List<List<PaintBlock>> PaintBlocks = new List<List<PaintBlock>>();
    public int _n = 0;
    public int _m = 0;

    public static int ColorConvertForWindows(Windows.UI.Color color)
    {
        if (color == Colors.Red)
        {
            return 12;
        }
        else if (color == Colors.Blue)
        {
            return 9;
        }
        else if (color == Colors.Yellow)
        {
            return 14;
        }
        else if (color == Colors.Magenta)
        {
            return 13;
        }
        else if (color == Colors.Green)
        {
            return 10;
        }
        else if (color == Colors.Cyan)
        {
            return 11;
        }
        else if (color == Colors.White)
        {
            return 15;
        }
        else
        {
            return 0;
        }
    }

    public StringStreamModel(TextBox input)
    {
        charsList = new List<List<char>>();
        colorList = new List<List<int>>();
        string s = input.Text;
        string[] split = s.Split(new char[2] {'\r','\n'});
        int _maxLength = 0 ;
        for(int i = 0; i < split.Length; i++)
        {
            charsList.Add(new List<char>());
            colorList.Add(new List<int>());
            PaintBlocks.Add(new List<PaintBlock>());
            if (_maxLength < split[i].Length) _maxLength = split[i].Length;
        }
        _m = _maxLength;
        _n = split.Length;
        for(int i = 0; i < split.Length; i++)
        {
            for(int j = 0; j < split[i].Length; j++)
            {
                charsList[i].Add(split[i][j]);
                colorList[i].Add(15);
                PaintBlocks[i].Add(new PaintBlock(split[i][j], i, j, new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Black)));
            }

            for(int j = split[i].Length; j < _maxLength; j++)
            {
                charsList[i].Add(' ');
                colorList[i].Add(0);
                PaintBlocks[i].Add(new PaintBlock(' ', i, j, new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Black)));
            }
        }
    }
    public StringStreamModel(int n, int m)
    {
        this._n = n;
        this._m = m;
        charsList = new List<List<char>>();
        colorList = new List<List<int>>();
        for (int i = 0; i < n; i++)
        {
            charsList.Add(new List<char>());
            colorList.Add(new List<int>());
            PaintBlocks.Add(new List<PaintBlock>());
        }

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
                charsList[i].Add(' ');
                colorList[i].Add(0);
                PaintBlocks[i].Add(new PaintBlock(' ', i, j, new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Black)));
            }
        }
    }

    public void Generate(ref Canvas g)
    {
        g.Children.Clear();
        for (int i = 0; i < _n; i++)
        {
            for (int j = 0; j < _m; j++)
            {
                PaintBlocks[i][j].PutInCanvas(ref g);
            }
        }

    }

    public void Paint(int x, int y, Brush brush)
    {
        if(x < _n && y < _m && x >= 0 && y >= 0)
        {
            PaintBlocks[x][y].ChangePaint(brush);
            StringStreamModel.charsList[x][y] = Convert.ToChar(brush.ch);
            SolidColorBrush scb = brush.foreground_color as SolidColorBrush;
            StringStreamModel.colorList[x][y] = ColorConvertForWindows(scb.Color);
        }
        
    }

    public void Erase(int x, int y)
    {
        if (x < _n && y < _m)
        {
            PaintBlocks[x][y].ChangePaint(new Brush('\0',new SolidColorBrush(Colors.Transparent), new SolidColorBrush(Colors.Black)));
        }
    }
}
