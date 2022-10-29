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
        appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = appWindow.Size.Width * 4 / 5, Height = appWindow.Size.Height });
        ImageModel.IMG = new ImageModel();
    }

    private IntPtr hwnd;
    private Window window;
    
    
}
