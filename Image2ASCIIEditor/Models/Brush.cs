using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

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
