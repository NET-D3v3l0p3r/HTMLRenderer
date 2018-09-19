using HTMLParser.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTMLParser
{
    public partial class Form1 : Form
    {
        Muktashif mkt;
        public Form1()
        {
            InitializeComponent();
            mkt = new Muktashif(this, this.Width, this.Height)
            {
                Location = new Point(0, 50)
            };
            Controls.Add(mkt);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mkt.Navigate(@"testattribute.html");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openF = new OpenFileDialog();
            openF.ShowDialog();
            if (string.IsNullOrEmpty(openF.FileName))
                return;

            mkt.Navigate(openF.FileName);
            textBox1.Text = openF.FileName;

            openF.Dispose();
        }
    }
}
