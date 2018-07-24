using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ClassLibrary;

using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Imaging;
using TransPi.Forms;

namespace TransPi
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            instumentComboBox.SelectedIndex = 0;
        }

        FormGraph Graph = new FormGraph();
        Bitmap MainBitmap = new Bitmap(800, 600);
        double ScalPic = 100;
        public static PictureMass MainPic = new PictureMass();
        public static PictureMass[] ArrayPic = new PictureMass[12];
        private UndoRedo Undo = new UndoRedo();



        public struct Vect3di
        {
            public int v1, v2, v3;
        }

        public static int Var3D = 0;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public double[,] SetEdge(double[,] InArray, double gr)
        {
            int width = InArray.GetLength(0),
                height = InArray.GetLength(1);
            double[,] OutArray = new double[width, height];

            double Min = 1000000, Max = -1000000;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (Min > InArray[x, y]) Min = InArray[x, y];
                    if (Max < InArray[x, y]) Max = InArray[x, y];
                }
            }

            double _edge = (Math.Abs(Min) + Math.Abs(Max)) * gr;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x > 0 && y > 0)
                        if (Math.Abs(InArray[x - 1, y] - InArray[x, y]) < _edge && Math.Abs(InArray[x, y - 1] - InArray[x, y]) < _edge &&
                            Math.Abs(InArray[x, y] - InArray[x - 1, y]) < _edge && Math.Abs(InArray[x, y] - InArray[x, y - 1]) < _edge)
                            OutArray[x, y] = 0;
                        else
                            OutArray[x, y] = 10000;
                    else
                        if ((x == 0 && y > 0 && Math.Abs(InArray[x, y - 1] - InArray[x, y]) < _edge && Math.Abs(InArray[x, y] - InArray[x, y - 1]) < _edge) ||
                            (y == 0 && x > 0 && Math.Abs(InArray[x - 1, y] - InArray[x, y]) < _edge && Math.Abs(InArray[x, y] - InArray[x - 1, y]) < _edge))
                            OutArray[x, y] = 0;
                        else
                            OutArray[x, y] = 10000;
                }
            }
            return OutArray;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (PicRadio1.Checked == true)
            { PicBox1.Image = pictureBox1.Image; ArrayPic[0] = new PictureMass(); ArrayPic[0].CopyArray(MainPic); }
            if (PicRadio2.Checked == true)
            { PicBox2.Image = pictureBox1.Image; ArrayPic[1] = new PictureMass(); ArrayPic[1].CopyArray(MainPic); }
            if (PicRadio3.Checked == true)
            { PicBox3.Image = pictureBox1.Image; ArrayPic[2] = new PictureMass(); ArrayPic[2].CopyArray(MainPic); }
            if (PicRadio4.Checked == true)
            { PicBox4.Image = pictureBox1.Image; ArrayPic[3] = new PictureMass(); ArrayPic[3].CopyArray(MainPic); }
            if (PicRadio5.Checked == true)
            { PicBox5.Image = pictureBox1.Image; ArrayPic[4] = new PictureMass(); ArrayPic[4].CopyArray(MainPic); }
            if (PicRadio6.Checked == true)
            { PicBox6.Image = pictureBox1.Image; ArrayPic[5] = new PictureMass(); ArrayPic[5].CopyArray(MainPic); }
            if (PicRadio7.Checked == true)
            { PicBox7.Image = pictureBox1.Image; ArrayPic[6] = new PictureMass(); ArrayPic[6].CopyArray(MainPic); }
            if (PicRadio8.Checked == true)
            { PicBox8.Image = pictureBox1.Image; ArrayPic[7] = new PictureMass(); ArrayPic[7].CopyArray(MainPic); }
            if (PicRadio9.Checked == true)
            { PicBox9.Image = pictureBox1.Image; ArrayPic[8] = new PictureMass(); ArrayPic[8].CopyArray(MainPic); }
            if (PicRadio10.Checked == true)
            { PicBox10.Image = pictureBox1.Image; ArrayPic[9] = new PictureMass(); ArrayPic[9].CopyArray(MainPic); }
            if (PicRadio11.Checked == true)
            { PicBox11.Image = pictureBox1.Image; ArrayPic[10] = new PictureMass(); ArrayPic[10].CopyArray(MainPic); }
            if (PicRadio12.Checked == true)
            { PicBox12.Image = pictureBox1.Image; ArrayPic[11] = new PictureMass(); ArrayPic[11].CopyArray(MainPic); }

        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (PicRadio1.Checked == true)
            { MainPic.PMClear(); pictureBox1.Image = PicBox1.Image; MainPic.CopyArray(ArrayPic[0]); ScalPic = 100; }
            if (PicRadio2.Checked == true)
            { MainPic.PMClear(); pictureBox1.Image = PicBox2.Image; MainPic.CopyArray(ArrayPic[1]); ScalPic = 100; }
            if (PicRadio3.Checked == true)
            { MainPic.PMClear(); pictureBox1.Image = PicBox3.Image; MainPic.CopyArray(ArrayPic[2]); ScalPic = 100; }
            if (PicRadio4.Checked == true)
            { MainPic.PMClear(); pictureBox1.Image = PicBox4.Image; MainPic.CopyArray(ArrayPic[3]); ScalPic = 100; }
            if (PicRadio5.Checked == true)
            { MainPic.PMClear(); pictureBox1.Image = PicBox5.Image; MainPic.CopyArray(ArrayPic[4]); ScalPic = 100; }
            if (PicRadio6.Checked == true)
            { MainPic.PMClear(); pictureBox1.Image = PicBox6.Image; MainPic.CopyArray(ArrayPic[5]); ScalPic = 100; }
            if (PicRadio7.Checked == true)
            { MainPic.PMClear(); pictureBox1.Image = PicBox7.Image; MainPic.CopyArray(ArrayPic[6]); ScalPic = 100; }
            if (PicRadio8.Checked == true)
            { MainPic.PMClear(); pictureBox1.Image = PicBox8.Image; MainPic.CopyArray(ArrayPic[7]); ScalPic = 100; }
            if (PicRadio9.Checked == true)
            { MainPic.PMClear(); pictureBox1.Image = PicBox9.Image; MainPic.CopyArray(ArrayPic[8]); ScalPic = 100; }
            if (PicRadio10.Checked == true)
            { MainPic.PMClear(); pictureBox1.Image = PicBox10.Image; MainPic.CopyArray(ArrayPic[9]); ScalPic = 100; }
            if (PicRadio11.Checked == true)
            { MainPic.PMClear(); pictureBox1.Image = PicBox11.Image; MainPic.CopyArray(ArrayPic[10]); ScalPic = 100; }
            if (PicRadio12.Checked == true)
            { MainPic.PMClear(); pictureBox1.Image = PicBox12.Image; MainPic.CopyArray(ArrayPic[11]); ScalPic = 100; }
            Undo.ClearUR();
            UndoButton.Enabled = false;
            RedoButton.Enabled = false;
        }

        #region MouseClick

        int Xpr, Ypr, Xen, Yen;
        Point beg_point, end_point;
        Boolean Line_b = false, _mouse_down = false;
        int num_vek = 0;
        Point[] vek = new Point[3], vek2 = new Point[2];


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int X = e.X;
            int Y = e.Y;
            int w1 = pictureBox1.Width, h1 = pictureBox1.Height;
            double width = (double)MainPic.Width / w1,
                   height = (double)MainPic.Height / h1;
            int Xr = (int)(e.X * width),
                Yr = (int)(e.Y * height);

            if (instumentComboBox.SelectedItem.ToString() == "Вырезать")
            {
                Point Van = new Point(0, 0);
                Van.X = Xr; Van.Y = Yr;
                Xpr = Van.X; Ypr = Van.Y; _mouse_down = true;
                beg_point = new Point(Xpr, Ypr);
                pictureBox1.Paint -= pictureBox1_Paint;
                pictureBox1.Paint += Rec_Paint;
            }

            if (instumentComboBox.SelectedItem.ToString() == "Закрасить прямоугольник")
            {
                Point Van = new Point(0, 0);
                Van.X = Xr; Van.Y = Yr;
                Xpr = Van.X; Ypr = Van.Y; _mouse_down = true;
                beg_point = new Point(Xpr, Ypr);
                pictureBox1.Paint -= pictureBox1_Paint;
                pictureBox1.Paint += Rec_Paint;
            }

            if (instumentComboBox.SelectedItem.ToString() == "Кисть")
            {
                Point Van = new Point(0, 0);
                Van.X = Xr; Van.Y = Yr;
                Xpr = Van.X; Ypr = Van.Y; _mouse_down = true;
                beg_point = new Point(Xpr, Ypr);
                Size1 = Convert.ToInt32(SizeBox1.Text);
            }
        }

        Rectangle set_rect;
        List<Rectangle> rect = new List<Rectangle>();
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (instumentComboBox.SelectedItem.ToString() == "Кисть")
            {
                int X = e.X;
                int Y = e.Y;
                int w1 = pictureBox1.Width, h1 = pictureBox1.Height;
                double width = (double)MainPic.Width / w1,
                       height = (double)MainPic.Height / h1;
                Xen = (int)(e.X * width);
                Yen = (int)(e.Y * height);
                end_point = new Point(Xen, Yen);

                (sender as PictureBox).Refresh();

            }

            if (_mouse_down)
            {
                if ((instumentComboBox.SelectedItem.ToString() == "Вырезать") || instumentComboBox.SelectedItem.ToString() == ("Закрасить прямоугольник"))
                {
                    int X = e.X;
                    int Y = e.Y;
                    int w1 = pictureBox1.Width, h1 = pictureBox1.Height;
                    double width = (double)MainPic.Width / w1,
                           height = (double)MainPic.Height / h1;
                    Xen = (int)(e.X * width);
                    Yen = (int)(e.Y * height);
                    end_point = new Point(Xen, Yen);

                    set_rect = GetSelRectangle(beg_point, end_point);
                    //MainPic.Pic = PictureMass.RgbToBitmapQ(MainPic.Array);
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                        (sender as PictureBox).Refresh();
                    PicScale();
                }
            }



        }

        Rectangle GetSelRectangle(Point orig, Point location)
        {
            int deltaX = location.X - orig.X, deltaY = location.Y - orig.Y;
            Size s = new Size(Math.Abs(deltaX), Math.Abs(deltaY));
            Rectangle rect = new Rectangle();
            if (deltaX >= 0 & deltaY >= 0)
                rect = new Rectangle(orig, s);
            if (deltaX < 0 & deltaY > 0)
                rect = new Rectangle(location.X, orig.Y, s.Width, s.Height);
            if (deltaX < 0 & deltaY < 0)
                rect = new Rectangle(location, s);
            if (deltaX > 0 & deltaY < 0)
                rect = new Rectangle(orig.X, location.Y, s.Width, s.Height);
            return rect;
        }

        int Size1 = 4;

        private void ell_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawEllipse(new Pen(Color.Azure), end_point.X - Size1, end_point.Y - Size1, 2 * Size1, 2 * Size1);
            if (_mouse_down)
            {

                for (int xi = Xen - Size1; xi < Xen + Size1; xi++)
                    for (int yi = Yen - Size1; yi < Yen + Size1; yi++)
                    {
                        if (xi < 0 || yi < 0 || xi >= MainPic.Width || yi >= MainPic.Height) break;
                        if ((Math.Sqrt(((xi - Xen) * (xi - Xen)) + ((yi - Yen) * (yi - Yen))) < Size1))
                            if (!gal_Button1.Checked)
                            {
                                MainPic.Array[xi, yi] = Convert.ToDouble(indexText.Text);
                            }
                            else
                            {
                                MainPic.Array[xi, yi] = 10000;
                            }
                    }
                if (!gal_Button1.Checked)
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Blue), end_point.X - Size1, end_point.Y - Size1, 2 * Size1, 2 * Size1);
                    rect.Add(new Rectangle(end_point.X - Size1, end_point.Y - Size1, 2 * Size1, 2 * Size1));
                    for (int i = 0; i < rect.Count; i++)
                    {
                        e.Graphics.FillEllipse(new SolidBrush(Color.Blue), rect[i]);
                    }
                }
                else
                {
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), end_point.X - Size1, end_point.Y - Size1, 2 * Size1, 2 * Size1);
                    rect.Add(new Rectangle(end_point.X - Size1, end_point.Y - Size1, 2 * Size1, 2 * Size1));
                    for (int i = 0; i < rect.Count; i++)
                    {
                        e.Graphics.FillEllipse(new SolidBrush(Color.Red), rect[i]);
                    }
                }
            }
        }

        Pen pen = new Pen(Brushes.Blue, 0.8f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
        private void Rec_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(pen, set_rect);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(0, Color.Aqua)), set_rect);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            int X = e.X;
            int Y = e.Y;
            int w1 = pictureBox1.Width, h1 = pictureBox1.Height;
            double width = (double)MainPic.Width / w1,
                height = (double)MainPic.Height / h1;
            int Xr = (int)(e.X * width),
                    Yr = (int)(e.Y * height);
            end_point = new Point(Xr, Yr);

            if (instumentComboBox.SelectedItem.ToString() == "Графики")
            {
                 
                double[] Xar = new double[MainPic.Width];
                double[] Yar = new double[MainPic.Height];
                try
                {
                for (int i = 0; i < MainPic.Width; i++)
                    Xar[i] = MainPic.Array[i, Yr];
                for (int i = 0; i < MainPic.Height - 1; i++)
                    Yar[i] = MainPic.Array[Xr, i];

                Graph.Xar = Xar; Graph.Yar = Yar;
                Graph.Owner = this;
                Graph.Show();
                }
                catch (System.IndexOutOfRangeException)
                {  return; }
            }

            if (instumentComboBox.SelectedItem.ToString() == "Заполнение полос")
            {

                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                int NamberArr = Convert.ToInt32(indexText.Text);
                NamberArr += 10100;
                //LineFill(Xr, Yr, 1, Xr, Xr, 10000, NamberArr);
                //PixelFill(Xr, Yr, 10000, NamberArr);
                DrawClass.WhileFill(MainPic, Xr, Yr, 10000, NamberArr);
                MainPic.BuildBit();
                PicScale();
                //pictureBox1.Image = MainPic.Pic;
            }

            if (instumentComboBox.SelectedItem.ToString() == "Заполнение искажений")
            {

                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                DrawClass.WhileEdgeFill(MainPic, Xr, Yr, 100);

                MainPic.SetBitmap(PictureMass.BitmapMinMax(MainPic.Array));
                pictureBox1.Image = MainPic.Pic;

                // MainPic.BuildBit();
                // PicScale();
                //pictureBox1.Image = MainPic.Pic;
            }

            if (instumentComboBox.SelectedItem.ToString() == "Заполнить и стереть")
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                DrawClass.WhileFill(MainPic, Xr, Yr, 10000, 10000);
                MainPic.BuildBit();
                PicScale();
            }

            if ((instumentComboBox.SelectedItem.ToString() == "Востановление обрывов") && (gal_Button1.Checked == false))
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                Point Van;
                if (!Line_b)
                {
                    Van = DrawClass.Blijh(MainPic.Array, Xr, Yr, 10000, 300);
                    Xpr = Van.X; Ypr = Van.Y; Line_b = true; b_e_Button.Text = "E";
                }
                else
                {
                    Van = DrawClass.Blijh(MainPic.Array, Xr, Yr, 10000, 300);
                    //Bresenham8Line(MainPic.Array, 10100, Xpr, Ypr, Van.X, Van.Y);
                    DrawClass.LineSpr(MainPic.Array, Xpr, Ypr, Van.X, Van.Y, 20);
                    MainPic.BuildBit();
                    Xpr = Van.X; Ypr = Van.Y;
                    pictureBox1.Image = MainPic.Pic;
                    Line_b = false; b_e_Button.Text = "B";

                    PicScale();
                }
            }

            if ((instumentComboBox.SelectedItem.ToString() == "Востановление обрывов") && (gal_Button1.Checked == true))
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                Point Van;
                if (!Line_b)
                {
                    Van = DrawClass.Blijh(MainPic.Array, Xr, Yr, 10000, 300);
                    Xpr = Van.X; Ypr = Van.Y; Line_b = true; b_e_Button.Text = "E";
                }
                else
                {
                    Van = DrawClass.Blijh(MainPic.Array, Xr, Yr, 10000, 300);
                    DrawClass.Bresenham8Line(MainPic.Array, 10000, Xpr, Ypr, Van.X, Van.Y);
                    //LineSpr(MainPic.Array, Xpr, Ypr, Van.X, Van.Y, 20);
                    MainPic.BuildBit();
                    Xpr = Van.X; Ypr = Van.Y;
                    pictureBox1.Image = MainPic.Pic;
                    Line_b = false; b_e_Button.Text = "B";

                    PicScale();
                }
            }

            if (instumentComboBox.SelectedItem.ToString() == "Нарисовать линию")
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                Point Van = new Point(0, 0);
                if (!Line_b)
                {
                    Van.X = Xr; Van.Y = Yr;
                    Xpr = Van.X; Ypr = Van.Y; Line_b = true; b_e_Button.Text = "E";
                }
                else
                {

                    Van.X = Xr; Van.Y = Yr;
                    DrawClass.Bresenham8Line(MainPic.Array, 10000, Xpr, Ypr, Van.X, Van.Y);
                    MainPic.BuildBit();
                    Xpr = Van.X; Ypr = Van.Y;
                    pictureBox1.Image = MainPic.Pic;
                    Line_b = false; b_e_Button.Text = "B";

                    PicScale();
                }
            }
            if (instumentComboBox.SelectedItem.ToString() == "Вырезать")
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                Point Van = new Point(0, 0);
                if (_mouse_down)
                {

                    Van.X = Xr; Van.Y = Yr;
                    MainPic = PictureMass.cutRec(MainPic, Xpr, Ypr, Van.X, Van.Y);
                    //MainPic.Pic = PictureMass.RgbToBitmapQ(MainPic.Array);

                    MainPic.SetBitmap(PictureMass.BitmapMinMax(MainPic.Array));
                    if (gal_Button1.Checked)
                    {
                        ParamDialogForm subForm = new ParamDialogForm(this);
                        subForm.label1.Text = "Вырезать из окон:";
                        subForm.textBox2.Hide();
                        subForm.label2.Hide();
                        subForm.richTextBox1.Hide();
                        subForm.ShowDialog();
                        for (int i = 0; i < NumPic.Length; i++)
                        {
                            ArrayPic[NumPic[i] - 1] = PictureMass.cutRec(ArrayPic[NumPic[i] - 1], Xpr, Ypr, Van.X, Van.Y);
                            ArrayPic[NumPic[i] - 1].Pic = PictureMass.RgbToBitmapQ(ArrayPic[NumPic[i] - 1].Array);
                        }
                    }
                    pictureBox1.Paint += pictureBox1_Paint;
                    pictureBox1.Paint -= Rec_Paint;
                    _mouse_down = false;

                    PicScale();
                }
            }
            if (instumentComboBox.SelectedItem.ToString() == "Закрасить прямоугольник")
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                Point Van = new Point(0, 0);
                if (_mouse_down)
                {

                    end_point = new Point(Xr, Yr);
                    set_rect = GetSelRectangle(beg_point, end_point);
                    Point end_p = new Point(set_rect.Right + set_rect.X, set_rect.Bottom + set_rect.Y);
                    if (!gal_Button1.Checked)
                    {
                        MainPic = PictureMass.fillRec(MainPic, set_rect.Location, end_p, Convert.ToDouble(indexText.Text));

                        MainPic.SetBitmap(PictureMass.BitmapMinMax(MainPic.Array));
                        pictureBox1.Image = MainPic.Pic;
                    }
                    else
                    {
                        MainPic = PictureMass.fillRec(MainPic, set_rect.Location, end_p, 10000);
                        MainPic.BuildBit();
                    }


                    pictureBox1.Paint += pictureBox1_Paint;
                    pictureBox1.Paint -= Rec_Paint;
                    _mouse_down = false;

                    PicScale();
                }
            }

            if (instumentComboBox.SelectedItem.ToString() == "Кисть")
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                Point Van = new Point(0, 0);
                if (_mouse_down)
                {

                    end_point = new Point(Xr, Yr);
                    set_rect = GetSelRectangle(beg_point, end_point);
                    Point end_p = new Point(set_rect.Right + set_rect.X, set_rect.Bottom + set_rect.Y);

                    _mouse_down = false;
                    if (!gal_Button1.Checked)
                    {
                        MainPic.SetBitmap(PictureMass.BitmapMinMax(MainPic.Array));
                        pictureBox1.Image = MainPic.Pic;
                    }
                    else
                    {
                        MainPic.BuildBit();
                    }
                    rect.Clear();
                    PicScale();
                }
            }
            if (instumentComboBox.SelectedItem.ToString() == "Плоскость по 3-м точкам")
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                Point Van = new Point(0, 0);
                end_point = new Point(Xr, Yr);
                vek[num_vek] = end_point;
                num_vek++;
                if (num_vek == 3)
                {
                    num_vek = 0;
                    MainPic = NKL(MainPic, vek);
                    PicScale();
                }


            }

            if (instumentComboBox.SelectedItem.ToString() == "Фон по 2-м точкам")
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                Point Van = new Point(0, 0);
                end_point = new Point(Xr, Yr);
                vek2[num_vek] = end_point;
                num_vek++;
                if (num_vek == 2)
                {
                    num_vek = 0;
                    MainPic = NKL(MainPic, vek2);
                    PicScale();
                }

            }

        }

        private PictureMass NKL(PictureMass MainPic, Point[] inpoint)
        {
            int lenght_in = inpoint.Length;
            Point left = new Point(), right = new Point(), mid = new Point();
            if (lenght_in == 3)
                if (inpoint[0].X >= inpoint[1].X && inpoint[0].X >= inpoint[2].X)
                {
                    right = inpoint[0];
                    if (inpoint[1].X >= inpoint[2].X)
                    { left = inpoint[2]; mid = inpoint[1]; }
                    else { left = inpoint[1]; mid = inpoint[2]; }
                }
                else
                {
                    if (inpoint[1].X >= inpoint[0].X && inpoint[1].X >= inpoint[2].X)
                    {
                        right = inpoint[1];
                        if (inpoint[0].X >= inpoint[2].X)
                        { left = inpoint[2]; mid = inpoint[0]; }
                        else { left = inpoint[0]; mid = inpoint[2]; }
                    }
                    else
                    {
                        right = inpoint[2];
                        if (inpoint[0].X >= inpoint[1].X)
                        { left = inpoint[1]; mid = inpoint[0]; }
                        else { left = inpoint[0]; mid = inpoint[1]; }
                    }
                }

            if (lenght_in == 2)
                if (inpoint[0].X >= inpoint[1].X)
                {
                    right = inpoint[0];
                    left = inpoint[1];
                }
                else
                {
                    right = inpoint[1];
                    left = inpoint[0];
                }

            int hig = (right.Y + left.Y) / 2, wig = (right.X + left.X) / 2;
            if (lenght_in == 2)
            {
                right.Y = hig;
                left.Y = hig;
            }

            double x1 = left.X,
                    y1 = left.Y,
                    z1 = MainPic.Array[(int)x1, (int)y1],

                    x2 = mid.X,
                    y2 = mid.Y,
                    z2 = MainPic.Array[(int)x2, (int)y2],
                    z_mid = MainPic.Array[wig, hig],


                    x3 = right.X,
                    y3 = right.Y,
                    z3 = MainPic.Array[(int)x3, (int)y3],

                    z_mid_end = z_mid - ((z1 + z3) / 2),
            A, B, C, D;

            double[] parra = new double[MainPic.Width];

            double a, b, c;

            a = (-z_mid_end) / Math.Pow(right.X - wig, 2);
            b = -2 * a * wig;
            c = a * wig * wig + z_mid_end;

            for (int i = 0; i < MainPic.Width; i++)
            {
                parra[i] = a * Math.Pow(i - wig, 2) + z_mid_end;
                parra[i] += ((z3 - z1) * i - (z3 * left.X - z1 * right.X)) / (right.X - left.X);
            }




            /* A = y1*(z2 - z3) + y2*(z3 - z1) + y3*(z1 - z2); 
             B = z1*(x2 - x3) + z2*(x3 - x1) + z3*(x1 - x2); 
             C = x1*(y2 - y3) + x2*(y3 - y1) + x2*(y1 - y2);
             D =-( x1 * (y2 * z3 - y3 * z2) + x2 * (y3 * z1 - y1 * z3) + x3 * (y1 * z2 - y2 * z1));*/

            double ax = x2 - x1, ay = y2 - y1, az = z2 - z1;
            double bx = x3 - x1, by = y3 - y1, bz = z3 - z1;
            A = ay * bz - az * by;
            B = -(ax * bz - bx * az);
            C = ax * by - ay * bx;
            D = -(A * x1 + B * y1 + C * z1);
            A = A / C; B = B / C; D = D / C;


            double[,] outarr = new double[MainPic.Width, MainPic.Height];
            double Min = 1000000, Max = -1000000;
            for (int i = 0; i < MainPic.Width; i++)
            {
                for (int j = 0; j < MainPic.Height; j++)
                {
                    //outarr[i,j]=(A*i+B*j+D)/(-C);
                    if (lenght_in == 3) outarr[i, j] = -(A * i + B * j + D);
                    if (lenght_in == 2) outarr[i, j] = parra[i];
                    if (Min > outarr[i, j]) Min = outarr[i, j];
                    if (Max < outarr[i, j]) Max = outarr[i, j];
                }
            }

            parra = null;
            double[,] ArTimMin = new double[MainPic.Width, MainPic.Height];
            PictureMass end = new PictureMass();
            end.SetArray(outarr);
            for (int x = 0; x < MainPic.Width; x++)
                for (int y = 0; y < MainPic.Height; y++)
                {
                    ArTimMin[x, y] = ((int)(255 * (outarr[x, y] + 1 - Min) / (Max - Min)));
                }
            end.Pic = PictureMass.RgbToBitmapQ(ArTimMin);

            return end;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (instumentComboBox.SelectedItem.ToString() == "Кисть") Cursor.Hide();
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (instumentComboBox.SelectedItem.ToString() == "Кисть") Cursor.Show();
        }

        #endregion

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region File
        static double[,] ReadArray(string path)
        {
            double[,] arr;
            using (StreamReader sr = new StreamReader(path))
            {
                int n = int.Parse(sr.ReadLine()); //число строк
                int m = int.Parse(sr.ReadLine()); //число столбцов
                arr = new double[n, m];
                for (int i = 0; i < n; i++)
                {
                    //Считываем очередную строку из файла, в которой хранятся значения столбцов текущей строки
                    //Методом Split разбиваем ее по пробелам и заполняем массив.
                    string temp = sr.ReadLine();
                    string[] line = temp.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < m; j++)
                    {
                        arr[i, j] = int.Parse(line[j]);
                    }
                }
            }
            return arr;
        }



        static void WriteArray(string path, double[,] array)
        {
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.WriteLine(array.GetLength(0));
                sw.WriteLine(array.GetLength(1));
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    string[] line = new string[array.GetLength(1)];
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        //Cобираем в строковый массив столбцы текущей строки массива
                        line[j] = array[i, j].ToString();
                    }
                    //Метод Join() склеивает элементы массива line в одну строку, разделяя их пробелами
                    sw.WriteLine(String.Join(" ", line));
                }
            }
        }

        static void WriteArrayObj(string path, double[,] array, double razm)
        {
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                string[] line = new string[4];
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        //Cобираем в строковый массив столбцы текущей строки массива
                        line[0] = "v";
                        line[1] = (i / razm).ToString();
                        line[2] = (j / razm).ToString();
                        line[3] = (array[i, j] / razm).ToString();
                        //Метод Join() склеивает элементы массива line в одну строку, разделяя их пробелами
                        sw.WriteLine(String.Join(" ", line));
                    }
                }

                sw.WriteLine("");

                for (int i = 1; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1) - 1; j++)
                    {
                        line[0] = "f";
                        line[1] = (j + 1 + ((i - 1) * array.GetLength(1))).ToString();
                        line[2] = (i * array.GetLength(1) + j + 1).ToString();
                        line[3] = (j + 2 + ((i - 1) * array.GetLength(1))).ToString();
                        sw.WriteLine(String.Join(" ", line));
                        if ((i * array.GetLength(1) + j + 2) <= (array.GetLength(0) * array.GetLength(1)))
                        {
                            line[0] = "f";
                            line[1] = (i * array.GetLength(1) + j + 2).ToString();
                            line[2] = (j + 2 + ((i - 1) * array.GetLength(1))).ToString();
                            line[3] = (i * array.GetLength(1) + j + 1).ToString();
                            sw.WriteLine(String.Join(" ", line));
                        }
                    }
                }
            }
        }

        private void загрузитьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|bmp files (*.bmp)|*.bmp|array files (*.zarr)|*.zarr|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 4;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            ScalPic = 100;
                            FileInfo fileInfo = new FileInfo(openFileDialog1.FileName);
                            if (fileInfo.Extension.ToLower() == ".txt")
                            {
                                MainPic.SetArray(ReadArray(openFileDialog1.FileName));
                                MainPic.BuildBit();
                                pictureBox1.Image = MainPic.Pic;
                            }
                            else
                            {
                                if (fileInfo.Extension.ToLower() == ".zarr")
                                {
                                    BinaryFormatter deserializer = new BinaryFormatter();
                                    ZArrayDescriptor savedArray = (ZArrayDescriptor)deserializer.Deserialize(myStream);
                                    PictureMass fs = new PictureMass();
                                    fs.SetSize(savedArray.width, savedArray.height);
                                    for (int i = 0; i < savedArray.width; i++)
                                    {
                                        for (int j = 0; j < savedArray.height; j++)
                                        {
                                            fs.Array[i, j] = savedArray.array[i, j];
                                        }
                                    }
                                    MainPic = fs;
                                    MainPic.SetBitmap(PictureMass.BitmapMinMax(MainPic.Array));
                                    pictureBox1.Image = MainPic.Pic;
                                    myStream.Close();
                                }
                                else
                                {
                                    // Insert code to read the stream here.
                                    Bitmap bmp = new Bitmap(openFileDialog1.FileName);
                                    MainPic.SetBitmap(bmp);
                                    MainPic.SetArray(PictureMass.BitmapToByteRgbQ(bmp));
                                    pictureBox1.Image = MainPic.Pic;

                                }
                            }
                            Undo.ClearUR();
                            RedoButton.Enabled = false;
                            UndoButton.Enabled = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Загрузка не удалась. " + ex.Message);
                }
            }
        }
        private void сохранитьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|bmp files (*.bmp)|*.bmp|array files (*.zarr)|*.zarr|3D obj files (*.obj)|*.obj";
            saveFileDialog1.FilterIndex = 3;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        FileInfo fileInfo = new FileInfo(saveFileDialog1.FileName);
                        if (fileInfo.Extension.ToLower() == ".txt")
                        {
                            myStream.Dispose();
                            WriteArray(saveFileDialog1.FileName, MainPic.Array);
                        }

                        else
                        {
                            if (fileInfo.Extension.ToLower() == ".zarr")
                            {
                                BinaryFormatter serializer = new BinaryFormatter();
                                ZArrayDescriptor arrayDescriptor = new ZArrayDescriptor(MainPic.Width, MainPic.Height);
                                for (int i = 0; i < MainPic.Width; i++)
                                {
                                    for (int j = 0; j < MainPic.Height; j++)
                                    {
                                        arrayDescriptor.array[i, j] = (double)MainPic.Array[i, j];
                                    }
                                }


                                serializer.Serialize(myStream, arrayDescriptor);
                                myStream.Close();
                            }
                            else if (fileInfo.Extension.ToLower() == ".obj")
                            {
                                myStream.Dispose();
                                ParamDialogForm subForm = new ParamDialogForm(this);
                                subForm.label1.Text = "Текстура:";
                                subForm.label2.Hide();
                                subForm.textBox2.Hide();
                                subForm.richTextBox1.Hide();
                                subForm.ShowDialog();
                                if (OKbutton == true)
                                {
                                    Bitmap texture = ArrayPic[Median - 1].Pic;
                                    if (!textb)
                                    {
                                        texture.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                                        textb = true;
                                    }
                                    ZArrayDescriptor zArrayDescriptor = new ZArrayDescriptor(MainPic.Width, MainPic.Height);
                                    for (int i = 1; i < MainPic.Width; i++)
                                    {
                                        for (int j = 1; j < MainPic.Height; j++)
                                        {
                                            zArrayDescriptor.array[MainPic.Width - i, MainPic.Height - j] = MainPic.Array[i, j];
                                        }
                                    }

                                    if (zArrayDescriptor != null && texture != null)
                                    {
                                        Visualisation.Mesh.ZArrayToObject(zArrayDescriptor, TransPi.Visualisation.Mesh.ColoringMethod.Texture, saveFileDialog1.FileName, texture);
                                    }
                                }

                                //WriteArrayObj(saveFileDialog1.FileName, MainPic.Array,1);

                            }
                            else
                            {
                                // Code to write the stream goes here.
                                Bitmap bad = MainPic.Pic;
                                //Bitmap bad = (Bitmap)pictureBox1.Image;
                                bad.Save(myStream, ImageFormat.Bmp);
                                myStream.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сохранение не удалось. " + ex.Message);
                }
            }
        }


        private void сохранитьГруппуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (saveFileDialog1.FileName != null)
                    {
                        String FileName = saveFileDialog1.FileName;
                        
                        for (int ir = 0; ir < 12; ir++)
                        {
                        String path = "";
                        switch(ir)
                        {
                            case 0:
                                { path = FileName + "_167_0.bmp"; break; }
                            case 1:
                                { path = FileName + "_167_90.bmp"; break; }
                            case 2:
                                { path = FileName + "_167_180.bmp"; break; }
                            case 3:
                                { path = FileName + "_167_270.bmp"; break; }
                            case 4:
                                { path = FileName + "_241_0.bmp"; break; }
                            case 5:
                                { path = FileName + "_241_90.bmp"; break; }
                            case 6:
                                { path = FileName + "_241_180.bmp"; break; }
                            case 7:
                                { path = FileName + "_241_270.bmp"; break; }
                            case 8:
                                { path = FileName + "_texture.bmp"; break; }
                            case 9:
                                { path = FileName + "_167_faze.zarr";  break; }
                            case 10:
                                { path = FileName + "_241_faze.zarr"; break; }
                            case 11:
                                { path = FileName + "_rebuilt.zarr"; break; }
                            default:
                                    break;
                        }
                            saveFileDialog1.FileName = path;
                            FileInfo fileInfo = new FileInfo(saveFileDialog1.FileName);
                            myStream = saveFileDialog1.OpenFile();
                            if (fileInfo.Extension.ToLower() == ".zarr")
                            {
                                BinaryFormatter serializer = new BinaryFormatter();
                                ZArrayDescriptor arrayDescriptor = new ZArrayDescriptor(ArrayPic[ir].Width, ArrayPic[ir].Height);
                                for (int i = 0; i < ArrayPic[ir].Width; i++)
                                {
                                    for (int j = 0; j < ArrayPic[ir].Height; j++)
                                    {
                                        arrayDescriptor.array[i, j] = (double)ArrayPic[ir].Array[i, j];
                                    }
                                }


                                serializer.Serialize(myStream, arrayDescriptor);
                            }
                            else
                            {
                                Bitmap bad = ArrayPic[ir].Pic;
                                bad.Save(myStream, ImageFormat.Bmp);
                                
                            }
                            myStream.Close();
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сохранение не удалось. " + ex.Message);
                }
            }
        }

        #endregion

        public int Median;

        #region Filter
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            double[,] ArTim = new double[MainPic.Width, MainPic.Height];
            double[,] ArTim2 = new double[MainPic.Width, MainPic.Height];
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Размерность окна:";
            subForm.label2.Hide();
            subForm.textBox2.Hide();
            subForm.richTextBox1.Hide();
            subForm.ShowDialog();
            double Min = 1000000, Max = -1000000;
            if (OKbutton == true && Median > 0)
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                ArTim = Filtration.Filt_Mediana(MainPic.Array, Median);
                MainPic.SetArray(ArTim);
                //MainPic.BuildBit();
                for (int x = 0; x < MainPic.Width; x++)
                    for (int y = 0; y < MainPic.Height; y++)
                    {
                        if (Max < MainPic.Array[x, y]) Max = MainPic.Array[x, y];
                        if (Min > MainPic.Array[x, y]) Min = MainPic.Array[x, y];
                    }
                for (int x = 0; x < MainPic.Width; x++)
                    for (int y = 0; y < MainPic.Height; y++)
                    {
                        ArTim2[x, y] = ((int)(255 * (ArTim[x, y] - Min) / (Max - Min)));
                    }
                MainPic.Pic = PictureMass.RgbToBitmapQ(ArTim2);
                PicScale();
                //pictureBox1.Image = MainPic.Pic;
            }
        }

        private void порогиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] ArTim = new double[MainPic.Width, MainPic.Height];
            double[,] ArTim2 = new double[MainPic.Width, MainPic.Height];
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Порог:";
            subForm.label2.Hide();
            subForm.textBox2.Hide();
            subForm.richTextBox1.Hide();
            subForm.ShowDialog();
            double Min = 1000000, Max = -1000000;
            if (OKbutton == true && Median > 0)
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                ArTim = Filtration.Porogi(MainPic.Array, Median);
                MainPic.SetArray(ArTim);
                //MainPic.BuildBit();
                for (int x = 0; x < MainPic.Width; x++)
                    for (int y = 0; y < MainPic.Height; y++)
                    {
                        if (Max < MainPic.Array[x, y]) Max = MainPic.Array[x, y];
                        if (Min > MainPic.Array[x, y]) Min = MainPic.Array[x, y];
                    }
                for (int x = 0; x < MainPic.Width; x++)
                    for (int y = 0; y < MainPic.Height; y++)
                    {
                        ArTim2[x, y] = ((int)(255 * (ArTim[x, y] - Min) / (Max - Min)));
                    }
                MainPic.Pic = PictureMass.RgbToBitmapQ(ArTim2);
                PicScale();
                //pictureBox1.Image = MainPic.Pic;
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            double[,] ArTim = new double[MainPic.Width, MainPic.Height];
            double[,] ArTim2 = new double[MainPic.Width, MainPic.Height];
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Размерность окна:";
            subForm.label2.Hide();
            subForm.textBox2.Hide();
            subForm.richTextBox1.Hide();
            subForm.ShowDialog();
            double Min = 1000000, Max = -1000000;
            if (OKbutton == true && Median > 0)
            {
                ArTim = Filtration.Filt_smothingSM(MainPic.Array, Median);
                MainPic.SetArray(ArTim);
                //MainPic.BuildBit();
                for (int x = 0; x < MainPic.Width; x++)
                    for (int y = 0; y < MainPic.Height; y++)
                    {
                        if (Max < MainPic.Array[x, y]) Max = MainPic.Array[x, y];
                        if (Min > MainPic.Array[x, y]) Min = MainPic.Array[x, y];
                    }
                for (int x = 0; x < MainPic.Width; x++)
                    for (int y = 0; y < MainPic.Height; y++)
                    {
                        ArTim2[x, y] = ((int)(255 * (ArTim[x, y] - Min) / (Max - Min)));
                    }
                MainPic.Pic = PictureMass.RgbToBitmapQ(ArTim2);
                PicScale();
                //pictureBox1.Image = MainPic.Pic;
            }
        }
        #endregion

        public int[] NumPic, ShiftData;
        public Boolean OKbutton;

        //----------------------------------------------------------------------------------------------------------------
        void PicScale()
        {
            try
            {
                Image myBitmap = MainPic.Pic;
                this.pictureBox1.Size = new Size(MainPic.Pic.Width, MainPic.Pic.Height);
                Size nSize = new Size((int)(MainPic.Pic.Width * (ScalPic / 100)), (int)(MainPic.Pic.Height * (ScalPic / 100)));
                Image gdi = new Bitmap(nSize.Width, nSize.Height);

                Graphics ZoomInGraphics = Graphics.FromImage(gdi);

                ZoomInGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                ZoomInGraphics.DrawImage(MainPic.Pic, new Rectangle(new Point(0, 0), nSize), new Rectangle(new Point(0, 0), MainPic.Pic.Size), GraphicsUnit.Pixel);
                ZoomInGraphics.Dispose();//

                pictureBox1.Image = gdi;
                pictureBox1.Size = gdi.Size;
            }
            catch (System.ArgumentException) { ScalPic += 10; }
        }
        private void buttonScale1_Click(object sender, EventArgs e)
        {

            ScalPic -= 10;
            PicScale();
        }
        private void buttonScale2_Click(object sender, EventArgs e)
        {
            ScalPic += 10;
            PicScale();
        }
        private void buttonScale3_Click(object sender, EventArgs e)
        {
            ScalPic = 100;
            PicScale();
        }

        //----------------------------------------------------------------------------------------------------------------
        #region Обработка
        private void growCutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureMass tmp = new PictureMass(MainPic);
            Undo.NextDo(tmp);
            UndoButton.Enabled = true;
            GrowCutForm subForm = new GrowCutForm(this);
            subForm.ShowDialog();

            pictureBox1.Image = MainPic.Pic;
        }

        private void установитьГраницыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureMass tmp = new PictureMass(MainPic);
            Undo.NextDo(tmp);
            UndoButton.Enabled = true;
            double[,] Fal = SetEdge(MainPic.Array, 0.2);
            // Fal=RecLine(Fal, 10000, 20);
            ScalPic = 100;
            MainPic.SetArray(Fal);
            MainPic.BuildBit();
            pictureBox1.Image = MainPic.Pic;
        }

        private void удалениеИзображениемToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Изображение и фон:";
            subForm.label2.Hide();
            subForm.textBox2.Hide();
            subForm.richTextBox1.Hide();
            subForm.ShowDialog();
            if (OKbutton == true && NumPic.Length == 2 && NumPic[0] > 0 && NumPic[1] > 0)
            {
                MainPic = PictureMass.DellBack(ArrayPic[NumPic[0] - 1], ArrayPic[NumPic[1] - 1]); PicScale();
            }

        }

        private void удалениеПомехToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Нижняя и верх. гран.:";
            subForm.textBox1.Text = "0 255";
            subForm.label2.Hide();
            subForm.textBox2.Hide();
            subForm.richTextBox1.Hide();
            subForm.ShowDialog();
            if (OKbutton == true && NumPic.Length == 2)
            {
                MainPic = PictureMass.DelMinMax(MainPic, NumPic[0], NumPic[1]); 

                MainPic.SetBitmap(PictureMass.BitmapMinMax(MainPic.Array));
                pictureBox1.Image = MainPic.Pic;
                PicScale();
            }
        }

        private void boolМаскаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Изображение и маска:";
            subForm.label2.Text = "Цвет заполнения:";
            OKbutton = false;
            subForm.textBox2.Hide();
            subForm.ShowDialog();
            if (OKbutton == true)
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                OKbutton = false;
                try
                {
                    int width = Math.Max(ArrayPic[NumPic[0] - 1].Width, ArrayPic[NumPic[1] - 1].Width),
                        height = Math.Max(ArrayPic[NumPic[0] - 1].Height, ArrayPic[NumPic[1] - 1].Height);
                    Bitmap Main1 = new Bitmap(width, height);
                    double min = 123234546;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (ArrayPic[NumPic[1] - 1].Array[x, y] == 1 || ArrayPic[NumPic[1] - 1].Array[x, y] == 255)
                                if (min > ArrayPic[NumPic[0] - 1].Array[x, y]) { min = ArrayPic[NumPic[0] - 1].Array[x, y]; }
                        }
                    }
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (ArrayPic[NumPic[1] - 1].Array[x, y] == 1 || ArrayPic[NumPic[1] - 1].Array[x, y] == 255)
                                MainPic.Array[x, y] = ArrayPic[NumPic[0] - 1].Array[x, y] - 1300 + 1;
                            else
                                MainPic.Array[x, y] = ShiftData[0];
                        }
                    }

                    ScalPic = 100;
                    MainPic.BuildBit();
                    pictureBox1.Image = MainPic.Pic;
                }
                catch (System.NullReferenceException)
                { MessageBox.Show("Исходные изображения пусты."); return; }
                catch (System.IndexOutOfRangeException)
                { MessageBox.Show("Не верно заданны номера изображений."); return; }
            }
        }

        private void маскаГраницToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Изображение и маска:";
            subForm.label2.Text = "Цвет заполнения:";
            subForm.textBox2.Hide();
            OKbutton = false;
            subForm.ShowDialog();
            if (OKbutton == true)
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                OKbutton = false;
                try
                {
                    int width = Math.Max(ArrayPic[NumPic[0] - 1].Width, ArrayPic[NumPic[1] - 1].Width),
                        height = Math.Max(ArrayPic[NumPic[0] - 1].Height, ArrayPic[NumPic[1] - 1].Height);
                    Bitmap Main1 = new Bitmap(width, height);
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (ArrayPic[NumPic[1] - 1].Array[x, y] != 10000)
                                MainPic.Array[x, y] = ArrayPic[NumPic[0] - 1].Array[x, y];
                            else
                                MainPic.Array[x, y] = ShiftData[0];
                        }
                    }

                    MainPic.Pic = PictureMass.RgbToBitmapQ(MainPic.Array);

                    ScalPic = 100;
                    pictureBox1.Image = MainPic.Pic;
                }
                catch (System.NullReferenceException)
                { MessageBox.Show("Исходные изображения пусты."); return; }
                catch (System.IndexOutOfRangeException)
                { MessageBox.Show("Не верно заданны номера изображений."); return; }
            }
        }

        private void minMaxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Min и Max:";
            subForm.label2.Hide();
            subForm.textBox2.Hide();
            subForm.richTextBox1.Hide();
            OKbutton = false;
            subForm.ShowDialog();
            if (OKbutton == true)
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                OKbutton = false;
                try
                {
                    MainPic = PictureMass.ScallePic(MainPic, NumPic[0] - 1, NumPic[1] - 1);

                    ScalPic = 100;
                    MainPic.BuildBit();
                    pictureBox1.Image = MainPic.Pic;
                }
                catch (System.NullReferenceException)
                { MessageBox.Show("Исходные изображения пусты."); return; }
            }
        }

        private void scaleСМаскойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.textBox2.Hide();
            subForm.label1.Text = "Маска:";
            subForm.label2.Text = "Min, Max и Scale:";
            subForm.richTextBox1.Text = "0 255 1";
            OKbutton = false;
            subForm.ShowDialog();
            if (OKbutton == true)
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                OKbutton = false;
                try
                {
                    MainPic = PictureMass.ScallePicWithMask(MainPic, ArrayPic[Median - 1], ShiftData[0], ShiftData[1], ShiftData[2]);

                    ScalPic = 100;
                    MainPic.SetBitmap(PictureMass.BitmapMinMax(MainPic.Array));
                    pictureBox1.Image = MainPic.Pic;
                }
                catch (System.NullReferenceException)
                { MessageBox.Show("Исходные изображения пусты."); return; }
            }
        }

        public double zscale;
        private void scaleZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Z * ";
            subForm.textBox1.Hide();
            subForm.label2.Hide();
            subForm.richTextBox1.Hide();
            OKbutton = false;
            subForm.ShowDialog();
            if (OKbutton == true)
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                OKbutton = false;
                try
                {
                    MainPic = PictureMass.ScalleZ(MainPic, zscale);

                    ScalPic = 100;
                    MainPic.SetBitmap(PictureMass.BitmapMinMax(MainPic.Array));
                    pictureBox1.Image = MainPic.Pic;
                }
                catch (System.NullReferenceException)
                { MessageBox.Show("Исходные изображения пусты."); return; }
            }
        }

        private void изображенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Номера изображений:";
            subForm.textBox1.Text = "1 2 3 4";
            subForm.textBox2.Hide();
            subForm.label2.Text = "Сдвиги фаз";
            subForm.richTextBox1.Text = "270 180 90 0 256";
            OKbutton = false;
            subForm.ShowDialog();
            if (OKbutton == true)
            {
                OKbutton = false;
                try
                {
                    //---ublic static int[,] TranscriptSet(int[] numberAr, PictureMass[] InPic, int[] Shift)
                    int width = ArrayPic[NumPic[0]].Array.GetLength(0),
                    height = ArrayPic[NumPic[0]].Array.GetLength(1),
                    Len = NumPic.Length;
                    double res;
                    PictureMass[] temp = new PictureMass[Len];
                    for (int i = 0; i < Len; i++)
                        temp[i] = ArrayPic[NumPic[i] - 1];
                    double[,] res2 = new double[width, height];
                    List<Point> cnt = new List<Point>();
                    Point tmp = new Point();
                    double sin, cos;

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            sin = 0; cos = 0;
                            for (int i = 0; i < Len; i++)
                            {
                                if (i == 0)
                                {
                                    sin += (temp[i + 1].Array[x, y] - temp[Len - 1].Array[x, y]) * Math.Sin(ShiftData[i] * Math.PI / 180);
                                    cos += (temp[Len - 1].Array[x, y] - temp[i + 1].Array[x, y]) * Math.Cos(ShiftData[i] * Math.PI / 180);
                                }
                                if (i == Len - 1)
                                {
                                    sin += (temp[0].Array[x, y] - temp[i - 1].Array[x, y]) * Math.Sin(ShiftData[i] * Math.PI / 180);
                                    cos += (temp[i - 1].Array[x, y] - temp[0].Array[x, y]) * Math.Cos(ShiftData[i] * Math.PI / 180);
                                }
                                if (i < Len - 1 && i > 0)
                                {
                                    sin += (temp[i + 1].Array[x, y] - temp[i - 1].Array[x, y]) * Math.Sin(ShiftData[i] * Math.PI / 180);
                                    cos += (temp[i - 1].Array[x, y] - temp[i + 1].Array[x, y]) * Math.Cos(ShiftData[i] * Math.PI / 180);
                                }
                            }
                            if ((y > (height / 2) - 60) && (y < (height / 2) - 57))
                            {
                                tmp.X = (int)sin;
                                tmp.Y = (int)cos;
                                cnt.Add(tmp);
                            }
                            if (sin == 0 && cos == 0) res = 0;
                            else res = (Math.Atan(sin / cos) + Math.PI / 2) * (ShiftData[Len] - 1) / Math.PI;


                            res2[x, y] = res;
                        }
                    }
                    int mincntx = cnt.Min(p => p.X);
                    int mincnty = cnt.Min(p => p.Y);
                    int maxcntx = cnt.Max(p => p.X);
                    int maxcnty = cnt.Max(p => p.Y);


                    //----
                    MainPic.SetArray(res2);
                    MainPic.SetBitmap(PictureMass.BitmapMinMax(MainPic.Array));
                    /*Bitmap Main1 = new Bitmap(width, height);
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = width - 1; x > 0; x--)
                        {
                            Main1.SetPixel(x, y, Color.FromArgb((int)res2[x, y], (int)res2[x, y], (int)res2[x, y]));
                        }
                    }
                    ScalPic = 100;
                    MainPic.Pic = Main1;
                    MainPic.SetArray(PictureMass.BitmapToByteRgbQ(Main1));*/
                    pictureBox1.Image = MainPic.Pic;


                    PictureMass ress = PictureMass.ContrPic(cnt, mincntx, mincnty, maxcntx, maxcnty);
                    ArrayPic[4] = ress;
                    PicBox6.Image = ArrayPic[4].Pic;
                }
                catch (System.NullReferenceException)
                { MessageBox.Show("Исходные изображения пусты."); return; }
                catch (System.IndexOutOfRangeException)
                { MessageBox.Show("Не верно заданны номера изображений."); return; }
            }
        }

        private void piToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Номера изображений:";
            subForm.textBox1.Text = "1 2 3 4";
            subForm.textBox2.Hide();
            subForm.label2.Text = "Сдвиги фаз";
            subForm.richTextBox1.Text = "270 180 90 0 256";
            OKbutton = false;
            subForm.ShowDialog();
            if (OKbutton == true)
            {

                OKbutton = false;
                try
                {
                    //---ublic static int[,] TranscriptSet(int[] numberAr, PictureMass[] InPic, int[] Shift)
                    int width = ArrayPic[NumPic[0]].Array.GetLength(0),
                    height = ArrayPic[NumPic[0]].Array.GetLength(1),
                    Len = NumPic.Length;
                    double res;
                    PictureMass[] temp = new PictureMass[Len];
                    for (int i = 0; i < Len; i++)
                        temp[i] = ArrayPic[NumPic[i] - 1];
                    double[,] res2 = new double[width, height];
                    List<Point> cnt = new List<Point>();
                    Point tmp = new Point();
                    double sin, cos;

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            sin = 0; cos = 0;
                            for (int i = 0; i < Len; i++)
                            {
                                if (i == 0)
                                {
                                    sin += (temp[i + 1].Array[x, y] - temp[Len - 1].Array[x, y]) * Math.Sin(ShiftData[i] * Math.PI / 180);
                                    cos += (temp[Len - 1].Array[x, y] - temp[i + 1].Array[x, y]) * Math.Cos(ShiftData[i] * Math.PI / 180);
                                }
                                if (i == Len - 1)
                                {
                                    sin += (temp[0].Array[x, y] - temp[i - 1].Array[x, y]) * Math.Sin(ShiftData[i] * Math.PI / 180);
                                    cos += (temp[i - 1].Array[x, y] - temp[0].Array[x, y]) * Math.Cos(ShiftData[i] * Math.PI / 180);
                                }
                                if (i < Len - 1 && i > 0)
                                {
                                    sin += (temp[i + 1].Array[x, y] - temp[i - 1].Array[x, y]) * Math.Sin(ShiftData[i] * Math.PI / 180);
                                    cos += (temp[i - 1].Array[x, y] - temp[i + 1].Array[x, y]) * Math.Cos(ShiftData[i] * Math.PI / 180);
                                }
                            }
                            if ((y > 50) && (y < 53))
                            {
                                tmp.X = (int)sin;
                                tmp.Y = (int)cos;
                                cnt.Add(tmp);
                            }
                            if (sin == 0 && cos == 0) res = 0;
                            else res = (Math.Atan(sin / cos)) * 2;


                            res2[x, y] = res;
                        }
                    }
                    int mincntx = cnt.Min(p => p.X);
                    int mincnty = cnt.Min(p => p.Y);
                    int maxcntx = cnt.Max(p => p.X);
                    int maxcnty = cnt.Max(p => p.Y);


                    //----
                    MainPic.SetArray(res2);
                    MainPic.SetBitmap(PictureMass.BitmapMinMax(MainPic.Array));
                    /*Bitmap Main1 = new Bitmap(width, height);
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = width - 1; x > 0; x--)
                        {
                            Main1.SetPixel(x, y, Color.FromArgb((int)res2[x, y], (int)res2[x, y], (int)res2[x, y]));
                        }
                    }
                    ScalPic = 100;
                    MainPic.Pic = Main1;
                    MainPic.SetArray(PictureMass.BitmapToByteRgbQ(Main1));*/
                    pictureBox1.Image = MainPic.Pic;


                    PictureMass ress = PictureMass.ContrPic(cnt, mincntx, mincnty, maxcntx, maxcnty);
                    ArrayPic[4] = ress;
                    PicBox6.Image = ArrayPic[4].Pic;
                }
                catch (System.NullReferenceException)
                { MessageBox.Show("Исходные изображения пусты."); return; }
                catch (System.IndexOutOfRangeException)
                { MessageBox.Show("Не верно заданны номера изображений."); return; }
            }
        }

        private void графикПроизводныхToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Окно сглаживания:";
            subForm.label2.Hide();
            subForm.textBox1.Text = "3";
            subForm.textBox2.Hide();
            subForm.richTextBox1.Hide();
            subForm.ShowDialog();
            try
            {
                if (OKbutton == true && Median > 0)
                {
                    double[] Xar = new double[MainPic.Width];
                    double[] Yar = new double[MainPic.Height];
                    for (int i = 0; i < MainPic.Width; i++)
                        for (int j = 0; j < MainPic.Height - 1; j++)
                        {
                            Xar[i] += MainPic.Array[i, j];
                            Yar[j] += MainPic.Array[i, j];
                        }
                    for (int i = 0; i < MainPic.Width; i++)
                       { Xar[i] /= MainPic.Width; Xar[i] -= 0.25;}
                    for (int j = 0; j < MainPic.Height - 1; j++)
                        Yar[j] /= MainPic.Height;

                    double[] dXar = new double[MainPic.Width - 2];
                    double[] dYar = new double[MainPic.Width];
                    
                    Polynomial pol = new Polynomial(Xar, Median);
                    double[] pXar = new double[MainPic.Width];
                    for (int i = 1; i < MainPic.Width - 1; i++)
                        pXar[i] = pol.GetValue(i); ;

                    for (int i = 0; i < MainPic.Width; i++)
                        dYar[i] = Xar[i] - pXar[i];

                    for (int i = 2; i < MainPic.Width - 2; i++)
                        dXar[i - 1] = (pXar[i + 1] - pXar[i - 1]) / 2;

                    for (int j = 1; j < MainPic.Height - 2; j++)
                        dYar[j] = (Yar[j + 1] - Yar[j - 1]) / 2;

                    

                    Graph.Xar = pXar; Graph.Yar = dXar;
                    Graph.Owner = this;
                    Graph.Show();
                }
                else
                {
                    double[] Xar = new double[MainPic.Width];
                    for (int i = 0; i < MainPic.Width; i++)
                        for (int j = 0; j < MainPic.Height - 1; j++)
                        {
                            Xar[i] += MainPic.Array[i, j];
                        }
                    for (int i = 0; i < MainPic.Width; i++)
                        {Xar[i] /= MainPic.Width; Xar[i]-=0.25;}

                    double[] dXar = new double[MainPic.Width];
                    dXar[0] = 0; dXar[MainPic.Width - 1] = 0;
                    Xar = Filtration.Filt_smothingSM1(Xar, Median);

                    for (int i = 1; i < MainPic.Width - 2; i++)
                        dXar[i - 1] = (Xar[i + 1] - Xar[i - 1]) / 2;

                      /*  double[] Xar = new double[MainPic.Width];
                            for (int i = 0; i < MainPic.Width; i++)
                            {
                                double fgg = 1 - ((double)i / MainPic.Width);
                                Xar[i] = 1227.0825 * (fgg * fgg * fgg - 3 * fgg + 2);
                            }
                            for (int i = 0; i < MainPic.Width; i++)
                            { Xar[i] /= MainPic.Width; Xar[i] += 0.4; }
                            double[] dXar = new double[MainPic.Width];
                            dXar[0] = 0; dXar[MainPic.Width - 1] = 0;

                            for (int i = 1; i < MainPic.Width - 2; i++)
                                dXar[i - 1] = (Xar[i + 1] - Xar[i - 1]) / 2;*/
                        
                    Graph.Xar = Xar; Graph.Yar = dXar;
                    Graph.Owner = this;
                    Graph.Show();
                }
            }

            catch (Exception Ex) {MessageBox.Show("Изображение отсутствует"); return; }
            
        }

        #endregion

        //----------------------------------------------------------------------------------------------------------------
        #region Развертки

        private void стандартнаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Номер инд. изобр.:";
            subForm.label2.Hide();
            subForm.textBox2.Hide();
            subForm.richTextBox1.Hide();
            OKbutton = false;
            subForm.ShowDialog();
            if (OKbutton == true)
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                double[,] ArTim = new double[MainPic.Width, MainPic.Height], ArTimMin = new double[MainPic.Width, MainPic.Height];
                double[,] Temp = new double[ArrayPic[Median - 1].Width, ArrayPic[Median - 1].Height];
                Boolean[,] Znach = new Boolean[ArrayPic[Median - 1].Width, ArrayPic[Median - 1].Height];
                double Max = 0, Min = 0, Vr;
                for (int x = 0; x < MainPic.Width; x++)
                    for (int y = 0; y < MainPic.Height; y++)
                    {
                        Vr = ArrayPic[Median - 1].Array[x, y] - 10000;
                        if (Vr > 0)
                        {
                            Temp[x, y] = Vr - 100;
                            Znach[x, y] = true;
                            ArTim[x, y] = (255 * Temp[x, y]) + MainPic.Array[x, y];

                            if (Max < ArTim[x, y]) Max = ArTim[x, y];
                            if (Min > ArTim[x, y]) Min = ArTim[x, y];
                        }
                        else Temp[x, y] = Vr;

                    }

                MainPic.SetArray(ArTim);
                for (int x = 0; x < MainPic.Width; x++)
                    for (int y = 0; y < MainPic.Height; y++)
                    {
                        if (Znach[x, y])
                        {
                            ArTimMin[x, y] = ((int)(255 * (ArTim[x, y] + 1 - Min) / (Max - Min)));
                        }
                    }
                /* for (int x = 0; x < MainPic.Width; x++)
                     for (int y = 0; y < MainPic.Height; y++)
                     {
                         Vr = 255 * (ArrayPic[4].Array[x, y] - 10100);
                         if (Vr < (-99*255)) Vr = 0;
                         Vr1 = MainPic.Array[x, y];
                         if (Vr1 > 255) Vr1 = 255;
                         if (Vr1 < 0) Vr1 = 0;
                         Vr += Vr1;
                         ArTim[x, y] = Vr;
                         if (Max < Vr) Max = Vr;
                         if (Min > Vr) Min = Vr;
                     }
                 for (int x = 0; x < MainPic.Width; x++)
                     for (int y = 0; y < MainPic.Height; y++)
                     {
                         ArTimMin[x, y] = (int)(255 * (ArTim[x, y] - Min) / (Max - Min));
                         ArTim[x, y] += Math.Abs(Min);
                     }*/
                ScalPic = 100;
                MainPic.Pic = PictureMass.RgbToBitmapQ(ArTimMin);
                //MainPic.BuildBit();
                //MainPic.BuildBigBit(ArTim);
                pictureBox1.Image = MainPic.Pic;
            }
        }

        private void разверткаСМаскойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[,] ArTim = new double[MainPic.Width, MainPic.Height], ArTimMin = new double[MainPic.Width, MainPic.Height];
            double[,] Temp = new double[ArrayPic[4].Width, ArrayPic[4].Height];
            Boolean[,] Znach = new Boolean[ArrayPic[4].Width, ArrayPic[4].Height];
            double Max = 0, Min = 0, Vr;
            PictureMass tmp = new PictureMass(MainPic);
            Undo.NextDo(tmp);
            UndoButton.Enabled = true;
            for (int x = 0; x < MainPic.Width; x++)
                for (int y = 0; y < MainPic.Height; y++)
                {
                    Vr = ArrayPic[4].Array[x, y] - 10000;
                    if (Vr > 0 && ArrayPic[5].Array[x, y] == 1)
                    {
                        Temp[x, y] = Vr - 100;
                        Znach[x, y] = true;
                        ArTim[x, y] = (255 * Temp[x, y]) + MainPic.Array[x, y];

                        if (Max < ArTim[x, y]) Max = ArTim[x, y];
                        if (Min > ArTim[x, y]) Min = ArTim[x, y];
                    }
                    else
                        if (Vr == 0 && ArrayPic[5].Array[x, y] == 1)
                        {
                            double mine = 300, mine2;
                            for (int x0 = -2; x0 < 3; x0++)
                                for (int y0 = -2; y0 < 3; y0++)
                                {
                                    if (x + x0 < 0 || x + x0 > MainPic.Width - 1 || y + y0 < 0 || y + y0 > MainPic.Height - 1) continue;
                                    mine2 = Math.Abs(MainPic.Array[x, y] - MainPic.Array[x + x0, y + y0]);
                                    if (mine2 < mine && ArrayPic[4].Array[x + x0, y + y0] - 10000 > 0) Vr = ArrayPic[4].Array[x + x0, y + y0] - 10000;
                                }
                            if (Vr - 100 > 0)
                            {
                                Temp[x, y] = Vr - 100;
                                Znach[x, y] = true;
                            }
                            ArTim[x, y] = (255 * Temp[x, y]) + MainPic.Array[x, y];

                            if (Max < ArTim[x, y]) Max = ArTim[x, y];
                            if (Min > ArTim[x, y]) Min = ArTim[x, y];
                        }
                        else
                            if (ArrayPic[4].Array[x, y] == 0) ArrayPic[5].Array[x, y] = 0;
                            else Temp[x, y] = Vr;

                }

            MainPic.SetArray(ArTim);
            for (int x = 0; x < MainPic.Width; x++)
                for (int y = 0; y < MainPic.Height; y++)
                {
                    if (Znach[x, y])
                    {
                        ArTimMin[x, y] = ((int)(255 * (ArTim[x, y] + 1 - Min) / (Max - Min)));
                    }
                }

            ScalPic = 100;
            MainPic.Pic = PictureMass.RgbToBitmapQ(ArTimMin);

            //MainPic.BuildBit();
            //MainPic.BuildBigBit(ArTim);
            pictureBox1.Image = MainPic.Pic;
        }

        private void стандартнаяСШагом2PiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Номер инд. изобр.:";
            subForm.label2.Hide();
            subForm.textBox2.Hide();
            subForm.richTextBox1.Hide();
            OKbutton = false;
            subForm.ShowDialog();
            if (OKbutton == true)
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                double[,] ArTim = new double[MainPic.Width, MainPic.Height], ArTimMin = new double[MainPic.Width, MainPic.Height];
                double[,] Temp = new double[ArrayPic[Median - 1].Width, ArrayPic[Median - 1].Height];
                Boolean[,] Znach = new Boolean[ArrayPic[Median - 1].Width, ArrayPic[Median - 1].Height];
                double Max = 0, Min = 0, Vr;
                for (int x = 0; x < MainPic.Width; x++)
                    for (int y = 0; y < MainPic.Height; y++)
                    {
                        Vr = ArrayPic[Median - 1].Array[x, y] - 10000;
                        if (Vr > 0)
                        {
                            Temp[x, y] = Vr - 100;
                            Znach[x, y] = true;
                            ArTim[x, y] = (2 * Math.PI * Temp[x, y]) + MainPic.Array[x, y];

                            if (Max < ArTim[x, y]) Max = ArTim[x, y];
                            if (Min > ArTim[x, y]) Min = ArTim[x, y];
                        }
                        else Temp[x, y] = Vr;

                    }

                MainPic.SetArray(ArTim);
                for (int x = 0; x < MainPic.Width; x++)
                    for (int y = 0; y < MainPic.Height; y++)
                    {
                        if (Znach[x, y])
                        {
                            ArTimMin[x, y] = ((int)(255 * (ArTim[x, y] + 1 - Min) / (Max - Min)));
                        }
                    }
                ScalPic = 100;
                MainPic.Pic = PictureMass.RgbToBitmapQ(ArTimMin);
                pictureBox1.Image = MainPic.Pic;
            }
        }

        private void стандартнаяРазностьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Номер 2-го изобр.:";
            subForm.label2.Hide();
            subForm.textBox2.Hide();
            subForm.richTextBox1.Hide();
            OKbutton = false;
            subForm.ShowDialog();
            if (OKbutton == true)
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                double[,] ArTim = new double[MainPic.Width, MainPic.Height];
                for (int x = 0; x < MainPic.Width; x++)
                    for (int y = 0; y < MainPic.Height; y++)
                    {
                        ArTim[x, y] = (MainPic.Array[x, y] - ArrayPic[Median - 1].Array[x, y]) % 255;
                        if (ArTim[x, y] < 0) ArTim[x, y] += 255;
                    }

                MainPic.SetArray(ArTim);

                ScalPic = 100;
                MainPic.Pic = PictureMass.RgbToBitmapQ(ArTim);
                //MainPic.BuildBit();
                //MainPic.BuildBigBit(ArTim);
                pictureBox1.Image = MainPic.Pic;
            }
        }

        private void табличнаяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Номер 1-го и 2-го изобр.:";
            subForm.textBox1.Text = "1 2";
            subForm.textBox2.Hide();
            subForm.label2.Text = "m1 и m2 и диаг.:";
            subForm.richTextBox1.Text = "167 241 13 0 10";
            OKbutton = false;
            subForm.ShowDialog();
            if (OKbutton == true)
            {
                PictureMass tmp = new PictureMass(MainPic);
                Undo.NextDo(tmp);
                UndoButton.Enabled = true;
                OKbutton = false;
                try
                {
                    PictureMass trans = Unrup.Unrup_array(ArrayPic[NumPic[1] - 1], ArrayPic[NumPic[0] - 1], ShiftData[0], ShiftData[1], ShiftData[2], ShiftData[3], ShiftData[4]);

                    MainPic.CopyArray(trans);
                    MainPic.Pic = PictureMass.BitmapMinMax(MainPic.Array);

                    ScalPic = 100;
                    pictureBox1.Image = MainPic.Pic;
                }
                catch (System.NullReferenceException)
                { MessageBox.Show("Исходные изображения пусты."); return; }
            }
        }


        #endregion

        Boolean first = true;
        //----------------------------------------------------------------------------------------------------------------
        #region Рабочая панель
        private void instumentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!first)
                if (instumentComboBox.SelectedItem.ToString() == "Кисть") { pictureBox1.Paint += ell_Paint; SizeBox1.Enabled = true; }
                else { pictureBox1.Paint -= ell_Paint; SizeBox1.Enabled = false; }
            switch (instumentComboBox.SelectedItem.ToString())
            {
                case "Указатель":
                    { b_e_Button.Enabled = false; gal_Button1.Enabled = false; indexText.Enabled = false; EnterButton.Enabled = false; break; }
                case "Графики":
                    { b_e_Button.Enabled = false; gal_Button1.Enabled = false; indexText.Enabled = false; EnterButton.Enabled = false; break; } //графики
                case "Востановление обрывов":
                    { b_e_Button.Enabled = true; gal_Button1.Enabled = true; gal_Button1.Text = "Соедениить линией"; indexText.Enabled = false; EnterButton.Enabled = false; break; }
                case "Нарисовать линию":
                    { b_e_Button.Enabled = true; gal_Button1.Enabled = false; indexText.Enabled = true; toolStripLabel2.Text = "Соедениить линией"; EnterButton.Enabled = false; break; }
                case "Вырезать":
                    { b_e_Button.Enabled = false; gal_Button1.Enabled = true; gal_Button1.Text = "Вырезать несколько"; indexText.Enabled = false; EnterButton.Enabled = false; break; }
                case "Заполнение полос":
                    { b_e_Button.Enabled = false; gal_Button1.Enabled = false; indexText.Enabled = true; toolStripLabel2.Text = "Индекс полосы:"; EnterButton.Enabled = false; break; } //заполнение
                case "Заполнить и стереть":
                    { b_e_Button.Enabled = false; gal_Button1.Enabled = false; indexText.Enabled = false; toolStripLabel2.Text = "Индекс полосы:"; EnterButton.Enabled = true; break; } //заполнение
                case "Закрасить прямоугольник":
                    { b_e_Button.Enabled = false; gal_Button1.Enabled = true; gal_Button1.Text = "Выделить как границу"; indexText.Enabled = true; toolStripLabel2.Text = "Цвет:"; EnterButton.Enabled = false; break; } //заполнение
                case "Кисть":
                    { b_e_Button.Enabled = false; gal_Button1.Enabled = true; gal_Button1.Text = "Выделить как границу"; indexText.Enabled = true; toolStripLabel2.Text = "Цвет:"; EnterButton.Enabled = false; break; } //заполнение
                default: { break; }
            }
            first = false;
        }

        private void b_e_Button_Click(object sender, EventArgs e)
        {
            if (b_e_Button.Text == "B")
            {
                b_e_Button.Text = "E";
                Line_b = true;
            }
            else
            {
                b_e_Button.Text = "B";
                Line_b = false;
            }
        }


        private void SizeBox1_TextChanged(object sender, EventArgs e)
        {
            Size1 = Convert.ToInt32(SizeBox1.Text);
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            if (Undo.GetUndo() == true)
            {
                MainPic = Undo.Undo(MainPic);
                if (PictureMass.GetMinMax(MainPic.Array) < 200)
                {
                    MainPic.SetBitmap(PictureMass.BitmapMinMax(MainPic.Array));
                    pictureBox1.Image = MainPic.Pic;
                }
                else
                {
                    MainPic.BuildBit();
                    PicScale();
                }
                RedoButton.Enabled = true;
                if (Undo.GetUndo() == false)
                    UndoButton.Enabled = false;
            }
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            if (Undo.GetRedo() == true)
            {
                MainPic = Undo.Redo(MainPic);
                if (PictureMass.GetMinMax(MainPic.Array) < 200)
                {
                    MainPic.SetBitmap(PictureMass.BitmapMinMax(MainPic.Array));
                    pictureBox1.Image = MainPic.Pic;
                }
                else
                {
                    MainPic.BuildBit();
                    PicScale();
                }
                UndoButton.Enabled = true;
                if (Undo.GetRedo() == false)
                    RedoButton.Enabled = false;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z && (e.Control))
            {
                UndoButton_Click(sender, e);
            }
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            установитьГраницыToolStripMenuItem_Click(sender, e);
        }

        bool textb = false;

        private void subForm_GenToMain(object sender, EventArgs e)
        {
            pictureBox1.Image = MainPic.Pic;
        }
        #endregion

        //----------------------------------------------------------------------------------------------------------------
        #region Visualise
        private void пиксельнаяФормаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZArrayDescriptor zArrayDescriptor = new ZArrayDescriptor(MainPic.Width, MainPic.Height);
            for (int i = 1; i < MainPic.Width; i++)
            {
                for (int j = 1; j < MainPic.Height; j++)
                {
                    zArrayDescriptor.array[MainPic.Width - i, MainPic.Height - j] = MainPic.Array[i, j];
                }
            }

            if (zArrayDescriptor != null)
            {
                TransPi.Visualisation.VisualisationWindow visualisationWindow =
                    new TransPi.Visualisation.VisualisationWindow(zArrayDescriptor, TransPi.Visualisation.Mesh.ColoringMethod.Grayscale,
                        new TransPi.Visualisation.BoundCamera(new OpenTK.Vector3(0, 0, 0), 0, 1.47f, 1000.0f));
                visualisationWindow.Run();
            }
        }

        private void сТекстуройToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParamDialogForm subForm = new ParamDialogForm(this);
            subForm.label1.Text = "Текстура:";
            subForm.label2.Hide();
            subForm.textBox2.Hide();
            subForm.richTextBox1.Hide();
            subForm.ShowDialog();
            if (OKbutton == true)
            {
                Bitmap texture = ArrayPic[Median - 1].Pic;
                if (!textb)
                {
                    texture.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                    textb = true;
                }
                ZArrayDescriptor zArrayDescriptor = new ZArrayDescriptor(MainPic.Width, MainPic.Height);
                for (int i = 1; i < MainPic.Width; i++)
                {
                    for (int j = 1; j < MainPic.Height; j++)
                    {
                        zArrayDescriptor.array[MainPic.Width - i, MainPic.Height - j] = MainPic.Array[i, j];
                    }
                }

                if (zArrayDescriptor != null && texture != null)
                {
                    TransPi.Visualisation.VisualisationWindow visualisationWindow =
                        new TransPi.Visualisation.VisualisationWindow(zArrayDescriptor, TransPi.Visualisation.Mesh.ColoringMethod.Texture, texture,
                            new TransPi.Visualisation.BoundCamera(new OpenTK.Vector3(0, 0, 0), 0, 1.47f, 1000.0f));
                    visualisationWindow.Run();
                }
            }


        }

        private void загрузитьОбъектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "obj files (*.obj)|*.obj";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            ScalPic = 100;
                            FileInfo fileInfo = new FileInfo(openFileDialog1.FileName);
                            if (fileInfo.Extension.ToLower() == ".obj")
                            {
                                TransPi.Visualisation.VisualisationWindow visualisationWindow =
                                    new TransPi.Visualisation.VisualisationWindow(openFileDialog1.FileName, new TransPi.Visualisation.BoundCamera(new OpenTK.Vector3(0, 0, 0), 0, 1.47f, 1000.0f));
                                visualisationWindow.Run();
                            }

                            Undo.ClearUR();
                            RedoButton.Enabled = false;
                            UndoButton.Enabled = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Загрузка не удалась. " + ex.Message);
                }
            }
        }
        #endregion

        //----------------------------------------------------------------------------------------------------------------
        #region Camera
        private void дизерингToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateSin subForm = new GenerateSin(this);
            subForm.GenToMain += subForm_GenToMain;
            subForm.ShowDialog();
        }

        private void снимкиCanonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenSin2 subForm = new GenSin2(this);
            subForm.GenToMain += subForm_GenToMain;

            OKbutton = false;
            subForm.ShowDialog();

            if (OKbutton == true)
            {
                OKbutton = false;
                List<PictureMass> temppic = new List<PictureMass> { ArrayPic[0], ArrayPic[1], ArrayPic[2], ArrayPic[3], ArrayPic[4], ArrayPic[5], ArrayPic[6], ArrayPic[7] };

                int[] tempshift = new int[4] { 270, 180, 90, 0 };
                double[,] new_pic = PictureMass.Trans4(temppic.GetRange(0, 4), tempshift);
                ArrayPic[9] = new PictureMass();
                ArrayPic[9].SetArray(new_pic);
                ArrayPic[9].SetBitmap(PictureMass.BitmapMinMax(ArrayPic[9].Array));
                PicBox10.Image = ArrayPic[9].Pic;
                this.Refresh();


                double[,] new_pic2 = PictureMass.Trans4(temppic.GetRange(4, 4), tempshift);
                ArrayPic[10] = new PictureMass();
                ArrayPic[10].SetArray(new_pic2);
                ArrayPic[10].SetBitmap(PictureMass.BitmapMinMax(ArrayPic[10].Array));
                PicBox11.Image = ArrayPic[10].Pic;


                this.Refresh();
                ArrayPic[11] = new PictureMass();
                ArrayPic[11] = PictureMass.ScalleZ(Unrup.Unrup_array(ArrayPic[10], ArrayPic[9], 167, 241, 13, 0, 10), 0.2);
                ArrayPic[11].SetBitmap(PictureMass.BitmapMinMax(ArrayPic[11].Array));
                PicBox12.Image = ArrayPic[11].Pic;

                //MainPic = PictureMass.ScalleZ(ArrayPic[11], 0.2);
            }
        }

        #endregion

      


























    }
}
