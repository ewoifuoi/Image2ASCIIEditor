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
