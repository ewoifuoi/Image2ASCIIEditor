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
using Image2ASCIIEditor.Views;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.IO;
using Windows.Graphics.Imaging;

namespace Image2ASCIIEditor.ViewModels;

public class MainWindowViewModel : DependencyObject
{
    public MainWindowViewModel(Window window)
    {
        this.window = window;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        this.hwnd = hWnd;
        MainWindow.hWnd = hWnd;
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
        appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 1400, Height = 1000 });
        
    }

    private IntPtr hwnd;
    private Window window;
    






    public async void GetImgFile()
    {
        try
        {
            ImageModel.IMG = new ImageModel();
        }
        catch
        {
            Console.log("图片对象创建失败!");
        }


        try
        {
            var picker = new FileOpenPicker();
            Console.log(picker.ToString());
            // 传入MainWindow实例，获取窗口句柄。
            
            Console.log(hwnd.ToString());
            // 将句柄用于初始化Picker。
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            // 正常使用Picker。
            var file = await picker.PickSingleFileAsync();
            
            if(file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    

                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);

                    // Get the SoftwareBitmap representation of the file
                    SoftwareBitmap bitmapImage = await decoder.GetSoftwareBitmapAsync();
                    ImageModel.IMG.InputIMG = bitmapImage;
                }

                Console.log(file.Name.ToString());
                var t = new Editor();
                t.Activate();
                window.Close();
            }
           

        }
        catch
        {
            Console.log("文件对话框打开失败");
        }



    }



}
