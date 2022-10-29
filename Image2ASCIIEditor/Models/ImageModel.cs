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
using System.Formats.Asn1;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Image2ASCIIEditor.Models;
public class ImageModel
{
    public static ImageModel IMG = null;
    public SoftwareBitmap InputIMG = null;
    private int Width;
    private int Height;
    private bool Refreshed = false;
    private Image imageControl;
    
    public bool isRefreshed{get { return Refreshed;}}

    /// <summary>
    /// 将图片在界面显示
    /// </summary>
    /// <param name="img"></param>
    public void showImage(ref Image img)
    {
        if (InputIMG == null) return;
        imageControl = img;
        CreateBitMap();     
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
        Refreshed = false;
    }

    public async void GetImgFile()
    {
        var picker = new FileOpenPicker();
        // 传入MainWindow实例，获取窗口句柄。

        // 将句柄用于初始化Picker。
        WinRT.Interop.InitializeWithWindow.Initialize(picker, MainWindow.hWnd);
        picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        picker.ViewMode = PickerViewMode.Thumbnail;
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".jpeg");
        picker.FileTypeFilter.Add(".png");
        
        // 打开文件选择对话框
        var file = await picker.PickSingleFileAsync();

        // 用户选择图片
        if (file != null)
        {
            using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                // Get the SoftwareBitmap representation of the file
                SoftwareBitmap bitmapImage = await decoder.GetSoftwareBitmapAsync();
                ImageModel.IMG.InputIMG = bitmapImage;
                Refreshed = true;
            }
        }
    }




}
