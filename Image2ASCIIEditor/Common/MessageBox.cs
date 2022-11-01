using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace Image2ASCIIEditor.Common;
public class MessageBox
{
    public static void Show(String content, UIElement u)
    {
        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = u.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "提示";
        dialog.Content = content;
        dialog.PrimaryButtonText = "确定";
        dialog.DefaultButton = ContentDialogButton.Primary;
        var result = dialog.ShowAsync();

    }
}
