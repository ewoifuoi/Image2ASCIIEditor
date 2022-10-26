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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Image2ASCIIEditor;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        
        this.ExtendsContentIntoTitleBar = true;  // enable custom titlebar
        this.SetTitleBar(AppTitleBar);

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
        contentFrame.NavigateToType(pageType, null, options);
    }
}
