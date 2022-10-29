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

        this.ExtendsContentIntoTitleBar = true;  // enable custom titlebar
        this.SetTitleBar(AppTitleBar);
        Console.console = console;
        

        Console.log("开始测试");



    }



    private void UseIMG(object sender, RoutedEventArgs e)
    {
        viewModel.GetImgFile();
        
    }

    private void Generate(object sender, RoutedEventArgs e)
    {

    }
    
}
