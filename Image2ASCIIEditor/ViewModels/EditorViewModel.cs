using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image2ASCIIEditor.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Image2ASCIIEditor.ViewModels;
public class EditorViewModel
{
    private Window window;
    private IntPtr hwnd;
    public EditorViewModel(Window window)
    {
        this.window = window;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        this.hwnd = hWnd;
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
        appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 1800, Height = 1400 });
        
    }
    public void setImage(ref Image img)
    {
        img.Source = ImageHelper.InputIMG;
    }
}
