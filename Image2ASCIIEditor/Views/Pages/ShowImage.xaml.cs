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
        ImageModel.IMG.showImage(ref this.image);
        Console.console = this.console;
        
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

    private void Next(object sender, RoutedEventArgs e)
    {
        MainWindow.showImage.IsSelected = false;
        MainWindow.editText.IsSelected = true;
        MainWindow.frame.NavigateToType(typeof(EditText), null, null);
    }

    private bool t = false;
    private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        
        if (!t) t = true;
        else
        {
            
        }
        
    }

    private void GenerateBitMatrix(object sender, RoutedEventArgs e)
    {
        ImageModel.IMG.CreateBitmap(ref testground, Convert.ToInt32(Value.Value));
    }
}
