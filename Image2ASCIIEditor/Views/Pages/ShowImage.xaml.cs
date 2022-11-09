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
using Windows.UI;
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
        Console.console = this.console;
        transformGroup = testground.RenderTransform as TransformGroup;
        ImageModel.IMG.showImage(ref this.image);
        Console.log("开始测试");

        
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
            
            ColorClusterStackPanel.Children.Clear();
            ColorClusterExpander.IsExpanded = false;
            
            
            Canvas.SetLeft(testground, 0);
            Canvas.SetTop(testground, 0);
            ImageModel.IMG.CreateBitmap(ref testground, Convert.ToInt32(Value.Value));
            ColorClusterExpander.IsEnabled = false;
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

    List<List<Rectangle>> k_means_rectangles;
    private void Generate_K_Means(object sender, RoutedEventArgs e)
    {
        if (ImageModel.IMG.RectangleList == null)
        {
            MessageBox.Show("请先生成像素矩阵", this);
        }
        else if (kind_of_color.SelectedItem == null)
        {
            MessageBox.Show("请选择使用颜色种类", this);
        }
        else 
        {
            Console.log("开始聚类");
            Cluster k_Means = new Cluster();
            k_means_rectangles = k_Means.Execute(ImageModel.IMG.RectangleList, Convert.ToInt32(kind_of_color.SelectedValue.ToString()));
            Console.log(k_means_rectangles[0].Count.ToString());

            

            ColorClusterExpander.IsEnabled = true;
            ColorClusterExpander.IsExpanded = true;
            ColorClusterStackPanel.Children.Clear();

            colorList = new List<ComboBox>();
            charList = new List<ComboBox>();
            rectList = new List<Rectangle>();
            selectColors = new List<Color>();
            for (int i = 0; i < Convert.ToInt32(kind_of_color.SelectedValue); i++)
            {
                StackPanel s = new StackPanel() { Width = 250, Height = 30, Margin = new Thickness(2), Orientation = Orientation.Horizontal };
                //s.Background = new SolidColorBrush(Colors.Aqua);
                ComboBox cb = new ComboBox() { Margin=new Thickness(25,0,25,0), FontSize=12, Width=95,Height=30 };
                cb.SelectionChanged += Cb_SelectionChanged;
                TextBlock tb = new TextBlock() {Padding=new Thickness(0,5,0,0), Text="聚类 "+i.ToString()+" :"};
                ComboBox Editcb = new ComboBox() { IsEditable = true, Width = 65, Height = 30 };
                Editcb.TextSubmitted += Char_TextSubmitted;
                selectColors.Add(Colors.Red);
                selectColors.Add(Colors.Green);
                selectColors.Add(Colors.Yellow);
                selectColors.Add(Colors.Blue);
                selectColors.Add(Colors.Magenta);
                selectColors.Add(Colors.Cyan);
                selectColors.Add(Colors.White);
                cb.Items.Add(new string("Red"));
                cb.Items.Add(new string("Green"));
                cb.Items.Add(new string("Yellow"));
                cb.Items.Add(new string("Blue"));
                cb.Items.Add(new string("Magenta"));
                cb.Items.Add(new string("Cyan"));
                cb.Items.Add(new string("White"));
                Rectangle rect = new Rectangle() { Margin = new Thickness(0, 0, 5, 0), Height =30,Width=30,Stroke=new SolidColorBrush(Colors.Gray),StrokeThickness=2};
                
                s.Children.Add(tb);
                s.Children.Add(cb);
                s.Children.Add(rect);
                
                ColorClusterStackPanel.Children.Add(s);
                colorList.Add(cb);charList.Add(Editcb);rectList.Add(rect);
                
            }
            Button b = new Button() { Content="使用字符替换", Width = 250, Height = 60, Margin = new Thickness(5) };
            b.Click += Generate_char;
            ColorClusterStackPanel.Children.Add(b);
        }
    }

    public StringStreamModel res;
    private async void Generate_char(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "提示";
        dialog.PrimaryButtonText = "确定";
        dialog.CloseButtonText = "取消";
        dialog.DefaultButton = ContentDialogButton.Primary;
        dialog.Content = "是否使用字符'+'对像素矩阵进行替换？（此过程不可逆）";

        var result = await dialog.ShowAsync();
       
        if(result != ContentDialogResult.Primary)
        {
            return;
        }
        res = new StringStreamModel(ImageModel.IMG.RectangleList.Count, ImageModel.IMG.RectangleList[0].Count);
        for(int i = 0; i < ImageModel.IMG.RectangleList.Count; i++)//40
        {
            for(int j = 0; j < ImageModel.IMG.RectangleList[i].Count; j++)//20
            {
                SolidColorBrush scb = ImageModel.IMG.RectangleList[i][j].Fill as SolidColorBrush;
                res.Paint(i, j, new Models.Brush('+', scb, new SolidColorBrush(Colors.Black)));
            }
        }
        //res.Generate(ref testground);
        
    }

    private void Cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        bool isColorChanged = false;
        int ColorChangedInd = -1;SolidColorBrush tempFill = null;
        for (int i = 0; i < Convert.ToInt32(kind_of_color.SelectedValue); i++)
        {
            if (colorList[i].SelectedValue != null)
            {
                int ind = colorList[i].SelectedIndex;
                SolidColorBrush scb = rectList[i].Fill as SolidColorBrush;
                if(scb == null  || scb.Color != selectColors[ind])
                {
                    isColorChanged = true;
                    ColorChangedInd = i;
                }
                tempFill = new SolidColorBrush(selectColors[ind]);
                rectList[i].Fill = new SolidColorBrush(selectColors[ind]);
                
            }
            else
            {
                rectList[i].Fill = new SolidColorBrush(Colors.Transparent);
            }
        }
        if (isColorChanged)
        {
            isColorChanged = false;
            for(int i = 0; i < k_means_rectangles[ColorChangedInd].Count; i++)
            {
                k_means_rectangles[ColorChangedInd][i].Fill = tempFill;
            }
        }

    }

    private List<ComboBox> colorList;
    private List<ComboBox> charList;
    private List<Rectangle> rectList;
    private List<Color> selectColors;

    private void Char_TextSubmitted(ComboBox sender, ComboBoxTextSubmittedEventArgs args)
    {
        
    }

    private void exportToEditor(object sender, RoutedEventArgs e)
    {
        EditText.GetStringFromImage(ref res);
        MainWindow.frame.NavigateToType(typeof(EditText), null, null);
        MainWindow.showImage.IsSelected = false;
        MainWindow.editText.IsSelected = true;
    }
}
