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
    public partial class GrowCutForm : Form
    {
        MainForm mainForm;
        Boolean Draw = false;
        int Width1, Height1, Size1 = 6;
        double[,] ArrayTF;
        Double ScalPic1 = 100, ScalPic2 = 100;
        float[,] ArrayStr;
        byte[, ,] ArrayRGB;
        byte[,] Mascer;
        Bitmap EndPic , mainPic;

        public GrowCutForm(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();

        }

        float g(int x, int MaxC)
        {
            float y;
            y = 1 - ((float)x / (float)MaxC);
            return y;
        }

        int MaxC(byte[,,] INArray)
        {
            int MaxC = 0;
            int width = INArray.GetLength(1),
                height = INArray.GetLength(2);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    int def = (int)(Math.Sqrt((INArray[0,x,y]*INArray[0,x,y]) +(INArray[1,x,y]*INArray[1,x,y])+(INArray[2,x,y]*INArray[2,x,y])));
                    if (MaxC < def)
                        MaxC = def;
                }
            return MaxC;
        }

        int Abs2C(byte[,,] ArrayRGB,int x, int y, int x2, int y2)
        {
            int Rez = 0, c0 = ArrayRGB[0, y, x] - ArrayRGB[0, y2, x2], c1 = ArrayRGB[1, y, x] - ArrayRGB[1, y2, x2], c2 = ArrayRGB[2, y, x] - ArrayRGB[2, y2, x2];
            Rez = (int)(Math.Sqrt((c0*c0)+(c1*c1)+(c2*c2)));
            return Rez;
        }

        private byte[,] GrowCut(double[,] ArrayTF, float[,] ArrayStr, byte[, ,] ArrayRGB, int rad)
        {
            byte[,] Boll = new byte[Width1, Height1];
            Boolean Zar = false;
            double[,] TFdef1 = new double[Width1, Height1], TFdef2 = ArrayTF;
            float[,] StrDef1 = new float[Width1, Height1], StrDef2 = ArrayStr;
            int MaxCf = MaxC(ArrayRGB), num = 0, num_2 = 0;
            while (num_2 < 50 /*&& num < 150 && Zar == false*/)
            {
                Zar = true;
                TFdef1 = TFdef2;
                StrDef1 = StrDef2;
                num++;
                for (int x = 0; x < Width1; x++)
                    Parallel.For (0,Height1, y=>
                    {
                        for (int x2 = x - rad; x2 <= x + rad; x2++)
                            for (int y2 = y - rad; y2 <= y + rad; y2++)
                            {
                                if (x2 < 0 || x2 >= Width1|| y2 < 0 || y2 >= Height1 || (TFdef2[x, y]==0 && TFdef1[x2, y2]==0)) continue;
                                int G = Abs2C(ArrayRGB,x,y,x2,y2);
                                if (g(G, MaxCf) * StrDef1[x2, y2] > StrDef1[x, y])
                                {
                                    TFdef2[x, y] = TFdef1[x2, y2];
                                    StrDef2[x, y] = g(G, MaxCf) * StrDef1[x2, y2];
                                }
                            }
                        if (TFdef2[x, y] == 0)   Zar = false; 
                    });
                if (Zar == true) num_2++;
            } 
            for (int x = 0; x < Width1; x++)
                for (int y = 0; y < Height1; y++)
                {
                    if (TFdef2[x, y] == 150) Boll[x, y] = 1;
                    if (TFdef2[x, y] == 250) Boll[x, y] = 0;
                }
            return Boll;
        }

        private byte[,,] Masker(byte [,,] RGB, byte[,] Masc)
        {
            byte[, ,] Rez = RGB;

            for (int x = 0; x < Width1; x++)
                for (int y = 0; y < Height1; y++)
                if (Masc[x,y]==0)
                {
                    Rez[0, y, x] = 0; Rez[1, y, x] = 0; Rez[1, y, x] = 0;
                }
                    
            return Rez;
        }

        private void GrowCutForm_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = MainForm.MainPic.Pic;
            mainPic = MainForm.MainPic.Pic;

            Width1 = MainForm.MainPic.Width;
            Height1 = MainForm.MainPic.Height;
            EndPic = new Bitmap(Width1, Height1);
            ArrayTF = new double[Width1, Height1];
            ArrayStr = new float[Width1, Height1];
            ArrayRGB = PictureMass.BitmapToByteRgbQ2(MainForm.MainPic.Pic);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

            Draw = false;
            EndPic = PictureMass.RgbToBitmapQ(ArrayTF);
            pictureBox2.Image = EndPic;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Draw == true)
            {
                int X = e.X;
                int Y = e.Y;
                int w1 = pictureBox1.Width, h1 = pictureBox1.Height;
                double width = (double)Width1 / w1,
                        height = (double)Height1 / h1;
                int Xr = (int)(e.X * width),
                        Yr = (int)(e.Y * height);
                Graphics graf = pictureBox1.CreateGraphics();
                for (int xi = Xr - (Size1 / 2); xi < Xr + (Size1 / 2); xi++)
                    for (int yi = Yr - (Size1 / 2); yi < Yr + (Size1 / 2); yi++)
                    {
                        if (xi < 0 || yi < 0 || xi >= Width1 || yi >= Height1) break;
                        if (Draw == true && (Math.Sqrt(((xi - Xr) * (xi - Xr)) + ((yi - Yr) * (yi - Yr))) < (Size1 / 2)))
                            if (radioButton1.Checked == true)
                            { ArrayTF[xi, yi] = 250; ArrayStr[xi, yi] = 1; }
                        if (radioButton2.Checked == true && Draw == true)
                        { ArrayTF[xi, yi] = 150; ArrayStr[xi, yi] = 1; }
                    }
                if (Draw == true)
                {
                    if (radioButton1.Checked == true)
                        graf.FillEllipse(Brushes.Blue, X - ((int)(Size1 / width) / 2), Y - ((int)(Size1 / height) / 2), (int)(Size1 / width), (int)(Size1 / height)); // толщина кисти
                    if (radioButton2.Checked == true)
                        graf.FillEllipse(Brushes.Red, X - ((int)(Size1 / width) / 2), Y - ((int)(Size1 / height) / 2), (int)(Size1 / width), (int)(Size1 / height)); // толщина кисти
                }
                mainPic = (Bitmap)pictureBox1.Image;
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Draw = true;
        }

        private void textBoxSize_TextChanged(object sender, EventArgs e)
        {
            Size1 = Convert.ToInt32(this.textBoxSize.Text);
        }

        private void buttonMasc_Click(object sender, EventArgs e)
        {
            Mascer = new byte[Width1,Height1];
            byte[,,] EndMass = new byte[3,Width1,Height1];
            Mascer = GrowCut(ArrayTF,ArrayStr,ArrayRGB, 2);
            EndMass = Masker(ArrayRGB, Mascer);
            EndPic = PictureMass.RgbToBitmapQ(ArrayTF);
            //Bitmap EndPic = PictureMass.RgbToBitmapQ2(EndMass);
            pictureBox2.Image = EndPic;
        }

        void PicScale1()
        {
            try
            {
                Image myBitmap = this.pictureBox1.Image;
                this.pictureBox1.Size = new Size(Width1, Height1);
                Size nSize = new Size((int)(Width1 * (ScalPic1 / 100)), (int)(Height1 * (ScalPic1 / 100)));
                Image gdi = new Bitmap(nSize.Width, nSize.Height);

                Graphics ZoomInGraphics = Graphics.FromImage(gdi);

                ZoomInGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                ZoomInGraphics.DrawImage(mainPic, new Rectangle(new Point(0, 0), nSize), new Rectangle(new Point(0, 0), mainPic.Size), GraphicsUnit.Pixel);
                ZoomInGraphics.Dispose();//

                pictureBox1.Image = gdi;
                pictureBox1.Size = gdi.Size;
            }
            catch (System.ArgumentException) { ScalPic1 += 10; }
        }
        void PicScale2()
        {
            try
            {
                Image myBitmap = this.pictureBox2.Image;
                this.pictureBox2.Size = new Size(Width1, Height1);
                Size nSize = new Size((int)(Width1 * (ScalPic2 / 100)), (int)(Height1 * (ScalPic2 / 100)));
                Image gdi = new Bitmap(nSize.Width, nSize.Height);

                Graphics ZoomInGraphics = Graphics.FromImage(gdi);

                ZoomInGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                ZoomInGraphics.DrawImage(EndPic, new Rectangle(new Point(0, 0), nSize), new Rectangle(new Point(0, 0), EndPic.Size), GraphicsUnit.Pixel);
                ZoomInGraphics.Dispose();//

                pictureBox2.Image = gdi;
                pictureBox2.Size = gdi.Size;
            }
            catch (System.ArgumentException) { ScalPic2 += 10; }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ScalPic1 = trackBar1.Value;
            PicScale1();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            ScalPic2 = trackBar2.Value;
            PicScale2();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ScalPic1 = 100;
            trackBar1.Value = 100;
            PicScale1();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ScalPic2 = 100;
            trackBar2.Value = 100;
            PicScale2();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < Width1; x++)
                for (int y = 0; y < Height1;y++ )
                    MainForm.MainPic.Array[x, y] = (int)Mascer[x, y];
            MainForm.MainPic.Pic = EndPic;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
