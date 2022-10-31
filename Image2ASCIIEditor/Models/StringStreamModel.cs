using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Image2ASCIIEditor.Common;
using Console = Image2ASCIIEditor.Common.Console;

namespace Image2ASCIIEditor.Models;
public class StringStreamModel
{
    public List<List<char>> OriginalStream = new List<List<char>>();
    public int n = 0;
    public int m = 0;

    public StringStreamModel(TextBox input)
    {
        string s = input.Text;
        string[] split = s.Split(new char[2] {'\r','\n'});
        int _maxLength = 0 ;
        for(int i = 0; i < split.Length; i++)
        {
            OriginalStream.Add(new List<char>());
            if (_maxLength < split[i].Length) _maxLength = split[i].Length;
        }
        m = _maxLength;
        n = split.Length;
        for(int i = 0; i < split.Length; i++)
        {
            for(int j = 0; j < split[i].Length; j++)
            {
                OriginalStream[i].Add(split[i][j]);
            }

            for(int j = split[i].Length; j < _maxLength; j++)
            {
                OriginalStream[i].Add(' ');
            }
        }
    }
    public StringStreamModel(int n, int m)
    {
        this.n = n;
        this.m = m;
        for(int i = 0; i < n; i++)
        {
            OriginalStream.Add(new List<char>());
        }

        for(int i = 0; i < n; i++)
        {
            for(int j = 0; j < m; j++)
            {
                OriginalStream[i].Add(' ');
            }
        }
    }

    
}
