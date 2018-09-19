using HTMLParser.Parser;
using HTMLParser.Parser.CSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTMLParser
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Run(new Form1());
        }
    }
}
