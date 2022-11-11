using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
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
using System.Runtime.InteropServices;
using static Windows.System.Launcher;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Image2ASCIIEditor.Views.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class Welcome : Page
{

    public Welcome()
    {
        this.InitializeComponent();
    }

    private void Start(object sender, RoutedEventArgs e)
    {
        
        MainWindow.frame.NavigateToType(typeof(ShowImage), null, null);
        MainWindow.welcome.IsSelected = false;
        MainWindow.showImage.IsSelected = true;

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        LaunchUriAsync(new Uri("https://github.com/ewoifuoi/Image2ASCIIEditor"));
    }
}
