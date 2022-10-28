using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Image2ASCIIEditor.Common;
using Console = Image2ASCIIEditor.Common.Console;
using Image2ASCIIEditor.Models;
using Microsoft.UI.Xaml;

namespace Image2ASCIIEditor.ViewModels;

public class MainWindowViewModel : DependencyObject
{
    public MainWindowViewModel(IntPtr hwnd)
    {
        this.hwnd = hwnd;
    }

    private IntPtr hwnd;



    public string path
    {
        get
        {
            return (string)GetValue(pathProperty);
        }
        set
        {
            SetValue(pathProperty, value);
        }
    }

    // Using a DependencyProperty as the backing store for path.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty pathProperty =
        DependencyProperty.Register("path", typeof(string), typeof(MainWindowViewModel), new PropertyMetadata(""));



    public async void GetImgFile()
    {
        try
        {
            var picker = new FileOpenPicker();
            Console.log(picker.ToString());
            // 传入MainWindow实例，获取窗口句柄。
            
            Console.log(hwnd.ToString());
            // 将句柄用于初始化Picker。
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            // 正常使用Picker。
            var file = await picker.PickSingleFileAsync();
            if(file != null)
            {
                ImageHelper.Path = file.Path;
                path = file.Path;
                Console.log(file.Path.ToString());
            }
           

        }
        catch
        {
            Console.log("文件对话框打开失败");
        }



    }


}
