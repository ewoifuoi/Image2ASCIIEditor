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
using System.Drawing;
using Image = Microsoft.UI.Xaml.Controls.Image;
using Microsoft.UI.Xaml.Shapes;
using Rectangle = Microsoft.UI.Xaml.Shapes.Rectangle;
using Microsoft.UI.Xaml.Media;

namespace Image2ASCIIEditor.Models;
public class ImageModel
{
    public static ImageModel IMG = null;
    public SoftwareBitmap InputIMG = null;
    private int Width;
    private int Height;
    private bool Refreshed = false;
    private Image imageControl;
    public string ImagePath;

    public List<List<Windows.UI.Color>> PixelList; 


    public void CreateBitmap(ref Canvas canvas, int rate)
    {
        Color srcColor;
        Bitmap srcBitmap = new Bitmap(ImagePath);
        // 设置目标图片高度时需除2
        Bitmap imageBitmap = new Bitmap(rate, Convert.ToInt32(Convert.ToDouble(srcBitmap.Height) * (Convert.ToDouble(rate) / Convert.ToDouble(srcBitmap.Width))) / 2);
        Graphics g = Graphics.FromImage(imageBitmap);
        g.DrawImage(srcBitmap, new System.Drawing.Rectangle(0, 0, imageBitmap.Width, imageBitmap.Height), new System.Drawing.Rectangle(0, 0,srcBitmap.Width, srcBitmap.Height),  GraphicsUnit.Pixel);

        //var imageBitmap = new Bitmap(ImagePath);
        Console.log(imageBitmap.Size.ToString());


        canvas.Children.Clear();
        for(int i = 0; i < imageBitmap.Height; i++)
        {
            for(int j = 0; j < imageBitmap.Width; j++)
            {
                // 注 ：j为横坐标， i为纵坐标

                // 目标图片高度已除2, 故不需改变矩形的高(矩形的高由计算得出)
                Rectangle rect = new Rectangle();// 生成在画布里的矩形
                if(imageBitmap.Height * 2 > imageBitmap.Width)
                {
                    rect.Height = canvas.Height / imageBitmap.Height;
                    rect.Width = rect.Height / 2;
                }
                else
                {
                    rect.Width = canvas.Height / imageBitmap.Width;// 矩形的宽
                    rect.Height = rect.Width * 2;// 矩形的高
                }
                
                srcColor = imageBitmap.GetPixel(j, i);// 取像素RGB

                // 两种颜色类型的转换
                rect.Fill = new SolidColorBrush(new Windows.UI.Color() {A=srcColor.A, R = srcColor.R, G = srcColor.G, B = srcColor.B });
                Canvas.SetLeft(rect, j * rect.Width);
                Canvas.SetTop(rect, i * rect.Height);
                canvas.Children.Add(rect);

            }
        }
    }

    public void showInRGB(ref Canvas canvas, int rate)
    {
    
    }

    public void showInGRAY(ref Canvas canvas, int rate)
    {
    
    }


    
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
            ImagePath = file.Path; // 暂存图片绝对路径
            
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
