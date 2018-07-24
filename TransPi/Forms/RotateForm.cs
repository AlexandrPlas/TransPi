using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public delegate void Rotate(double k1, double k2, double k3);
public delegate void Move(double k1, double k2, double k3);

namespace TransPi.Forms
{
    public partial class RotateForm : Form
    {
        private double rx = 0, ry = 0, rz = 0;
        private double mx = 0, my = 0, mz = 0;


        public event Rotate Rotate1;
        public event Move Move1;

        public RotateForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rx = Convert.ToDouble(RotateX.Text);
            ry = Convert.ToDouble(RotateY.Text);
            rz = Convert.ToDouble(RotateZ.Text);

            mx = Convert.ToDouble(MoveX.Text);
            my = Convert.ToDouble(MoveY.Text);
            mz = Convert.ToDouble(MoveZ.Text);

            if (rx != 0 || ry != 0 || rz != 0)
            {
                Rotate1(rx, ry, rz);
            }

            if (mx != 0 || my != 0 || mz != 0)
            {
                Move1(mx,my,mz);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
