using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtsUtilities.DpsLookUpParserWithProgress
{
    public partial class DpsLookUpParserWithProgressWindow : Form
    {
        private DpsLookUpParserWithProgressWindow()
        {
            //Default is not allowed
        }

        public DpsLookUpParserWithProgressWindow(String[] arguments)
        {
            String
                userName = arguments[0],
                passWord = arguments[1],
                searchBy = arguments[2],
                query = arguments[3];


        }
    }
}
