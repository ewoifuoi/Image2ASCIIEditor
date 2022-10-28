using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Image2ASCIIEditor.Views;
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
using System.Runtime.InteropServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Image2ASCIIEditor;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public partial class MainWindow : Window
{
    [DllImport("User32", CharSet = CharSet.Unicode)]
    static extern IntPtr GetSystemMetrics(int nIndex);


    public MainWindow()
    {
        this.InitializeComponent();

        Console.console = this.lv;
        this.ExtendsContentIntoTitleBar = true;  // enable custom titlebar
        this.SetTitleBar(AppTitleBar);
        IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
        appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 1400, Height = 1000});

        Console.log("开始测试");



    }



    private void nvSample_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        FrameNavigationOptions options = new FrameNavigationOptions();
        options.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
        
        string navItemTag = args.InvokedItemContainer.Tag.ToString();
        Type pageType = null;

        if(navItemTag == "AddImage")
        {
            pageType = typeof(AddImage);
        }

        if(pageType == null)
        {
            return;
        }
        
    }
}
