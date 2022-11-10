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
using Microsoft.UI.Xaml.Shapes;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

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
    private static StringStreamModel _StreamModel;
    private TransformGroup transformGroup;
    private List<SolidColorBrush> ColorOptions = new List<SolidColorBrush>();
    private SolidColorBrush CurrentColorBrush = null;
    private bool _hasCanvas = false;
    private static bool has_streamModel = false;

    private double _el_x1;
    private double _el_y1;
    private double _el_x2;
    private double _el_y2;

    public EditText()
    {
        this.InitializeComponent();
        
        transformGroup = playground.RenderTransform as TransformGroup;
        _brush = new Brush('*', new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Black));


        ColorOptions.Add(new SolidColorBrush(Colors.Red));
        ColorOptions.Add(new SolidColorBrush(Colors.Yellow));
        ColorOptions.Add(new SolidColorBrush(Colors.Cyan));
        ColorOptions.Add(new SolidColorBrush(Colors.Magenta));
        ColorOptions.Add(new SolidColorBrush(Colors.Blue));
        ColorOptions.Add(new SolidColorBrush(Colors.White));
        ColorOptions.Add(new SolidColorBrush(Colors.Green));
        ColorOptions.Add(new SolidColorBrush(Colors.DarkRed));
        ColorOptions.Add(new SolidColorBrush(Colors.DarkCyan));
        ColorOptions.Add(new SolidColorBrush(Colors.DarkMagenta));
        ColorOptions.Add(new SolidColorBrush(Colors.DarkBlue));
        ColorOptions.Add(new SolidColorBrush(Colors.DarkGreen));
        ColorOptions.Add(new SolidColorBrush(Colors.Gray));


        if (has_streamModel && _StreamModel != null)
        {
            has_streamModel = false;
            _StreamModel.Generate(ref g);
            _hasCanvas = true;

            gg.Height = _StreamModel._n * 48;
            gg.Width = _StreamModel._m * 23;
        } 
    }

    private void outside_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        PointerPoint currentPoint = e.GetCurrentPoint(outside);
        double s = ((double)currentPoint.Properties.MouseWheelDelta) / 1000 + 1;
        transformGroup.Children.Add(new ScaleTransform() { CenterX = outside.Width / 2, CenterY = outside.Height / 2, ScaleX = s, ScaleY = s });

    }

    public static void GetStringFromImage(ref StringStreamModel res)
    {
       _StreamModel = res;
        has_streamModel = true;
        
    }

    private void playground_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        _isMouseDown = true;
        if (!isEditing)
        {
            var c = sender as Canvas;
            _mouseDownPosition = e.GetCurrentPoint(outside);
            _mouseDownControlPosition = new Point(double.IsNaN(Canvas.GetLeft(c)) ? 0 : Canvas.GetLeft(c), double.IsNaN(Canvas.GetTop(c)) ? 0 : Canvas.GetTop(c));
            c.CapturePointer(e.Pointer);
        }
        else if(_hasCanvas)
        {
            // 计算正在点击的坐标
            
            PointerPoint p = e.GetCurrentPoint(playground);
            double x = p.Position.X; double y = p.Position.Y;
            //Console.log("click x:" + x.ToString() + " " + "y:" + y.ToString());
            int nx = Convert.ToInt32(Math.Floor(x / 23)); int ny = Convert.ToInt32(Math.Floor(y / 48));
            
            if (x - nx * 23 <= 20 && y - ny * 48 <= 45)
            {
                if (toolbar_drawPoint.IsSelected)
                {
                    _StreamModel.Paint(ny, nx, _brush);
                }
                else if (toolbar_drawLine.IsSelected) // 启动绘制直线
                {
                    Ellipse el = new Ellipse() { Width = 20, Height = 20, Fill = new SolidColorBrush(Colors.Aqua) };
                    Canvas.SetLeft(el, nx * 23 + 11 - 10 ); Canvas.SetTop(el, ny * 48 + 24 - 10);
                    canvas_showLine.Children.Add(el);
                    _el_x1 = nx * 23 + 11;
                    _el_y1 = ny * 48 + 24;
                }              
            }
        }
    }

    private void playground_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        _isMouseDown = false;
        if (!isEditing)
        {
            var c = sender as Canvas;
            
            c.ReleasePointerCapture(e.Pointer);
        }
        else if (_hasCanvas)
        {
            if (toolbar_drawLine.IsSelected) // 直线的绘图判断
            {
                canvas_showLine.Children.Clear();
                int y1 = Math.Max(Convert.ToInt32((_el_y1 - 24) / 48), Convert.ToInt32((_el_y2 - 24) / 48));
                int y2 = Math.Min(Convert.ToInt32((_el_y1 - 24) / 48), Convert.ToInt32((_el_y2 - 24) / 48));
                int x1 = Math.Max(Convert.ToInt32((_el_x1 - 11) / 23), Convert.ToInt32((_el_x2 - 11) / 23));
                int x2 = Math.Min(Convert.ToInt32((_el_x1 - 11) / 23), Convert.ToInt32((_el_x2 - 11) / 23));
                if (_el_y1 == _el_y2)
                {
                    
                    for (int i = 0; i < x1 -x2 + 1 ; i++)
                    {
                        _StreamModel.Paint(Convert.ToInt32((_el_y1 - 24) / 48), i + x2, _brush);
                    }
                }
                else if(_el_x1 == _el_x2)
                {
                    
                    for (int i = 0; i < y1 - y2 + 1; i++)
                    {
                        _StreamModel.Paint( i + y2, Convert.ToInt32((_el_x1 - 11) / 23), _brush);
                    }
                }
                else if(x1 - x2 == y1 - y2)
                {
                    if((_el_x1 - _el_x2) * (_el_y1 - _el_y2) < 0)
                    {
                        for (int i = 0; i < x1 - x2 + 1; i++)
                        {
                            for (int j = 0; j < y1 - y2 + 1; j++)
                            {
                                if (i + j == y1 - y2) _StreamModel.Paint(i + y2, j + x2, _brush);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < x1 - x2 + 1; i++)
                        {
                            for (int j = 0; j < y1 - y2 + 1; j++)
                            {
                                if (i == j) _StreamModel.Paint(i + y2, j + x2, _brush);
                            }
                        }
                    }
                    
                }
                
                _el_x1 =0;
                _el_y1=0;
                _el_x2=0;
                _el_y2=0;
            }
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
        else if (_hasCanvas)
        {
            if (_isMouseDown) 
            {
                PointerPoint p = e.GetCurrentPoint(playground);
                double x = p.Position.X; double y = p.Position.Y;
                //Console.log("click x:" + x.ToString() + " " + "y:" + y.ToString());
                int nx = Convert.ToInt32(Math.Floor(x / 23)); int ny = Convert.ToInt32(Math.Floor(y / 48));

                if (x - nx * 23 <= 20 && y - ny * 48 <= 45)
                {
                    if (toolbar_erase.IsSelected) // 橡皮的拖拽逻辑
                    {
                        _StreamModel.Erase(ny, nx);
                    }
                    else if (toolbar_draw.IsSelected) // 连续绘制拖拽逻辑
                    {
                        _StreamModel.Paint(ny, nx, _brush);
                    }
                    else if (toolbar_drawLine.IsSelected) // 绘制直线时,提示直线的生成逻辑
                    {
                        canvas_showLine.Children.Clear();
                        Ellipse el2 = new Ellipse() { Width = 20, Height = 20, Fill = new SolidColorBrush(Colors.Aqua) };
                        Canvas.SetLeft(el2, _el_x1 - 10); Canvas.SetTop(el2, _el_y1 - 10);
                        canvas_showLine.Children.Add(el2);
                        Ellipse el1 = new Ellipse() { Width = 20, Height = 20, Fill = new SolidColorBrush(Colors.Aqua) };
                        Canvas.SetLeft(el1, x - 10); Canvas.SetTop(el1, y - 10);
                        canvas_showLine.Children.Add(el1);
                        Line el = new Line() { X1=_el_x1,Y1=_el_y1, X2 = x, Y2 = y};
                        el.StrokeThickness = 5;el.Stroke = new SolidColorBrush(Colors.Aqua);
                        
                        canvas_showLine.Children.Add(el);
                        _el_x2 = nx * 23 + 11;
                        _el_y2 = ny * 48 + 24;
                    } 
                }  
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
            outside1.Height = 700;
            outside1.Width = 1000;
            outside2.Height = 700;
            outside2.Width = 1000;
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
            outside1.Height = 450;
            outside1.Width = 450;
            outside2.Height = 450;
            outside2.Width = 450;
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
        //:ChangeEditMode(this, new RoutedEventArgs());
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

    /// <summary>
    /// 点击工具条
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangeEditMode3(object sender, RoutedEventArgs e)
    {
        if(toolbar.IsExpanded == false)
        {
            toolbar.IsExpanded = true;
            isEditing = true;
            ChangeModeBtn.IsChecked = true;
            ChangeModeBtn2.IsChecked = false;
        }
        else
        {
            toolbar.IsExpanded = false;
        }
    }

    private void playground_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        _isMouseDown = false;
        canvas_showLine.Children.Clear();
    }

    [DllImport("Kernel32")]
    public static extern void AllocConsole();

    [DllImport("Kernel32")]
    public static extern void FreeConsole();

    private void AppBarButton_Click(object sender, RoutedEventArgs e)
    {
        BackgroundWorker worker = new BackgroundWorker();// 新建一后台线程
        worker.DoWork += (s, e) => {
            if (StringStreamModel.charsList == null)
            {
                return;
            }
            AllocConsole(); // 为调用进程分配一个新的控制台。
            System.Console.WriteLine("当前效果预览 :\n");
            
            for(int j = 0; j < StringStreamModel.charsList.Count; j++)
            {
                for(int i = 0; i < StringStreamModel.charsList[j].Count; i++)
                {
                    System.Console.ForegroundColor = (ConsoleColor)StringStreamModel.colorList[j][i];
                    System.Console.Write(StringStreamModel.charsList[j][i]);
                    System.Console.ResetColor();
                }
                System.Console.Write('\n');
            }
            System.Console.WriteLine("按任意键继续");
            System.Console.Read();
            
        };
        worker.RunWorkerCompleted += (s, e) => { // 释放后台线程
            //e.Result"returned" from thread
            FreeConsole();

        };
        worker.RunWorkerAsync();
    }

    private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
    {
        MainWindow.frame.NavigateToType(typeof(ShowImage), null, null);
        MainWindow.editText.IsSelected = false;
        MainWindow.showImage.IsSelected = true;
    }

    private async void AppBarButton_Click_2(object sender, RoutedEventArgs e)//导出逻辑
    {

        if(_StreamModel == null || StringStreamModel.charsList == null)
        {
            MessageBox.Show("请先生成画布", this);
            return;
        }
        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "选择导出格式";
        dialog.PrimaryButtonText = "确定";
        dialog.CloseButtonText = "取消";
        dialog.DefaultButton = ContentDialogButton.Primary;
        RadioButtons selection = null;
        dialog.Content = new ExportPage(ref selection);

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            int op = selection.SelectedIndex;
            if(op == -1) MessageBox.Show("请选择导出格式!", this);
            else
            {
                ExportModel exportmodel = new ExportModel(op);
            }
        }
        else
        {
            return;
        }

    }

    private void clear_canvas(object sender, RoutedEventArgs e)
    {
        g.Children.Clear();
        _StreamModel = null;
        StringStreamModel.charsList = null;
        StringStreamModel.colorList = null;
    }

    private void refresh_canvas(object sender, RoutedEventArgs e)
    {
       
    }
}
