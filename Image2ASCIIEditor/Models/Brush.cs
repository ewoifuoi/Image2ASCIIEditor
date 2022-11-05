using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;

namespace Image2ASCIIEditor.Models;
public class Brush
{
    public char ch;
    public SolidColorBrush foreground_color;
    public SolidColorBrush background_color;

    public Brush(char ch, SolidColorBrush f_c, SolidColorBrush b_c)
    {
        this.ch = ch;
        this.foreground_color = f_c;
        this.background_color = b_c;
    }

    public void SetPaint(ref TextBlock _b, ref Canvas _c)
    {
        _b.Text = ch.ToString();
        _b.Foreground = foreground_color;
        _c.Background = background_color;
    }

   
}

public class BrushesList
{


    private List<Brush> brushes;
    public BrushesList()
    {
        brushes = new List<Brush>();
    }

    public void AddBrushToList(ref ListView list, Brush _brush)
    {
        brushes.Add(_brush);
        list.Items.Add(GenerateAnItem(_brush));
        
    }

    public StackPanel GenerateAnItem(Brush _brush)
    {
        StackPanel sp = new StackPanel();
        sp.Orientation = Orientation.Horizontal;
        Rectangle re = new Rectangle();
        re.Height = 20; re.Width = 20;
        re.Fill = _brush.foreground_color;
        TextBlock title = new TextBlock();
        title.Text = "笔刷 " + (brushes.Count).ToString() + " : ";
        title.Margin = new Microsoft.UI.Xaml.Thickness(0, 0, 10, 0);
        TextBlock ch = new TextBlock();
        ch.Text = _brush.ch.ToString();
        ch.Margin = new Microsoft.UI.Xaml.Thickness(15, 0, 0, 0);
        sp.Children.Add(title);
        sp.Children.Add(re);
        sp.Children.Add(ch);

        return sp;
    }

    public void Clear()
    {
        brushes.Clear();
    }
}
