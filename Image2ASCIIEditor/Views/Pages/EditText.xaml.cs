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
using Brush = Image2ASCIIEditor.Models.Brush;

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
    Brush _brush;
    private Canvas _canvas;
    StringStreamModel _StreamModel;
    private TransformGroup transformGroup;
    private List<SolidColorBrush> ColorOptions = new List<SolidColorBrush>();
    private SolidColorBrush CurrentColorBrush = null;
    private bool _hasCanvas = false;


    public EditText()
    {
        this.InitializeComponent();
        //Common.Console.console = this.console;
        Common.Console.log("开始测试");
        transformGroup = playground.RenderTransform as TransformGroup;
        _brush = new Brush('*', new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Black));

        ColorOptions.Add(new SolidColorBrush(Colors.Black));
        ColorOptions.Add(new SolidColorBrush(Colors.Red));
        ColorOptions.Add(new SolidColorBrush(Colors.Orange));
        ColorOptions.Add(new SolidColorBrush(Colors.Yellow));
        ColorOptions.Add(new SolidColorBrush(Colors.Green));
        ColorOptions.Add(new SolidColorBrush(Colors.Blue));
        ColorOptions.Add(new SolidColorBrush(Colors.Indigo));
        ColorOptions.Add(new SolidColorBrush(Colors.Violet));
        ColorOptions.Add(new SolidColorBrush(Colors.White));
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
        else if(_hasCanvas)
        {


            // 计算正在点击的坐标
            var c = sender as Canvas;
            PointerPoint p = e.GetCurrentPoint(playground);
            double x = p.Position.X; double y = p.Position.Y;
            //Console.log("click x:" + x.ToString() + " " + "y:" + y.ToString());
            int nx = Convert.ToInt32(Math.Floor(x / 23)); int ny = Convert.ToInt32(Math.Floor(y / 48));
            
            if(x - nx * 23 <= 20 && y - ny * 48 <= 45)
            {
                _StreamModel.Paint(ny, nx, _brush);
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

    /// <summary>
    /// 用指定字符串生成矩阵
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        _StreamModel = new StringStreamModel(input);
        
        GenerateEditor();
    }

    /// <summary>
    /// 生成空字符矩阵
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// 生成图像
    /// </summary>
    private void GenerateEditor()
    {
        _StreamModel.Generate(ref g);
        _hasCanvas = true;

        gg.Height = _StreamModel._n * 48;
        gg.Width = _StreamModel._m * 23;
    }

    private bool isEditing = false;

    /// <summary>
    /// 现在是编辑模式
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangeEditMode(object sender, RoutedEventArgs e)
    {
        isEditing = true;
        
        ChangeModeBtn.IsChecked = true;
        ChangeModeBtn2.IsChecked = false;
    }

    /// <summary>
    /// 现在是拖拽模式
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangeEditMode2(object sender, RoutedEventArgs e)
    {
        isEditing = false;
        
        
        ChangeModeBtn.IsChecked = false;
        ChangeModeBtn2.IsChecked = true;
    }

    /// <summary>
    /// 复位画布
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void refresh(object sender, RoutedEventArgs e)
    {
        transformGroup.Children.Clear();
        Canvas.SetLeft(playground, 0);
        Canvas.SetTop(playground, 0);
    }

    /// <summary>
    /// 页面大小变化用来设置画布的大小改变
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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



    private void BrushSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // When the flyout part of the split button is opened and the user selects
        // an option, set their choice as the current color, apply it, then close the flyout.
        CurrentColorBrush = (SolidColorBrush)e.AddedItems[0];
        SelectedColorBorder.Background = CurrentColorBrush;
        ChangeColor();
        BrushFlyout.Hide();
    }

    private void ChangeColor()
    {
        try
        {
            if (brush_ch.Text == "") _brush.ch = ' ';
            else _brush.ch = Convert.ToChar(brush_ch.Text);
        }
        catch
        {
            MessageBox.Show("请输入正确笔刷字符", this);
        }
        _brush.foreground_color = CurrentColorBrush;
        ChangeEditMode(this, new RoutedEventArgs());
    }

    private void BrushButtonClick(object sender, object e)
    {
        // When the button part of the split button is clicked,
        // apply the selected color.
        ChangeColor();
    }

    
    private BrushesList _brushesList = new BrushesList();

    private void save_brush(object sender, RoutedEventArgs e)
    {
        brushes.IsExpanded = true;
        
        _brushesList.AddBrushToList(ref brushes_list, _brush);
    }

    private void del_brush(object sender, RoutedEventArgs e)
    {
        brushes_list.Items.Clear();
        _brushesList.Clear();
        brushes.IsExpanded = false;
        useWheelToSwitchBrush.IsOn = false;
    }

    private void brush_ch_TextSubmitted(object sender, object e)
    {
        try
        {
            if (brush_ch.Text == "") _brush.ch = ' ';
            else _brush.ch = Convert.ToChar(brush_ch.Text);
        }
        catch
        {
            brush_ch.Text = "";
            MessageBox.Show("请输入正确笔刷字符", this);
        }
    }

    private void brush_ch_TextSubmitted_1(ComboBox sender, ComboBoxTextSubmittedEventArgs args)
    {
        try
        {
            if (brush_ch.Text == "") _brush.ch = ' ';
            else _brush.ch = Convert.ToChar(brush_ch.Text);
        }
        catch
        {
            brush_ch.Text = "";
            MessageBox.Show("请输入正确笔刷字符", this);
        }
    }
}
