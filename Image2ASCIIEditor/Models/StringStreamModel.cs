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

namespace Image2ASCIIEditor.Models;
public class StringStreamModel
{
    public List<List<char>> OriginalStream = new List<List<char>>();

    public List<List<PaintBlock>> PaintBlocks = new List<List<PaintBlock>>();
    public int _n = 0;
    public int _m = 0;

    public StringStreamModel(TextBox input)
    {
        string s = input.Text;
        string[] split = s.Split(new char[2] {'\r','\n'});
        int _maxLength = 0 ;
        for(int i = 0; i < split.Length; i++)
        {
            PaintBlocks.Add(new List<PaintBlock>());
            if (_maxLength < split[i].Length) _maxLength = split[i].Length;
        }
        _m = _maxLength;
        _n = split.Length;
        for(int i = 0; i < split.Length; i++)
        {
            for(int j = 0; j < split[i].Length; j++)
            {
                PaintBlocks[i].Add(new PaintBlock(split[i][j], i, j, new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Black)));

            }

            for(int j = split[i].Length; j < _maxLength; j++)
            {
                PaintBlocks[i].Add(new PaintBlock(' ', i, j, new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Black)));
            }
        }
    }
    public StringStreamModel(int n, int m)
    {
        this._n = n;
        this._m = m;
        for(int i = 0; i < n; i++)
        {
            PaintBlocks.Add(new List<PaintBlock>());
        }

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
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
}
