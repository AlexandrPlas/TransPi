using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransPi;

namespace TransPi
{
    public partial class ParamDialogForm : Form
    {
        MainForm mainForm;
        public ParamDialogForm(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }

        private void OKButt_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != string.Empty)
            {
                string temp = this.textBox1.Text;
                string[] split = temp.Split(new Char[] { ' ', ',', '.' });
                mainForm.NumPic = new int[split.Length];
                if (split.Length == 1)
                    mainForm.Median = Convert.ToInt32(this.textBox1.Text);
                else
                
                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i] != "")
                        mainForm.NumPic[i] = Convert.ToInt32(split[i]);
                }
                
            }
            if (this.textBox2.Text != string.Empty)
            {
                string temp = this.textBox2.Text;
                string[] split = temp.Split(new Char[] { ' ', ',', '.' });
                mainForm.zscale = Convert.ToDouble(this.textBox2.Text);
                

            }
            if (this.richTextBox1.Text != string.Empty)
            {
                string temp = this.richTextBox1.Text;
                string[] split = temp.Split(new Char[] { ' ', ',', '.', ':', '\t', '\n' });
                mainForm.ShiftData = new int[split.Length];
                for (int i = 0; i < split.Length; i++)
                {
                   if (split[i] != "")
                        mainForm.ShiftData[i] = Convert.ToInt32(split[i]);
                }
            }
            mainForm.OKbutton = true;
            this.Close();

        }

        private void CancelButt_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
