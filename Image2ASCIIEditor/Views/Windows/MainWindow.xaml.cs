using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Image2ASCIIEditor.ViewModels;
using Image2ASCIIEditor.Views;
using Image2ASCIIEditor.Views.Pages;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI;
using Microsoft.UI.Windowing;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Image2ASCIIEditor;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public partial class MainWindow : Window
{

    MainWindowViewModel viewModel;
    public static IntPtr hWnd;
    public static Frame frame;
    public static NavigationViewItem welcome;
    public static NavigationViewItem showImage;
    public static NavigationViewItem editText;

    [DllImport("User32", CharSet = CharSet.Unicode)]
    static extern Boolean SetLayeredWindowAttributes(IntPtr hwnd,uint crKey,byte bAlpha, uint dwFlags);

    [DllImport("User32", CharSet = CharSet.Unicode)]
    static extern uint SetWindowLongA(IntPtr hwnd, int nIndex, uint dwNewLong);

    [DllImport("User32", CharSet = CharSet.Unicode)]
    static extern uint GetWindowLongA(IntPtr hwnd, int dwNewLong);

    public MainWindow()
    {
        
        this.InitializeComponent();

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        var apw = AppWindow.GetFromWindowId(myWndId).Presenter as OverlappedPresenter;
        apw.IsResizable = false;


        viewModel = new MainWindowViewModel(this);

        this.ExtendsContentIntoTitleBar = true;  // enable custom titlebar
        this.SetTitleBar(AppTitleBar);

        welcome = this.Welcome;
        frame = this.contentFrame;
        editText = this.EditText;
        showImage = this.ShowImage;
        grid.DataContext = viewModel;

        var wh = new Acrylic();
        wh.TrySetAcrylicBackdrop(ref window);
        
        


        

        Welcome.IsSelected = true;
        contentFrame.NavigateToType(typeof(Welcome), null, null);


        if(SetWindowLongA(MainWindow.hWnd, -20, GetWindowLongA(hWnd, -20) | 0x00080000) == 0)
        {
            Console.log("窗体拓展样式设置失败");
        }

        if (SetLayeredWindowAttributes(MainWindow.hWnd, 0, 255, 0x00000002))
        {

            Console.log("透明度设置成功");
        }
        else
        {
            Console.log("透明度设置失败！");
            Console.log(MainWindow.hWnd.ToString());
        }


    }

    private void nvSample_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        FrameNavigationOptions options = new FrameNavigationOptions();
        options.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;

        string navItemTag = args.InvokedItemContainer.Tag.ToString();
        Type pageType = null;

        if (navItemTag == "ShowImage" && ShowImage.IsSelected == false)
        {
            pageType = typeof(ShowImage);
            contentFrame.NavigateToType(pageType, null, options);
        }
        if (navItemTag == "EditText" && EditText.IsSelected == false)
        {
            pageType = typeof(EditText);
            contentFrame.NavigateToType(pageType, null, options);
        }
        if (navItemTag == "Welcome" && Welcome.IsSelected == false)
        {
            pageType = typeof(Welcome);
            contentFrame.NavigateToType(pageType, null, options);
        }
        if (navItemTag == "Web")
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("https://github.com/ewoifuoi/Image2ASCIIEditor"));
        }

        if (pageType == null)
        {
            return;
        }

    }

    private void window_SizeChanged(object sender, WindowSizeChangedEventArgs args)
    {
        if(App.isMax)
        {
            App.isMax = false;
        }
        else
        {
            App.isMax = true;
        }
    }
}
