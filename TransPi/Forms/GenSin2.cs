using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransPi;
using TransPi;

public delegate void OneShotOfSeries(Image newImage, int imageNumber);

namespace TransPi.Forms
{
    public partial class GenSin2 : Form
    {
        public GenSin2(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }



        MainForm mainForm;
        private int shotNumbers = 4;
        private int imageNumber;
        private Bitmap Rezult;

        public PictureMass outPic = new PictureMass();
        Signal Signal = new Signal();

        public event OneShotOfSeries oneShotOfSeries;
        public event EventHandler GenToMain;

        private void button2_Click(object sender, EventArgs e)
        {
            Signal.Wave = Convert.ToInt32(textBox5.Text);
            Signal.Faze = Convert.ToInt32(textBox1.Text);
            Signal.urovn = Convert.ToInt32(textBox7.Text);
            Signal.gamma = Convert.ToDouble(textBox8.Text);
            Signal.swic = "sine";
            Signal.Owner = this;
            if (!Signal.Visible)
            {
                Screen[] sc;
                sc = Screen.AllScreens;
                if (sc.Length == 2)
                {
                    Signal.Left = sc[1].Bounds.Width;
                    Signal.Top = sc[1].Bounds.Height;
                    Signal.StartPosition = FormStartPosition.Manual;
                    Signal.Location = sc[1].Bounds.Location;
                    Point p = new Point(sc[1].Bounds.Location.X, sc[1].Bounds.Location.Y);
                    Signal.Location = p;
                    Signal.WindowState = FormWindowState.Maximized;
                }
                try
                {
                    Signal.Show();
                }
                catch (System.ObjectDisposedException) { MessageBox.Show("Не удалось запустить другую форму."); return; }
            }
            else
                Signal.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Signal.Wave = Convert.ToInt32(textBox5.Text);
            Signal.Faze = Convert.ToInt32(textBox1.Text);
            Signal.kvan = Convert.ToInt32(textBox6.Text);
            Signal.urovn = Convert.ToInt32(textBox7.Text);
            Signal.gamma = Convert.ToDouble(textBox8.Text);
            Signal.swic = "dith";
            Signal.Size = new Size(800, 600);
            Signal.Owner = this;

            if (!Signal.Visible)
            {
                Screen[] sc;
                sc = Screen.AllScreens;
                if (sc.Length == 2) { 
                    Signal.Left = sc[1].Bounds.Width;
                    Signal.Top = sc[1].Bounds.Height;
                    Signal.StartPosition = FormStartPosition.Manual;
                    Signal.Location = sc[1].Bounds.Location;
                    Point p = new Point(sc[1].Bounds.Location.X, sc[1].Bounds.Location.Y);
                    Signal.Location = p;
                    Signal.WindowState = FormWindowState.Maximized;
                }
                try
                {
                    Signal.Show();
                }
                catch (System.ObjectDisposedException) { MessageBox.Show("Не удалось запустить другую форму."); return; }
            }
            else
                Signal.Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Signal.Wave = Convert.ToInt32(textBox5.Text);
            Signal.Faze = Convert.ToInt32(textBox1.Text);
            Signal.kvan = Convert.ToInt32(textBox6.Text);
            Signal.urovn = Convert.ToInt32(textBox7.Text);
            Signal.gamma = Convert.ToDouble(textBox8.Text);
            Signal.swic = "dithVZ";
            Signal.Size = new Size(800, 600);
            Signal.Owner = this;

            if (!Signal.Visible)
            {
                Screen[] sc;
                sc = Screen.AllScreens;
                if (sc.Length == 2)
                {
                    Signal.Left = sc[1].Bounds.Width;
                    Signal.Top = sc[1].Bounds.Height;
                    Signal.StartPosition = FormStartPosition.Manual;
                    Signal.Location = sc[1].Bounds.Location;
                    Point p = new Point(sc[1].Bounds.Location.X, sc[1].Bounds.Location.Y);
                    Signal.Location = p;
                    Signal.WindowState = FormWindowState.Maximized;
                }
                try
                {
                    Signal.Show();
                }
                catch (System.ObjectDisposedException) { MessageBox.Show("Не удалось запустить другую форму."); return; }
            }
            else
                Signal.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MainForm.MainPic = Signal.outPic;
            if (Signal.outPic != null && GenToMain != null)
                GenToMain(this, EventArgs.Empty);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            imageNumber = 0;
            shotNumbers = 1;
            Signal.Wave = Convert.ToInt32(textBox5.Text);
            Signal.Faze = Convert.ToInt32(textBox1.Text);
            Signal.Size = new Size(800, 600);
            Signal.Owner = this;

            Signal.Refresh();

            ImageGetter.sharedInstance().imageReceived += imageTaken;
            ImageGetter.sharedInstance().getImage();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            imageNumber = 0;
            shotNumbers = 4;
            Signal.Wave = Convert.ToInt32(textBox5.Text);
            Signal.Faze = Convert.ToInt32(textBox1.Text);
            Signal.Size = new Size(800, 600);
            Signal.Owner = this;

            Signal.Refresh();

            ImageGetter.sharedInstance().imageReceived += imageTaken;
            ImageGetter.sharedInstance().getImage();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            imageNumber = 0;
            shotNumbers = 9;
            Signal.Wave = 167;
            Signal.Faze = Convert.ToInt32(textBox1.Text);
            Signal.Size = new Size(800, 600);
            Signal.Owner = this;

            Signal.Refresh();
            try { 
            ImageGetter.sharedInstance().imageReceived += imageTaken9;
            ImageGetter.sharedInstance().getImage();
           
            
            
            }
            catch { }

        }

        private void imageTaken(Image newImage)
        {
            imageNumber++;
            
            if (oneShotOfSeries != null)
            {
                oneShotOfSeries(newImage, imageNumber);
            }

            Rezult = new Bitmap(newImage);
            if (imageNumber >= shotNumbers)
            {
                ImageGetter.sharedInstance().imageReceived -= imageTaken;
                MainForm.ArrayPic[3] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[3]);
                mainForm.PicBox4.Image = Rezult;
                return;
            }

            if (imageNumber == 1)
                Signal.Faze = Convert.ToInt32(textBox2.Text);

            if (imageNumber == 2)
                Signal.Faze = Convert.ToInt32(textBox3.Text);
            if (imageNumber == 3)
                Signal.Faze = Convert.ToInt32(textBox4.Text);

            Signal.Refresh();
            Thread.Sleep(1000);
            if (imageNumber == 1)
            {
                MainForm.ArrayPic[0] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[0]);
                mainForm.PicBox1.Image = Rezult;
            }
            if (imageNumber == 2)
            {
                MainForm.ArrayPic[1] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[1]);
                mainForm.PicBox2.Image = Rezult;
            }
            if (imageNumber == 3)
            {
                MainForm.ArrayPic[2] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[2]);
                mainForm.PicBox3.Image = Rezult;
            }
            ImageGetter.sharedInstance().getImage();
        }

        private void imageTaken9(Image newImage)
        {
            imageNumber++;
            
            if (oneShotOfSeries != null)
            {
                oneShotOfSeries(newImage, imageNumber);
            }

            Rezult = new Bitmap(newImage);
            if (imageNumber >= shotNumbers)
            {
                ImageGetter.sharedInstance().imageReceived -= imageTaken;
                MainForm.ArrayPic[8] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[8]);
                mainForm.PicBox9.Image = Rezult;
                mainForm.OKbutton = true;
                return;
            }

            if (imageNumber == 1)
                Signal.Faze = Convert.ToInt32(textBox2.Text);
            if (imageNumber == 2)
                Signal.Faze = Convert.ToInt32(textBox3.Text);
            if (imageNumber == 3)
                Signal.Faze = Convert.ToInt32(textBox4.Text);
            if (imageNumber == 4) {
                Signal.Wave = 241;
                Signal.urovn +=3;
                Signal.gamma = Signal.gamma * 1.055;
                Signal.Faze = Convert.ToInt32(textBox1.Text);}
            if (imageNumber == 5)
                Signal.Faze = Convert.ToInt32(textBox2.Text);
            if (imageNumber == 6)
                Signal.Faze = Convert.ToInt32(textBox3.Text);
            if (imageNumber == 7)
                Signal.Faze = Convert.ToInt32(textBox4.Text);
            if (imageNumber == 8) { 
                Signal.Wave = 2000;
                Signal.Faze = 0;
            }

            Signal.Refresh();
            if (imageNumber == 1)
            {
                MainForm.ArrayPic[0] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[0]);
                mainForm.PicBox1.Image = Rezult;
            }
            if (imageNumber == 2)
            {
                MainForm.ArrayPic[1] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[1]);
                mainForm.PicBox2.Image = Rezult;
            }
            if (imageNumber == 3)
            {
                MainForm.ArrayPic[2] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[2]);
                mainForm.PicBox3.Image = Rezult;
            }
            if (imageNumber == 4)
            {
                MainForm.ArrayPic[3] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[3]);
                mainForm.PicBox4.Image = Rezult;
            }
            if (imageNumber == 5)
            {
                MainForm.ArrayPic[4] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[4]);
                mainForm.PicBox5.Image = Rezult;
            }
            if (imageNumber == 6)
            {
                MainForm.ArrayPic[5] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[5]);
                mainForm.PicBox6.Image = Rezult;
            }
            if (imageNumber == 7)
            {
                MainForm.ArrayPic[6] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[6]);
                mainForm.PicBox7.Image = Rezult;
            }
            if (imageNumber == 8)
            {
                MainForm.ArrayPic[7] = new PictureMass();
                PictureMass.SetBitmap(Rezult, MainForm.ArrayPic[7]);
                mainForm.PicBox8.Image = Rezult;
            }
            ImageGetter.sharedInstance().getImage();
            Thread.Sleep(1000);
        }



    }
}
