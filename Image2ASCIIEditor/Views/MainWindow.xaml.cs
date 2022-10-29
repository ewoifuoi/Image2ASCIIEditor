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
using System.Security.AccessControl;

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


    public MainWindow()
    {
        this.InitializeComponent();
        viewModel = new MainWindowViewModel(this);
        grid.DataContext = viewModel;

        MainWindow.hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        this.ExtendsContentIntoTitleBar = true;  // enable custom titlebar
        this.SetTitleBar(AppTitleBar);
        Console.console = console;
        Console.log("开始测试");
        contentFrame.NavigateToType(typeof(Start), null, null);


    }

    private void nvSample_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        FrameNavigationOptions options = new FrameNavigationOptions();
        options.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;

        string navItemTag = args.InvokedItemContainer.Tag.ToString();
        Type pageType = null;

        if (navItemTag == "Start")
        {
            pageType = typeof(Start);
        }

        if (pageType == null)
        {
            return;
        }
        contentFrame.NavigateToType(pageType, null, options);
    }
}
