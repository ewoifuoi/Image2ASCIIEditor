using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Image2ASCIIEditor.Common;
using Console = Image2ASCIIEditor.Common.Console;
using Microsoft.UI;
using Image2ASCIIEditor.Models;
using Microsoft.UI.Windowing;
using Windows.Foundation;
using Rect = Windows.Foundation.Rect;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Image2ASCIIEditor.Views.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EditText : Page
{
    private bool _isMouseDown = false;
    private PointerPoint _mouseDownPosition;
    private Point _mouseDownControlPosition;
    private Canvas _canvas;
    StringStreamModel _StreamModel;
    private TransformGroup transformGroup;



    public EditText()
    {
        this.InitializeComponent();
        //Common.Console.console = this.console;
        Common.Console.log("开始测试");
        transformGroup = playground.RenderTransform as TransformGroup;
        
    }

    private void outside_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        PointerPoint currentPoint = e.GetCurrentPoint(outside);
        double s = ((double)currentPoint.Properties.MouseWheelDelta) / 1000 + 1;
        transformGroup.Children.Add(new ScaleTransform() { CenterX = outside.Width / 2, CenterY = outside.Height / 2, ScaleX = s, ScaleY = s });

    }

    private void playground_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (!isEditing)
        {
            var c = sender as Canvas;
            _isMouseDown = true;
            _mouseDownPosition = e.GetCurrentPoint(outside);
            _mouseDownControlPosition = new Point(double.IsNaN(Canvas.GetLeft(c)) ? 0 : Canvas.GetLeft(c), double.IsNaN(Canvas.GetTop(c)) ? 0 : Canvas.GetTop(c));

            c.CapturePointer(e.Pointer);
        }
        else
        {
            var c = sender as Canvas;
            PointerPoint p = e.GetCurrentPoint(playground);
            double x = p.Position.X; double y = p.Position.Y;
            
            //Console.log("click x:" + x.ToString() + " " + "y:" + y.ToString());

            int nx = Convert.ToInt32(Math.Floor(x / 23)); int ny = Convert.ToInt32(Math.Floor(y / 48));
            
            if(x - nx * 23 <= 20 && y - ny * 48 <= 45)
            {
                Console.log(nx.ToString() + " " + ny.ToString());
            }
        }
    }

    private void playground_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (!isEditing)
        {
            var c = sender as Canvas;
            _isMouseDown = false;
            c.ReleasePointerCapture(e.Pointer);
        }
    }


    private void playground_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!isEditing)
        {
            if (_isMouseDown)
            {
                var c = sender as Canvas;
                _canvas = c;
                var pos = e.GetCurrentPoint(outside);
                var dpx = pos.Position.X - _mouseDownPosition.Position.X;
                var dpy = pos.Position.Y - _mouseDownPosition.Position.Y;
                Canvas.SetLeft(c, _mouseDownControlPosition.X + dpx);
                Canvas.SetTop(c, _mouseDownControlPosition.Y + dpy);
            }
        }
    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
        _StreamModel = new StringStreamModel(input);
        GenerateEditor();
    }

    private void UseDefault(object sender, RoutedEventArgs e)
    {
        

        int g_height = 0; int g_width = 0;
        if (!int.TryParse(gen_width.Text, out g_width))
        {
            MessageBox.Show("请输入宽度", this);
        }
        else if (!int.TryParse(gen_height.Text, out g_height))
        {
            MessageBox.Show("请输入高度", this);
        }
        else
        {
            _StreamModel = new StringStreamModel(g_height, g_width);
            GenerateEditor();
        }
    }

    private void GenerateEditor()
    {
        g.Children.Clear();
        for (int i = 0; i < _StreamModel.n; i++)
        {
            for (int j = 0; j < _StreamModel.m; j++)
            {
                var border = new Border();
                var b = new TextBlock();
                //b.Background = new SolidColorBrush(p.Color);
                b.Text = _StreamModel.OriginalStream[i][j].ToString();
                b.TextAlignment = TextAlignment.Center;
                b.FontSize = 30;
                b.Width = 20;
                b.Height = 45;
                //b.Click += B_Click;
                border.Child = b;
                border.HorizontalAlignment = HorizontalAlignment.Center;
                border.VerticalAlignment = VerticalAlignment.Center;
                //border.BorderThickness = new Thickness(1); border.BorderBrush = new SolidColorBrush(Colors.Gray);
                Canvas.SetTop(border, i * 48);
                Canvas.SetLeft(border, j * 23);
                Canvas.SetZIndex(border, 9);
                Canvas c = new Canvas(); c.Background = new SolidColorBrush(Colors.Black); c.Opacity = 0.3;
                c.Width = 20; c.Height = 45;
                Canvas.SetTop(c, i * 48);
                Canvas.SetLeft(c, j * 23);
                Canvas.SetZIndex(c, 0);
                g.Children.Add(border);
                g.Children.Add(c);

            }
        }

        gg.Height = _StreamModel.n * 48;
        gg.Width = _StreamModel.m * 23;
    }

    private bool isEditing = false;

    private void ChangeEditMode(object sender, RoutedEventArgs e)
    {
        isEditing = true;
        brushes.IsEnabled = true;
        ChangeModeBtn.IsChecked = true;
        ChangeModeBtn2.IsChecked = false;
    }

    private void ChangeEditMode2(object sender, RoutedEventArgs e)
    {
        isEditing = false;
        brushes.IsEnabled = false;
        
        ChangeModeBtn.IsChecked = false;
        ChangeModeBtn2.IsChecked = true;
    }

    private void refresh(object sender, RoutedEventArgs e)
    {
        transformGroup.Children.Clear();
        Canvas.SetLeft(playground, 0);
        Canvas.SetTop(playground, 0);
    }

    private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if(App.isMax)
        {
            
            outside.Height = 700;
            outside.Width = 1000;
            playground.Width = 1000;
            playground.Height = 700;
            border.Height = 706;
            border.Width = 1006;
            
            rect.Rect = new Rect() { Height = 700, Width = 1000, X = 0, Y = 0};
        }
        else
        {
            
            outside.Height = 450;
            outside.Width = 450;
            playground.Width = 450;
            playground.Height = 450;
            border.Height = 456;
            border.Width = 456;
            rect.Rect = new Rect() { Height = 450, Width = 450, X = 0, Y = 0 };
        }
        
    }
}
