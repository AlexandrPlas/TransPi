using ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransPi.Forms
{
    public partial class Signal : Form
    {
        public Signal()
        {
            InitializeComponent();
        }
        public Bitmap Sinbit = new Bitmap(800, 600);
        public PictureMass outPic = new PictureMass();
        public int Wave;
        public double Faze = 0;
        public int kvan, urovn = 200;
        public string swic;
        public double gamma = 1;
        public PictureMass ContrlPicMass= new PictureMass();

        private void Signal_Paint(object sender, PaintEventArgs e)
        {
            
            PictureMass fs = new PictureMass();
            switch (swic)
            {
                case "sine":
                    {
                        double wave = (double)360 / Wave;
                        Graphics g = e.Graphics;
                        g.Clear(Color.White);
                        double angle;
                        int Intence;

                        fs.SetSize(1600, 800);
                        for (int i = 0; i < fs.Width; i++)
                        {
                            angle = Math.PI * ((i % Wave) * wave + Faze) / 180.0;
                            angle = Math.Pow((Math.Sin(angle) + 1)/2, gamma);
                            Intence = (int)(angle * urovn);
                            for (int j = 0; j < fs.Height; j++)
                            {
                                fs.Array[i, j] = Intence;
                            }
                        }

                        Sinbit = new Bitmap(PictureMass.RgbToBitmapQ(fs.Array));
                        g.DrawImage(Sinbit, 0, 0);

                    }
                    break;
                case "dith":
                    {
                        ZArrayDescriptor img = new ZArrayDescriptor();
                        img = Dithering(Faze,  Wave, kvan, urovn, gamma);
                        Graphics g = e.Graphics;
                        g.Clear(Color.White);

                        Sinbit = new Bitmap(PictureMass.RgbToBitmapQ(img.array));
                        g.DrawImage(Sinbit, 0, 0);

                        fs.SetSize(img.width, img.height);
                        for (int i = 0; i < img.width; i++)
                        {
                            for (int j = 0; j < img.height; j++)
                            {
                                fs.Array[i, j] = img.array[i, j];
                            }
                        }
                    }
                    break;
                case "dithVZ":
                    {
                        ZArrayDescriptor img = new ZArrayDescriptor();
                        img = DitheringVZ(Faze,  Wave, kvan, urovn, gamma);
                        Graphics g = e.Graphics;
                        g.Clear(Color.White);

                        Sinbit = new Bitmap(PictureMass.RgbToBitmapQ(img.array));
                        g.DrawImage(Sinbit, 0, 0);

                        fs.SetSize(img.width, img.height);
                        for (int i = 0; i < img.width; i++)
                        {
                            for (int j = 0; j < img.height; j++)
                            {
                                fs.Array[i, j] = img.array[i, j];
                            }
                        }

                    }
                    break;
                default:
                    break;
            }

            outPic = fs;
            outPic.Pic = new Bitmap(PictureMass.RgbToBitmapQ(fs.Array));
        }

        private void Signal_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Signal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        #region dithering
        //----------------------------------------------------------------------------------------------------------Dithering

        public static ZArrayDescriptor Dithering(double fz, double n_polos, int N_kvant, int N_urovn, double gamma)
        {

            int NX = 1024;
            int NY = 1024;

            if (N_kvant < 2 && N_kvant > 256) { MessageBox.Show("Dithering  N_kvant"); return null; }


            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт
            double wave = (double)360 / n_polos;
            double kx = 2.0 * Math.PI / NX;

            //double.MinValue

            for (int i = 0; i < NX; i++)                                     // Приведение к числу уровней         
                for (int j = 0; j < NY; j++)
                {
                    double fz1 = Math.Pow((Math.Sin(Math.PI * ((i % n_polos) * wave + fz) / 180.0) + 1) / 2, gamma);

                    int ifz1 = (int)(fz1 * N_urovn) + 1;
                    if (ifz1 > N_urovn) ifz1 = N_urovn;                      // Чтобы избежать sin=1

                    cmpl.array[i, j] = ifz1 - 1;
                }



            for (int i = 0; i < NX; i++)                                    // Приведение к числу квантов
            {
                for (int j = 0; j < NY; j++)
                {

                    int ifz0 = (int)cmpl.array[i, j];
                    int ifz1 = Kvant(cmpl.array[i, j], N_kvant, N_urovn);

                    cmpl.array[i, j] = ifz1;
                    double err = ifz0 - ifz1;

                    int j1 = j + 1;
                    if (j1 < NY) { cmpl.array[i, j1] = cmpl.array[i, j1] + err * 7 / 16; }
                    int i1 = i + 1;
                    int j0 = j - 1;
                    if (i1 < NX)
                    {
                        if (j1 < NY) cmpl.array[i1, j1] = cmpl.array[i1, j1] + err * 3 / 16;
                        cmpl.array[i1, j] = cmpl.array[i1, j] + err * 5 / 16;
                        if (j0 >= 0) cmpl.array[i1, j0] = cmpl.array[i1, j0] + err * 1 / 16;
                    }
                }
                i++;
                for (int j = NY - 1; j >= 0; j--)
                {

                    int ifz0 = (int)cmpl.array[i, j];
                    int ifz1 = Kvant(cmpl.array[i, j], N_kvant, N_urovn);

                    cmpl.array[i, j] = ifz1;
                    double err = ifz0 - ifz1;

                    int j0 = j - 1;
                    if (j0 >= 0) { cmpl.array[i, j0] = cmpl.array[i, j0] + err * 7 / 16; }
                    int i1 = i + 1;
                    int j1 = j + 1;
                    if (i1 < NX)
                    {
                        if (j0 >= 0) cmpl.array[i1, j0] = cmpl.array[i1, j0] + err * 3 / 16;
                        cmpl.array[i1, j] = cmpl.array[i1, j] + err * 5 / 16;
                        if (j1 < NY) cmpl.array[i1, j1] = cmpl.array[i1, j1] + err * 1 / 16;
                    }
                }

            }

            return cmpl;
        }



        public static int Kvant(double a, int N_kvant, int N_urovn)
        {
            double fz1 = a / N_urovn;
            int ifz1 = (int)(fz1 * N_kvant) + 1;
            if (ifz1 > N_kvant) ifz1 = N_kvant;

            fz1 = (ifz1 - 1);
            ifz1 = (int)fz1 * N_urovn / (N_kvant - 1);
            return ifz1;
        }

        //-----------------------------------------------------------------------------------------------------------------------
        public static ZArrayDescriptor DitheringVZ(double fz, double n_polos, int N_kvant, int N_urovn,double gamma)  // Матрица возбуждения
        {

            int NX = 1024;
            int NY = 1024;
            double wave = (double)360 / n_polos;
            ZArrayDescriptor cmpl = new ZArrayDescriptor(NX, NY);      // Результирующий фронт
            double kx = 2.0 * Math.PI / NX;
            for (int i = 0; i < NX; i++)                                     // Приведение к числу уровней         
                for (int j = 0; j < NY; j++)
                {
                    double fz1 = Math.Pow((Math.Sin(Math.PI * ((i % n_polos) * wave + fz) / 180.0) + 1) / 2, gamma);

                    int ifz1 = (int)(fz1 * N_urovn) + 1;
                    if (ifz1 > N_urovn) ifz1 = N_urovn;                      // Чтобы избежать sin=1

                    cmpl.array[i, j] = ifz1 - 1;
                }

            int n = 8;// N_urovn / 32;
            double[,] D = new double[8, 8];
            D = Model_VZ(8);

            for (int x = 0; x < NX; x++)                                     // Приведение к числу уровней         
                for (int y = 0; y < NY; y++)
                {
                    int i = (x % n);
                    int j = (y % n);
                    if (cmpl.array[x, y] / 4 < D[i, j]) cmpl.array[x, y] = 0; else cmpl.array[x, y] = 255;
                }
            return cmpl;

        }
        public static double[,] Model_VZ(int n)   // Генерация матрицы возбуждения
        {
            double[,] array = new double[n, n];
            if (n == 2)
            {
                array[0, 0] = 0; array[0, 1] = 2;
                array[1, 0] = 3; array[1, 1] = 1;
                return array;
            }
            int n2 = n / 2;
            double[,] array1 = new double[n2, n2];
            array1 = Model_VZ(n2);
            for (int i = 0; i < n2; i++)
            {
                for (int j = 0; j < n2; j++) array[i, j] = 4 * array1[i, j];
                for (int j = n2; j < n; j++) array[i, j] = 4 * array1[i, j - n2] + 2;
            }
            for (int i = n2; i < n; i++)
            {
                for (int j = 0; j < n2; j++) array[i, j] = 4 * array1[i - n2, j] + 3;
                for (int j = n2; j < n; j++) array[i, j] = 4 * array1[i - n2, j - n2] + 1;
            }

            return array;
        }

        #endregion

    }
}
