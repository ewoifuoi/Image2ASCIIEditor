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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Image2ASCIIEditor.Views.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ExportPage : Page
{
    public ExportPage()
    {
        this.InitializeComponent();
    }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {

    }

    private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
    {
        showENV1.IsOpen = true;
    }

    private void HyperlinkButton_Click2(TeachingTip sender, object args)
    {
        showENV1.IsOpen = false;
        showENV2.IsOpen = true;
    }

    private void HyperlinkButton_Click3(TeachingTip sender, object args)
    {
        showENV2.IsOpen = false;
        
    }

    
}
