using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Image2ASCIIEditor.Common;
using Image2ASCIIEditor.Models;
using Image2ASCIIEditor.Views.Pages;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.VisualBasic;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Console = Image2ASCIIEditor.Common.Console;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Image2ASCIIEditor.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ShowImage : Page
{
    public ShowImage()
    {
        this.InitializeComponent();
        transformGroup = testground.RenderTransform as TransformGroup;
        ImageModel.IMG.showImage(ref this.image);

        
    }

    private void OpenFile(object sender, RoutedEventArgs e)
    {
        // 启动后台线程 手动使 GetImgFile 和 showImage 异步
        // 使用 isRefreshed 判断是否成功打开文件
        BackgroundWorker worker = new BackgroundWorker();
        worker.DoWork += (s, e) => {
            //Some work...
            ImageModel.IMG.GetImgFile();
            while (ImageModel.IMG.isRefreshed == false) { Thread.Sleep(1000); };
        };
        worker.RunWorkerCompleted += (s, e) => {
            //e.Result"returned" from thread
            ImageModel.IMG.showImage(ref this.image);
            Console.log(ImageModel.IMG.ImagePath);
            

        };
        worker.RunWorkerAsync();
    }

    private bool _isMouseDown = false;
    private PointerPoint _mouseDownPosition;
    private Point _mouseDownControlPosition;
    private TransformGroup transformGroup;
    private Canvas _canvas;

    /// <summary>
    /// 生成像素矩阵
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GenerateBitMatrix(object sender, RoutedEventArgs e)
    {
        if(ImageModel.IMG.ImagePath == null)
        {
            MessageBox.Show("请先导入图片", this);
        }
        else
        {
            transformGroup.Children.Clear();
            Canvas.SetLeft(testground, 0);
            Canvas.SetTop(testground, 0);
            ImageModel.IMG.CreateBitmap(ref testground, Convert.ToInt32(Value.Value));
        }
        
    }

    private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void outside_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        PointerPoint currentPoint = e.GetCurrentPoint(outside);
        double s = ((double)currentPoint.Properties.MouseWheelDelta) / 1000 + 1;
        transformGroup.Children.Add(new ScaleTransform() { CenterX = outside.Width / 2, CenterY = outside.Height / 2, ScaleX = s, ScaleY = s });
    }

    private void playground_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        var c = sender as Canvas;
        _isMouseDown = true;
        _mouseDownPosition = e.GetCurrentPoint(outside);
        _mouseDownControlPosition = new Point(double.IsNaN(Canvas.GetLeft(c)) ? 0 : Canvas.GetLeft(c), double.IsNaN(Canvas.GetTop(c)) ? 0 : Canvas.GetTop(c));

        c.CapturePointer(e.Pointer);
    }

    private void playground_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        var c = sender as Canvas;
        _isMouseDown = false;
        c.ReleasePointerCapture(e.Pointer);
    }

    private void playground_PointerMoved(object sender, PointerRoutedEventArgs e)
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

    private void EraseWhiteBackground(object sender, RoutedEventArgs e)
    {
        if(ImageModel.IMG.RectangleList == null)
        {
            MessageBox.Show("请先生成像素矩阵", this);
        }
        ImageModel.IMG.EraseWhiteBackground(ref testground, Convert.ToInt32(eraseWhiteRate.Value));
    }
}
