using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using System.Drawing.Imaging;
using System.Text;

namespace TransPi
{
    public class PictureMass
    {
        public int Width, Height;
        public double[,] Array;
        public Bitmap Pic;

        public PictureMass()
        {
            Width = 0;
            Height = 0;
            Array = new double[Width, Height];
            Pic = null;
        }

        public void PMClear()
        {
            Width = 0;
            Height = 0;
            Array = new double[Width, Height];
            Pic = null;
        }

        public PictureMass(PictureMass In)
        {
            Width = In.Width;
            Height = In.Height;
            Array = In.Array;
            Pic = In.Pic;
        }

        public PictureMass(int X, int Y, double [,] InArray, Bitmap InPic)
        {
            Width = X;
            Height = Y;
            Array = InArray;
            Pic = InPic;
        }

        public void SetSize(int X, int Y)
        {
            Width = X;
            Height = Y;
            Array = null;
            Array = new double[Width, Height];
        }

        public void SetArray(double[,] InArray)
        {
            Width = InArray.GetLength(0);
            Height = InArray.GetLength(1);
            Array = null;
            Array = InArray;
        }

        public void SetBitmap(Bitmap IN)
        {
            Pic = IN;
        }

        public static void SetBitmap(Bitmap IN, PictureMass Out)
        {
            Out.Pic = IN;
            Out.Array = BitmapToByteRgbQ(IN);
            Out.Width = IN.Width;
            Out.Height = IN.Height;
        }

        public void CopyArray(PictureMass InImg)
        {
            try
            {
                Width = InImg.Width;
                Height = InImg.Height;
                Array = InImg.Array;
                Pic = InImg.Pic;
            }
            catch (System.NullReferenceException) { }
        }

        public static PictureMass DellBack(PictureMass InImg, PictureMass Back)
        {
            try
            {
                int Width = InImg.Width;
                int Height = InImg.Height;


                double[,] ArTim = new double[Width, Height];
                double[,] ArTimMin = new double[Width, Height];
                Boolean[,] Znach = new Boolean[Width, Height];
                double Min = 1000000, Max = -1000000;
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (InImg.Array[x, y] > 0)
                        {
                            ArTim[x, y] = InImg.Array[x, y] - Back.Array[x, y];
                            Znach[x, y] = true;
                            if (Min > ArTim[x, y]) Min = ArTim[x, y];
                            if (Max < ArTim[x, y]) Max = ArTim[x, y];
                        }
                    }
                }
                /*for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                        if (Znach[x, y]) ArTim[x, y] -= Min;*/
                PictureMass Outimg = new PictureMass();
                Outimg.SetArray(ArTim);
                Outimg.Pic = BitmapMinMax(ArTim, Znach, Min, Max);
                return Outimg;
            }
            catch (System.NullReferenceException) { return null; }
        }


        public static PictureMass ScallePic(PictureMass InImg,double Min, double Max)
        {
            int width = InImg.Width,
            height = InImg.Height;
            double[,] ArTimMin = new double[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                        try
                        {
                            ArTimMin[x, y] = (255 * (InImg.Array[x, y] + 1 - Min) / (Max - Min));
                        }
                        catch (System.DivideByZeroException) { ArTimMin[x, y] = 0; }
                    
                }
            PictureMass res = new PictureMass(width, height, ArTimMin, PictureMass.RgbToBitmapQ(ArTimMin));
            return res;
        }

        public static PictureMass ScallePicWithMask(PictureMass InImg, PictureMass Mask, double Min, double Max, double Scale)
        {
            int width = InImg.Width,
            height = InImg.Height;
            double[,] ArTimMin = new double[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    try
                    {
                        if (Mask.Array[x,y] > 0)
                        { 
                            ArTimMin[x, y] = (255 * (InImg.Array[x, y] + 1 - Min) / (Max - Min))*Scale;
                        }
                        else ArTimMin[x, y] = 0;
                    }
                    catch (System.DivideByZeroException) { ArTimMin[x, y] = 0; }

                }
            PictureMass res = new PictureMass(width, height, ArTimMin, PictureMass.RgbToBitmapQ(ArTimMin));
            return res;
        }

        public static PictureMass ScalleZ(PictureMass InImg, double Scale)
        {
            int width = InImg.Width,
            height = InImg.Height;
            double[,] ArTimMin = new double[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    try
                    {
                        ArTimMin[x, y] = Scale * InImg.Array[x, y] ;
                    }
                    catch (System.DivideByZeroException) { ArTimMin[x, y] = 0; }

                }
            PictureMass res = new PictureMass(width, height, ArTimMin, PictureMass.RgbToBitmapQ(ArTimMin));
            return res;
        }

        public static PictureMass DelMinMax(PictureMass InImg, int Min, int Max)
        {
            int width = InImg.Width,
            height = InImg.Height;
            double[,] ArTimMin = new double[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    try
                    {
                        if (InImg.Array[x, y] < Max && InImg.Array[x, y] > Min)
                            ArTimMin[x, y] = InImg.Array[x, y];
                        else  ArTimMin[x, y] = 0;
                    }
                    catch (System.DivideByZeroException) { ArTimMin[x, y] = 0; }

                }
            PictureMass res = new PictureMass(width, height, ArTimMin, PictureMass.BitmapMinMax(ArTimMin));
            return res;
        }

        public static Bitmap BitmapMinMax(double[,] bmp, Boolean[,] Znach, double Min, double Max)
        {
            int width = bmp.GetLength(0),
            height = bmp.GetLength(1);
            double[,] ArTimMin = new double[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (Znach[x, y])
                    {
                        try
                        {
                            ArTimMin[x, y] = (255 * (bmp[x, y] + 1 - Min) / (Max - Min));
                        }
                        catch (System.DivideByZeroException) { ArTimMin[x, y] = 0; }
                    }
                }

            Bitmap res = PictureMass.RgbToBitmapQ(ArTimMin);
            return res;
        }

        public static double GetMinMax(double[,] bmp)
        {
            int width = bmp.GetLength(0),
            height = bmp.GetLength(1);
            double Min = 1000000, Max = -1000000;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (Min > bmp[x, y]) Min = bmp[x, y];
                    if (Max < bmp[x, y]) Max = bmp[x, y];
                }
            }
            return Max - Min;
        }

        public static double GetMax(double[,] bmp)
        {
            int width = bmp.GetLength(0),
            height = bmp.GetLength(1);
            double Max = -1000000;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (Max < bmp[x, y]) Max = bmp[x, y];
            return Max;
        }

        public static double GetMin(double[,] bmp)
        {
            int width = bmp.GetLength(0),
            height = bmp.GetLength(1);
            double Min = 1000000;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (Min > bmp[x, y]) Min = bmp[x, y];
            return Min;
        }

        public static Bitmap BitmapMinMax(double[,] bmp)
        {
            int width = bmp.GetLength(0),
            height = bmp.GetLength(1);
            double[,] ArTimMin = new double[width, height];
            double Min = 1000000, Max = -1000000;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (Min > bmp[x, y]) Min = bmp[x, y];
                    if (Max < bmp[x, y]) Max = bmp[x, y];
                }
            }
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                        try
                        {
                            ArTimMin[x, y] = (255 * (bmp[x, y] + 1 - Min) / (Max - Min));
                        }
                        catch (System.DivideByZeroException) { ArTimMin[x, y] = 0; }
                }

            Bitmap res = PictureMass.RgbToBitmapQ(ArTimMin);
            return res;
        }

        public static double[,] BitmapToByteRgbQ1(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            double[,] res = new double[width, height];
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Color color = bmp.GetPixel(x, y);
                    res[x, y] = (int)((color.R + color.G + color.B) / 3);
                }
            }
            return res;
        }
        public unsafe static double[,] BitmapToByteRgbQ(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            double[,] res2 = new double[width, height];
            byte[, ,] res = new byte[3, height, width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
            PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                for (int h = 0; h < height; h++)
                {
                    curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        res[2, h, w] = *(curpos++);
                        res[1, h, w] = *(curpos++);
                        res[0, h, w] = *(curpos++);
                        res2[w, h] = (int)((res[0, h, w] + res[1, h, w] + res[2, h, w]) / 3);
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return res2;
        }
        public unsafe static byte[,,] BitmapToByteRgbQ2(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[, ,] res = new byte[3, height, width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
            PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                for (int h = 0; h < height; h++)
                {
                    curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        res[2, h, w] = *(curpos++);
                        res[1, h, w] = *(curpos++);
                        res[0, h, w] = *(curpos++);
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return res;
        }

        public static Bitmap RgbToBitmapQ1(double[,] rgb)
        {
            int width = rgb.GetLength(1),
                height = rgb.GetLength(0);

            Bitmap result = new Bitmap(height, width);

            for (int y = 0; y < width; ++y)
            {
                for (int x = 0; x < height; ++x)
                {
                    result.SetPixel(x, y, Color.FromArgb((int)rgb[x, y], (int)rgb[x, y], (int)rgb[x, y]));
                }
            }

            return result;
        }
        public unsafe static Bitmap RgbToBitmapQ(double[,] rgb1)
        {
            int width = rgb1.GetLength(0),
                height = rgb1.GetLength(1);
            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bd = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                for (int h = 0; h < height; h++)
                {
                    curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        *(curpos++) = (byte)rgb1[w, h];
                        *(curpos++) = (byte)rgb1[w, h];
                        *(curpos++) = (byte)rgb1[w, h];
                    }
                }
            }
            finally
            {
                result.UnlockBits(bd);
            }

            return result;
        }
        public unsafe static Bitmap RgbToBitmapQ2(byte[, ,] rgb)
        {
            if ((rgb.GetLength(0) != 3))
            {
                throw new ArrayTypeMismatchException("Size of first dimension for passed array must be 3 (RGB components)");
            }

            int width = rgb.GetLength(2),
                height = rgb.GetLength(1);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bd = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            try
            {
                byte* curpos;
                fixed (byte* _rgb = rgb)
                {
                    byte* _r = _rgb, _g = _rgb + 1, _b = _rgb + 2;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *(curpos++) = *_b; _b += 3;
                            *(curpos++) = *_g; _g += 3;
                            *(curpos++) = *_r; _r += 3;
                        }
                    }
                }
            }
            finally
            {
                result.UnlockBits(bd);
            }

            return result;
        }


        public unsafe void BuildBit()
        {
            Pic = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            BitmapData bd = Pic.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly,
               PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                for (int y = 0; y < Height; y++)
                {
                    curpos = ((byte*)bd.Scan0) + y * bd.Stride;
                    for (int x = 0; x < Width; x++)
                    {
                        if (Array[x, y] < 256)
                        {
                            *(curpos++) = (byte)Array[x, y];
                            *(curpos++) = (byte)Array[x, y];
                            *(curpos++) = (byte)Array[x, y];
                        }
                        else
                            if (Array[x, y] == 10000)
                            {
                                *(curpos++) = (byte)0;
                                *(curpos++) = (byte)0;
                                *(curpos++) = (byte)255;
                            }
                            else
                            {
                                switch (((int)Array[x, y] - 10000) % 5)
                                {
                                    case 0:
                                        {
                                            *(curpos++) = (byte)255;
                                            *(curpos++) = (byte)0;
                                            *(curpos++) = (byte)0; break;
                                        }
                                    case 1:
                                        {
                                            *(curpos++) = (byte)0;
                                            *(curpos++) = (byte)255;
                                            *(curpos++) = (byte)0; break;
                                        }
                                    case 2:
                                        {
                                            *(curpos++) = (byte)0;
                                            *(curpos++) = (byte)255;
                                            *(curpos++) = (byte)255; break;
                                        }
                                    case 3:
                                        {
                                            *(curpos++) = (byte)255;
                                            *(curpos++) = (byte)0;
                                            *(curpos++) = (byte)255; break;
                                        }
                                    case 4:
                                        {
                                            *(curpos++) = (byte)255;
                                            *(curpos++) = (byte)255;
                                            *(curpos++) = (byte)0; break;
                                        }
                                }
                            }
                    }
                }
            }

            finally
            {
                Pic.UnlockBits(bd);
            }

        }

        public static PictureMass cutRec(PictureMass In,int x1, int y1, int x2, int y2)
        {
            PictureMass Out = new PictureMass();
            Out.Width = Math.Abs(x1 - x2);
            Out.Height = Math.Abs(y1 - y2);
            Out.Array = new double[Out.Width, Out.Height];
            int x_f, y_f;
            if (x1 < x2) x_f = x1;
            else x_f = x2;
            if (y1 < y2) y_f = y1;
            else y_f = y2;
            try{
            for (int x = 0; x < Out.Width; x++)
                for (int y = 0; y < Out.Height;y++)
                {
                    Out.Array[x, y] = In.Array[x + x_f, y + y_f];
                }
            Out.Pic = new Bitmap(Out.Width, Out.Height);
            using (Graphics g = Graphics.FromImage(Out.Pic))
            {

                g.DrawImage(In.Pic, 0, 0, new Rectangle(x_f, y_f, x_f+Out.Width,y_f+ Out.Height), GraphicsUnit.Pixel);
            }
            }
            catch (Exception Ex) { }
            //Out.Pic = In.Pic.Clone(new Rectangle(x_f, x_f, Out.Width, Out.Height), In.Pic.PixelFormat);
            return Out;
        }

        public static PictureMass fillRec(PictureMass In, Point top, Point bot, double color)
        {
            PictureMass Out = In;
            for (int x = top.X; x < bot.X-top.X; x++)
                for (int y = top.Y; y < bot.Y - top.Y; y++)
                {
                    Out.Array[x, y] = color;
                }
            //Out.Pic = In.Pic.Clone(new Rectangle(x_f, x_f, Out.Width, Out.Height), In.Pic.PixelFormat);
            return Out;
        }
        

        public unsafe void BuildBigBit(double[,] InArray)
        {
            int width = InArray.GetLength(0),
                height = InArray.GetLength(1);
            double[,] tempa = new double[width, height];
            double Max=0, Min=0;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (Max < InArray[x, y]) Max = InArray[x, y];
                    if (Min > InArray[x, y]) Min = InArray[x, y];
                }
            for (int x = 0; x <width; x++)
                for (int y = 0; y < height; y++)
                    tempa[x, y] = (int)(255 * (InArray[x, y] - Min) / (Max - Min));
            Pic = new Bitmap(width, height);
            BitmapData bd = Pic.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly,
               PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                for (int y = 0; y < Height; y++)
                {
                    curpos = ((byte*)bd.Scan0) + y * bd.Stride;
                    for (int x = 0; x < Width; x++)
                    {
                        if (Array[x, y] < 256)
                        {
                            *(curpos++) = (byte)tempa[x, y];
                            *(curpos++) = (byte)tempa[x, y];
                            *(curpos++) = (byte)tempa[x, y];
                        }
                        else
                            if (Array[x, y] == 10000)
                            {
                                *(curpos++) = (byte)0;
                                *(curpos++) = (byte)0;
                                *(curpos++) = (byte)255;
                            }
                            else
                            {
                                switch (((int)Array[x, y] - 10000) % 5)
                                {
                                    case 0:
                                        {
                                            *(curpos++) = (byte)255;
                                            *(curpos++) = (byte)0;
                                            *(curpos++) = (byte)0; break;
                                        }
                                    case 1:
                                        {
                                            *(curpos++) = (byte)0;
                                            *(curpos++) = (byte)255;
                                            *(curpos++) = (byte)0; break;
                                        }
                                    case 2:
                                        {
                                            *(curpos++) = (byte)0;
                                            *(curpos++) = (byte)255;
                                            *(curpos++) = (byte)255; break;
                                        }
                                    case 3:
                                        {
                                            *(curpos++) = (byte)255;
                                            *(curpos++) = (byte)0;
                                            *(curpos++) = (byte)255; break;
                                        }
                                    case 4:
                                        {
                                            *(curpos++) = (byte)255;
                                            *(curpos++) = (byte)255;
                                            *(curpos++) = (byte)0; break;
                                        }
                                }
                            }
                    }
                }
            }

            finally
            {
                Pic.UnlockBits(bd);
            }

        }

      


        public static PictureMass ContrPic(List<Point> Vh_po, int minx, int miny, int maxx, int maxy)
        {
            double [,] u = new double[200,200];
            for (int i=0;i<199;i++)
                for (int j=0;j<199;j++)
                    u[i,j]=255;
            PictureMass res = new PictureMass ();
            res.SetArray(u);
            List<Point> proverka = new List<Point>();
            foreach (Point po in Vh_po)
            {
                int Xr = ((po.X - minx) * 199 / (maxx - minx));
                int Yr = ((po.Y - miny) * 199 / (maxy - minx));
                try
                {
                //if (res.Array[Xr, Yr] >= 63)
                    res.Array[Xr, Yr] = 0;
                }
                catch
                {

                }
                
            }

            res.Pic = new Bitmap(PictureMass.RgbToBitmapQ(res.Array));

                    
            return res;
        }

        public static double[,] Trans4(List<PictureMass> pic, int[] shift)
        {
            PictureMass result = new PictureMass();
            //---ublic static int[,] TranscriptSet(int[] numberAr, PictureMass[] InPic, int[] Shift)
            int width = pic[0].Array.GetLength(0),
            height = pic[0].Array.GetLength(1),
            Len = pic.Count;
            double res;
            PictureMass[] temp = new PictureMass[Len];
            for (int i = 0; i < Len; i++)
                temp[i] = pic[i];
            double[,] res2 = new double[width, height];
            List<Point> cnt = new List<Point>();
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
                            sin += (temp[i + 1].Array[x, y] - temp[Len - 1].Array[x, y]) * Math.Sin(shift[i] * Math.PI / 180);
                            cos += (temp[Len - 1].Array[x, y] - temp[i + 1].Array[x, y]) * Math.Cos(shift[i] * Math.PI / 180);
                        }
                        if (i == Len - 1)
                        {
                            sin += (temp[0].Array[x, y] - temp[i - 1].Array[x, y]) * Math.Sin(shift[i] * Math.PI / 180);
                            cos += (temp[i - 1].Array[x, y] - temp[0].Array[x, y]) * Math.Cos(shift[i] * Math.PI / 180);
                        }
                        if (i < Len - 1 && i > 0)
                        {
                            sin += (temp[i + 1].Array[x, y] - temp[i - 1].Array[x, y]) * Math.Sin(shift[i] * Math.PI / 180);
                            cos += (temp[i - 1].Array[x, y] - temp[i + 1].Array[x, y]) * Math.Cos(shift[i] * Math.PI / 180);
                        }
                    }
                   
                    if (sin == 0 && cos == 0) res = 0;
                    else res = (Math.Atan(sin / cos)) * 2;

                    res2[x, y] = res;
                }
            }
            //----
            
            return res2;
        }



    }

}
