using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Image2ASCIIEditor.Models;
public class ExportModel
{
    private int op;
    private string outputForLinux;
    private string filePath = null;
    private string ColorConvertForLinux(int id)
    {
        switch (id)
        {
            case 0:
                return "\\033[0;30m";
                break;
            case 12:
                return "\\033[1;31m";
                break;
            case 10:
                return "\\033[1;32m";
                break;
            case 14:
                return "\\033[1;33m";
                break;
            case 9:
                return "\\033[1;34m";
                break;
            case 13:
                return "\\033[1;35m";
                break;
            case 11:
                return "\\033[1;36m";
                break;
            case 15:
                return "\\033[1;37m";
                break;


            case 4:
                return "\\033[0;31m";
                break;
            case 2:
                return "\\033[0;32m";
                break;
            case 6:
                return "\\033[0;33m";
                break;
            case 1:
                return "\\033[0;34m";
                break;
            case 5:
                return "\\033[0;35m";
                break;
            case 3:
                return "\\033[0;36m";
                break;
            case 7:
                return "\\033[0;30m";
                break;

            default:
                return "";
        }
    }

    private bool isok = false;

    public ExportModel(int op)
    {
        this.op = op;
        if(op == 0)
        {

        }
        else if(op == 1)
        {
            outputForLinux = "#!/bin/bash\n";
            
            for(int i = 0; i < StringStreamModel.charsList.Count; i++)
            {
                outputForLinux += "echo -e \"";
                for (int j = 0; j < StringStreamModel.charsList[i].Count; j++)
                {
                    outputForLinux += ColorConvertForLinux(StringStreamModel.colorList[i][j]);
                    outputForLinux += StringStreamModel.charsList[i][j].ToString();
                    outputForLinux += "\\033[0m";
                }
                outputForLinux += "\"\n";
            }
            

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) => {
                //Some work...
                SaveTXTFile("bash Script", ".sh");
                while (isok == false) { Thread.Sleep(100); };
            };
            worker.RunWorkerCompleted += (s, e) => {
                //e.Result"returned" from thread

                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(outputForLinux);
                //【3】释放资源
                sw.Close();
                fs.Close();

            };
            worker.RunWorkerAsync();
        }
    }

    public async void SaveTXTFile(string a, string b)
    {
        var savePicker = new FileSavePicker();
        WinRT.Interop.InitializeWithWindow.Initialize(savePicker, MainWindow.hWnd);
        savePicker.SuggestedStartLocation =
            Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
        
        savePicker.FileTypeChoices.Add(a, new List<string>() { b });
        
        savePicker.SuggestedFileName = "字符图像"+DateTime.Now.ToString("t");


        // 打开文件选择对话框
        var file = await savePicker.PickSaveFileAsync();

        
        if (file != null)
        {
            filePath = file.Path; // 暂存绝对路径
            isok = true;
        }


    }
}
