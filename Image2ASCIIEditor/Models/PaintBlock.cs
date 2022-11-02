using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.ComponentModel;

namespace Image2ASCIIEditor.Models;
public class PaintBlock
{
    private int x; private int y;//坐标
    /// <summary>
    /// 实际字符
    /// </summary>
    private char ch;
    /// <summary>
    /// 前景色
    /// </summary>
    private SolidColorBrush foreground_color;
    /// <summary>
    /// 背景色
    /// </summary>
    private SolidColorBrush background_color;

    /// <summary>
    /// 字符实际背景
    /// </summary>
    private Border _border;
    /// <summary>
    /// 字符框
    /// </summary>
    private TextBlock _b;
    /// <summary>
    /// 图片蒙版画布
    /// </summary>
    private Canvas _c;

    public PaintBlock(char ch,int x, int y, SolidColorBrush f, SolidColorBrush b)
    {
        _border = new Border();
        _b = new TextBlock();
        _c = new Canvas();
        _b.Text = ch.ToString();
        _b.TextAlignment = TextAlignment.Center;
        _b.FontSize = 30;
        _b.Width = 20;
        _b.Height = 45;
        _border.Child = _b;
        _border.HorizontalAlignment = HorizontalAlignment.Center;
        _border.VerticalAlignment = VerticalAlignment.Center;
        Canvas.SetTop(_border, x * 48);
        Canvas.SetLeft(_border, y * 23);
        Canvas.SetZIndex(_border, 9);
        _c.Background = new SolidColorBrush(Colors.Black); 
        _c.Opacity = 0.3;
        _c.Width = 20; 
        _c.Height = 45;
        Canvas.SetTop(_c, x * 48);
        Canvas.SetLeft(_c, y * 23);
        Canvas.SetZIndex(_c, 0);
    }

    public void PutInCanvas(ref Canvas g)
    {
        g.Children.Add(_border);
        g.Children.Add(_c);
    }

    public void ChangePaint(Brush brush)
    {
        brush.SetPaint(ref _b, ref _c);
    }
}
