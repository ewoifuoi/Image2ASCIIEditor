using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Controls;

namespace Image2ASCIIEditor.Common;
public static class Console
{
    public static ListView console = null;

    public static void log(string content)
    {
        if(console != null)
        {
            ListBoxItem l = new ListBoxItem();
            l.Content = new TextBlock().Text = DateTime.Now.ToString("T") + " : " + content;
            console.Items.Add(l);
        }
         
    }
}
