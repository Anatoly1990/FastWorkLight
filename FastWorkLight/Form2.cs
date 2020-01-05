using System;
using System.Windows.Forms;
using System.IO;

namespace FastWorkLight
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string s = Properties.Resources.Справка;            
            label1.Text = s;
        }
    }
}
