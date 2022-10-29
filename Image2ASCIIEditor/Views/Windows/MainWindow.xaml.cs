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

    public MainWindow()
    {
        this.InitializeComponent();
        welcome = this.Welcome;
        frame = this.contentFrame;
        showImage = this.ShowImage;
        viewModel = new MainWindowViewModel(this);
        grid.DataContext = viewModel;

        this.ExtendsContentIntoTitleBar = true;  // enable custom titlebar
        this.SetTitleBar(AppTitleBar);

        Welcome.IsSelected = true;
        

        contentFrame.NavigateToType(typeof(Welcome), null, null);



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

}
