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
using Image2ASCIIEditor.ViewModels;
using Image2ASCIIEditor.Models;
using Image2ASCIIEditor.Views.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Image2ASCIIEditor.Views;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Editor : Window
{
    private EditorViewModel viewModel;
    public Editor()
    {
        
        this.InitializeComponent();
        viewModel = new EditorViewModel(this);
        this.ExtendsContentIntoTitleBar = true;  // enable custom titlebar
        this.SetTitleBar(AppTitleBar);
    }

    private void nvSample_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        FrameNavigationOptions options = new FrameNavigationOptions();
        options.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;

        string navItemTag = args.InvokedItemContainer.Tag.ToString();
        Type pageType = null;

        if (navItemTag == "ShowImage" && ShowImage.IsSelected==false)
        {
            pageType = typeof(ShowImage);
            contentFrame.NavigateToType(pageType, null, options);
        }
        if (navItemTag == "EditText" && EditText.IsSelected == false)
        {
            pageType = typeof(EditText);
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

    private void BackEventHandler(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        var t = new MainWindow();
        t.Activate();
        this.Close();
    }
}
