using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Image2ASCIIEditor.Common;
using Console = Image2ASCIIEditor.Common.Console;

namespace Image2ASCIIEditor.Models;
public class ImageModel
{
    public static ImageModel IMG = null;
    public SoftwareBitmap InputIMG;
    private int Width;
    private int Height;
    private Image imageControl;
    

    /// <summary>
    /// 将图片在界面显示
    /// </summary>
    /// <param name="img"></param>
    public void showImage(ref Image img)
    {
        try
        {
            imageControl = img;
            CreateBitMap();
        }
        catch
        {
            Console.log("图片载入失败");
        }
        
    }

    private async void CreateBitMap()
    {
        if (InputIMG.BitmapPixelFormat != BitmapPixelFormat.Bgra8 ||
    InputIMG.BitmapAlphaMode == BitmapAlphaMode.Straight)
        {
            InputIMG = SoftwareBitmap.Convert(InputIMG, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
        }

        var source = new SoftwareBitmapSource();
        await source.SetBitmapAsync(InputIMG);

        // Set the source of the Image control
        imageControl.Source = source;
    }


}
