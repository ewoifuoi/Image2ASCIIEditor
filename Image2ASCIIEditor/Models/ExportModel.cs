using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image2ASCIIEditor.Models;
public class ExportModel
{
    private int op;
    private string outputForLinux;
    private string ColorConvertForLinux(int id)
    {
        switch (id)
        {
            case 0:
                return "\\033[1;30m";
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
    public ExportModel(int op)
    {
        this.op = op;
        if(op == 0)
        {

        }
        else if(op == 1)
        {
            for(int i = 0; i < StringStreamModel.charsList.Count; i++)
            {
                for(int j = 0; j < StringStreamModel.charsList[i].Count; j++)
                {


                }
            }
        }
    }
}
