using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Image2ASCIIEditor.Models;
public class ImageModel
{
    public static ImageModel IMG = null;
    public BitmapImage InputIMG;
    private int Width;
    private int Height;

    /// <summary>
    /// 将图片在界面显示
    /// </summary>
    /// <param name="img"></param>
    public void showImage(ref Image img)
    {
        img.Source = InputIMG;
    }


}
