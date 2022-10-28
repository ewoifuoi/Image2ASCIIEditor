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

namespace Image2ASCIIEditor.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AddImage : Page
{
    public AddImage()
    {
        this.InitializeComponent();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        int n = int.Parse(tb.Text);
        var l = new ListBoxItem();
        l.Content = new TextBlock().Text= DateTime.Now.ToString("T") + " " + $"创建成功 n = {n}";
        lb.Items.Add(l);
        g.ColumnDefinitions.Clear();
        g.RowDefinitions.Clear();
        g.Children.Clear();
        for(int i = 0; i < n; i++)
        {
            g.ColumnDefinitions.Add(new ColumnDefinition());
            g.RowDefinitions.Add(new RowDefinition());
        }
        for(int i = 0; i < n; i++)
        {
            for(int j=0; j < n; j++)
            {
                
                var b = new Button();
                b.Background = new SolidColorBrush(p.Color);
                b.Content = n.ToString();
                b.Width = g.Width / n;
                b.Height = g.Height / n;
                b.Click += B_Click;
                Grid.SetColumn(b, i);
                Grid.SetRow(b, j);
                Grid.SetRowSpan(b,1);Grid.SetColumnSpan(b,1);
                g.Children.Add(b);

            }
        }
        
    }

    private void B_Click(object sender, RoutedEventArgs e)
    {
        var b = sender as Button;
        b.Background = new SolidColorBrush(p.Color);
    }
}
