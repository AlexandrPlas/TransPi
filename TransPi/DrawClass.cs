using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace TransPi
{
    class DrawClass
    {
        PictureMass MainPic = MainForm.MainPic;

        public static Point Blijh(double[,] InArray, int x, int y, int clr, int wind)
        {
            Point End = new Point(x, y);
            Boolean[,] Prav = new Boolean[wind * 2 + 1, wind * 2 + 1];
            double Min = wind + 1; double rad;
            int count = 1;
            while (count < wind)
            {
                for (int xi = x - count; xi < x + count; xi++)
                    for (int yi = y - count; yi < y + count; yi++)
                    {
                        if (xi < 0 || yi < 0 || xi >= InArray.GetLength(0) || yi >= InArray.GetLength(1) || Prav[xi - x + wind, yi - y + wind] == true) continue;
                        Prav[xi - x + wind, yi - y + wind] = true;
                        rad = Math.Sqrt(((xi - x) * (xi - x)) + ((yi - y) * (yi - y)));
                        if ((int)rad <= count)
                        {
                            if (InArray[xi, yi] == clr)
                                if (Min > rad) { Min = (int)rad; End.X = xi; End.Y = yi; }
                        }
                    }
                if (Min < wind + 1) break;
                count++;
            }

            return End;
        }
        public static void LineSpr(double[,] InArray, int x1, int y1, int x2, int y2, int str)
        {
            int width = Math.Abs(x1 - x2) + str * 2, height = Math.Abs(y1 - y2) + str * 2;
            double[,] SubArr = new double[width, height];

            int Xl, Xr, Yt, Yb, sub_x1, sub_x2, sub_y1, sub_y2;
            if (x1 < x2) { Xl = x1; Xr = x2; sub_x1 = str; sub_x2 = width - str; } else { Xl = x2; Xr = x1; sub_x2 = str; sub_x1 = width - str; }
            if (y1 < y2) { Yt = y1; Yb = y2; sub_y1 = str; sub_y2 = height - str; } else { Yt = y2; Yb = y1; sub_y2 = str; sub_y1 = height - str; }
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (x + Xl - str < 0 || y + Yt - str < 0 || x + Xl - str >= InArray.GetLength(0) || y + Yt - str >= InArray.GetLength(1)) continue;
                    SubArr[x, y] = InArray[x + Xl - str, y + Yt - str];
                }
            double Clr = InArray[x2, y2];
            double[,] BAr = new double[width, height];
            byte[,] temp = new byte[width, height];
            temp[sub_x1, sub_y1] = 1;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (SubArr[x, y] == Clr) BAr[x, y] = 1; else BAr[x, y] = 0;

            Point Rez = new Point(sub_x1, sub_y1);
            BAr[sub_x1, sub_y1] = 1; int Xtemp = 0, Ytemp = 0;
            while (BAr[sub_x2, sub_y2] != 10000 && temp[sub_x2, sub_y2] != 2)
            {
                Xtemp = Rez.X; Ytemp = Rez.Y;
                Rez = WhileLineFill(BAr, temp, Rez.X, Rez.Y);
                if (Xtemp == Rez.X && Ytemp == Rez.Y)
                { Rez.X = sub_x2; Rez.Y = sub_y2; }
            }

            /*int raz1 = 0, raz2 = 0;
            while (BAr[sub_x2,sub_y2]<2)
            {
                WhileLineFill(BAr, temp, sub_x1 + raz1, sub_y1 + raz2, 10);
                if (width > height) if (Napr) raz1++; else raz1--;
                else if (Napr2) raz2++; else raz2--;
                
            }
            int tx = sub_x2, ty = sub_y2;
            BAr[sub_x2, sub_y2] = -1;
            while (BAr[sub_x1,sub_y1]!=-1)
            {
                int MinZ=9999999, tempX=0,tempY=0;
                for (int x=tx-1;x<=tx+1;x++)
                    for(int y=ty-1;y<=ty+1;y++)
                    {
                        if (x < 0 || x >= width || y < 0 || y >= height|| (x==tx&&y==ty)) continue;
                        if (BAr[x, y] < MinZ && BAr[x, y]>0) { MinZ = BAr[x, y]; tempX = x; tempY = y; }
                    }
                BAr[tempX, tempY] = -1;
                if (tx == tempX && ty == tempY) break;
                tx = tempX; ty = tempY;

            }
            for (int x = 1; x < width; x++)
                for (int y = 1; y < height; y++)
                    if (BAr[x, y] == -1) BAr[x, y] = 10000; else BAr[x, y] = 0;*/

            //BAr=LinePix(BAr, SubArr, sub_x1, sub_y1);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (BAr[x, y] == 10000) SubArr[x, y] = 10000;

            //оставим еденичные линие
            /*double rad;
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    for (int xi = x - str; xi < x + str; xi++)
                        for (int yi = y - str; yi < y + str; yi++)
                        {
                            if (xi < 0 || yi < 0 || xi >= width || yi >= height || BAr[xi, yi] == -1) continue;
                            rad = Math.Sqrt(((xi - x) * (xi - x)) + ((yi - y) * (yi - y)));
                            if (rad <= str)
                            {
                                if (BAr[xi, yi] == -1) SubArr[xi,yi]=Clr+1;
                                else SubArr[xi, yi] = 0;
                            }
                        }
            */

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (x + Xl - str < 0 || y + Yt - str < 0 || x + Xl - str >= InArray.GetLength(0) || y + Yt - str >= InArray.GetLength(1)) continue;
                    InArray[x + Xl - str, y + Yt - str] = SubArr[x, y];
                }

        }

        public static void PixelFill(PictureMass MainPic, int x, int y, double BolderColor, double color)
        {
            if (x >= 0 && y >= 0 && x < MainForm.MainPic.Width && y < MainPic.Height)
            {
                double c = MainPic.Array[x, y];
                if ((c != BolderColor) && (c != color))
                {
                    MainPic.Array[x, y] = color;
                    PixelFill(MainPic,x - 1, y, BolderColor, color);
                    PixelFill(MainPic,x + 1, y, BolderColor, color);
                    PixelFill(MainPic,x, y - 1, BolderColor, color);
                    PixelFill(MainPic,x, y + 1, BolderColor, color);
                }
            }
        }
        public static int LineFill(PictureMass MainPic, int x, int y, int dir, int PrevXl, int PrevXr, double BolderColor, double color)
        {
            int xl = x, xr = x; double c;
            if (x >= 0 && y >= 0 && x < MainPic.Width - 1 && y < MainPic.Height)
            {

                // Find line segment
                do if (xl > 0) c = MainPic.Array[--xl, y]; else break; while ((c != BolderColor) && (c != color));
                do if (xr < MainPic.Width - 1) c = MainPic.Array[++xr, y]; else break; while ((c != BolderColor) && (c != color));
                xl++; xr--;
                for (x = xl; x <= xr; x++)
                    MainPic.Array[x, y] = color; // fill segment
                // Fill adjacent segments in the same direction
                for (x = xl; x <= xr; x++)
                {
                    if (y + dir < MainPic.Height && y + dir > -1)
                    {
                        c = MainPic.Array[x, y + dir];
                        if ((c != BolderColor) && (c != color)) x = LineFill(MainPic,x, y + dir, dir, xl, xr, BolderColor, color);
                    }
                }
                for (x = xl; x < PrevXl; x++)
                {
                    if (y - dir > -1 && y - dir < MainPic.Height)
                    {
                        c = MainPic.Array[x, y - dir];
                        if ((c != BolderColor) && (c != color)) x = LineFill(MainPic,x, y - dir, -dir, xl, xr, BolderColor, color);
                    }
                }
                for (x = PrevXr; x < xr; x++)
                {
                    if (y - dir > -1 && y - dir < MainPic.Height)
                    {
                        c = MainPic.Array[x, y - dir];
                        if ((c != BolderColor) && (c != color)) x = LineFill(MainPic,x, y - dir, -dir, xl, xr, BolderColor, color);
                    }
                }
            }
            return xr;

        }
        public static void WhileFill(PictureMass MainPic, int x, int y, double BolderColor, double color)
        {
            Stack<Point> st = new Stack<Point>();
            Point start = new Point(x, y);
            st.Push(start);

            while (st.Count != 0)
            {
                Point temp = st.Pop();
                int Y1 = temp.Y;
                while (Y1 >= 1 && MainPic.Array[temp.X, Y1] != BolderColor) Y1--;
                Y1++;
                Boolean yl = false, yr = false;

                while (Y1 < MainPic.Height && MainPic.Array[temp.X, Y1] != BolderColor)
                {
                    MainPic.Array[temp.X, Y1] = color;

                    if (yl == false && temp.X > 0 && MainPic.Array[temp.X - 1, Y1] != BolderColor && MainPic.Array[temp.X - 1, Y1] != color)
                    {
                        Point NewPoint = new Point(temp.X - 1, Y1); st.Push(NewPoint); yl = true;
                    }
                    else if (yl == true && temp.X > 0 && (MainPic.Array[temp.X - 1, Y1] == BolderColor || MainPic.Array[temp.X - 1, Y1] == color))
                        yl = false;

                    if (yr == false && temp.X < MainPic.Width - 1 && MainPic.Array[temp.X + 1, Y1] != BolderColor && MainPic.Array[temp.X + 1, Y1] != color)
                    {
                        Point NewPoint = new Point(temp.X + 1, Y1); st.Push(NewPoint); yr = true;
                    }
                    else if (yr == true && temp.X > 0 && (MainPic.Array[temp.X + 1, Y1] == BolderColor || MainPic.Array[temp.X + 1, Y1] == color))
                        yr = false;
                    Y1++;
                }

            }
        }

        public static void WhileEdgeFill(PictureMass MainPic, int x, int y, double Edge)
        {
            Stack<Point> st = new Stack<Point>();
            Point start = new Point(x, y);
            st.Push(start);
            bool[,] temparr = new bool[MainPic.Width, MainPic.Height];

            double edgeItem = 0;
            while (st.Count != 0)
            {
                Point temp = st.Pop();
                int Y1 = temp.Y;
                edgeItem = 0;
                while (Y1 >= 1 && Math.Abs(edgeItem) <= Edge) 
                {
                    double temoXY = MainPic.Array[temp.X, Y1]; 
                    Y1--; 
                    edgeItem =  MainPic.Array[temp.X, Y1] -temoXY ;
                }
                Y1++;
                Boolean yl = false, yr = false;

                double edgeItem2 = 0;
                while (Y1 < MainPic.Height && edgeItem2 <= Edge)
                {
                    double temoXY = MainPic.Array[temp.X, Y1];
                    
                    temparr[temp.X, Y1] = true;

                    if (yl == false && temp.X > 0 && (Math.Abs(temoXY - MainPic.Array[temp.X - 1, Y1])) <= Edge && temparr[temp.X - 1, Y1] != true)
                    {
                        Point NewPoint = new Point(temp.X - 1, Y1); st.Push(NewPoint); yl = true;
                    }
                    else if (yl == true && temp.X > 0 && ((Math.Abs(temoXY - MainPic.Array[temp.X - 1, Y1])) > Edge || temparr[temp.X - 1, Y1] == true))
                        yl = false;

                    if (yr == false && temp.X < MainPic.Width - 1 && (Math.Abs(temoXY - MainPic.Array[temp.X + 1, Y1])) <= Edge && temparr[temp.X + 1, Y1] != true)
                    {
                        Point NewPoint = new Point(temp.X + 1, Y1); st.Push(NewPoint); yr = true;
                    }
                    else if (yr == true && temp.X > 0 && ((Math.Abs(temoXY - MainPic.Array[temp.X + 1, Y1])) > Edge || temparr[temp.X + 1, Y1] == true))
                        yr = false;
                    Y1++;
                    if (Y1 < MainPic.Height)
                    edgeItem2 = Math.Abs(temoXY - MainPic.Array[temp.X, Y1]);
                }

            }
            for (int xs = 0; xs < MainPic.Width; xs++)
                for (int ys = 0; ys < MainPic.Height; ys++)
                {
                    if (temparr[xs, ys] == true)
                        MainPic.Array[xs, ys] += edgeItem;
                }

        }

        public static Point WhileLineFill(double[,] BAr, byte[,] edge, int x, int y)
        {

            Stack<Point> st = new Stack<Point>();
            Point start = new Point(x, y), End = new Point(x, y);
            st.Push(start);
            double[,] tempBar = BAr; double Clr = BAr[x, y];
            byte[,] rez = edge;
            while (st.Count != 0)
            {
                Point temp = st.Pop();
                if (edge[temp.X, temp.Y] != 2)
                {
                    if (Clr == 1) BAr[temp.X, temp.Y] = 10000;
                    rez[temp.X, temp.Y] = 2;
                    for (int xl = temp.X - 1; xl <= temp.X + 1; xl++)
                        for (int yl = temp.Y - 1; yl <= temp.Y + 1; yl++)
                        {
                            if (xl < 0 || xl >= edge.GetLength(0) || yl < 0 || yl >= edge.GetLength(1)) continue;
                            if (Clr == 1)
                            {
                                if (BAr[xl, yl] == 1) { Point NewPoint = new Point(xl, yl); st.Push(NewPoint); }
                                if (BAr[xl, yl] == 0)
                                {
                                    rez[xl, yl] = 1;
                                    End.X = xl; End.Y = yl;
                                }
                            }
                            else
                            {
                                if (rez[xl, yl] == 1) { Point NewPoint = new Point(xl, yl); st.Push(NewPoint); }

                                if (BAr[xl, yl] == 1)
                                {
                                    Point fad = Blijh(BAr, xl, yl, 10000, 20);
                                    WhileLineFill(BAr, rez, xl, y);
                                    Bresenham8Line(BAr, 10000, xl, yl, fad.X, fad.Y);
                                }

                                if (rez[xl, yl] == 0)
                                {
                                    rez[xl, yl] = 1;
                                    End.X = xl; End.Y = yl;
                                }
                            }
                        }
                }
            }
            BAr = tempBar;
            edge = rez;
            return End;
        }
        public static double[,] LinePix(double[,] BAr, double[,] Main, int x, int y)
        {
            Stack<Point> st = new Stack<Point>();
            Point start = new Point(x, y);
            st.Push(start);
            double[,] tempBar = BAr, tempBar2 = BAr;
            double Clr = BAr[x, y];
            int Num = 0, x2 = 0, y2 = 0;
            while (st.Count != 0)
            {
                Point temp = st.Pop();

                Point temp2 = Blijh(Main, temp.X, temp.Y, 10000, 300);
                tempBar2[temp.X, temp.Y] = 0;
                tempBar2[temp2.X, temp2.Y] = 10000;
                if (Num > 0) Bresenham8Line(tempBar2, 10000, x2, y2, temp2.X, temp2.Y);
                x2 = temp2.X; y2 = temp2.Y;
                for (int xl = temp.X - 1; xl <= temp.X + 1; xl++)
                    for (int yl = temp.Y - 1; yl <= temp.Y + 1; yl++)
                    {
                        if (xl < 0 || xl >= BAr.GetLength(0) || yl < 0 || yl >= BAr.GetLength(1)) continue;
                        if (tempBar[xl, yl] == Clr) { Point NewPoint = new Point(xl, yl); st.Push(NewPoint); Num++; }
                    }
            }
            return tempBar2;

        }
        public static int SmLine(double[,] InArray, int x1, int y1, int x2, int y2)
        {
            int width = Math.Abs(x1 - x2) + 1, height = Math.Abs(y1 - y2) + 1;
            double[,] SubArr = new double[width, height];
            double[,] SubArr1 = new double[width, height];
            int count = 0, xpred = 0, ypred = 0; double sum = 0;
            Boolean fl = false;

            int Xl, Xr, Yt, Yb, sub_x1, sub_x2, sub_y1, sub_y2;
            if (x1 < x2) { Xl = x1; Xr = x2; sub_x1 = 0; sub_x2 = width - 1; } else { Xl = x2; Xr = x1; sub_x2 = 0; sub_x1 = width - 1; }
            if (y1 < y2) { Yt = y1; Yb = y2; sub_y1 = 0; sub_y2 = height - 1; } else { Yt = y2; Yb = y1; sub_y2 = 0; sub_y1 = height - 1; }
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (x + Xl < 0 || y + Yt < 0 || x + Xl >= InArray.GetLength(0) || y + Yt >= InArray.GetLength(1)) continue;
                    SubArr[x, y] = InArray[x + Xl, y + Yt];
                    SubArr1[x, y] = InArray[x + Xl, y + Yt];
                }

            Bresenham8Line(SubArr1, 10000, sub_x1, sub_y1, sub_x2, sub_y2);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (SubArr1[x, y] == 10000)
                    {
                        if (fl)
                        {
                            double fag = SubArr[x, y] - SubArr[xpred, ypred];
                            if (fag < 0) fag += 255;
                            sum += fag;
                            count++;
                        }
                        else
                            fl = true;
                        xpred = x;
                        ypred = y;
                    }
            return (int)(sum / count);
        }

        static public void Bresenham8Line(double[,] InArray, int clr, int x0, int y0, int x1, int y1)
        {
            //Изменения координат
            int dx = (x1 > x0) ? (x1 - x0) : (x0 - x1);
            int dy = (y1 > y0) ? (y1 - y0) : (y0 - y1);
            //Направление приращения
            int sx = (x1 >= x0) ? (1) : (-1);
            int sy = (y1 >= y0) ? (1) : (-1);

            if (dy < dx)
            {
                int d = (dy << 1) - dx;
                int d1 = dy << 1;
                int d2 = (dy - dx) << 1;
                int x = x0 + sx;
                int y = y0;
                for (int i = 1; i <= dx; i++)
                {
                    if (d > 0)
                    {
                        d += d2;
                        y += sy;
                    }
                    else
                        d += d1;
                    InArray[x, y] = clr;
                    x += sx;
                }
            }
            else
            {
                int d = (dx << 1) - dy;
                int d1 = dx << 1;
                int d2 = (dx - dy) << 1;
                int x = x0;
                int y = y0 + sy;
                for (int i = 1; i <= dy; i++)
                {
                    if (d > 0)
                    {
                        d += d2;
                        x += sx;
                    }
                    else
                        d += d1;
                    InArray[x, y] = clr;
                    y += sy;
                }
            }
        }
        public double[,] RecLine(double[,] InArray, int clr, int rad)
        {
            int width = InArray.GetLength(0),
                height = InArray.GetLength(1);
            double[,] OutArray = InArray;
            for (int x = rad; x < width - rad; x++)
                for (int y = rad; y < height - rad; y++)
                    if (InArray[x, y] == clr)
                    {
                        for (int x2 = x - rad; x2 < x + rad; x2++)
                            for (int y2 = y - rad; y2 < +rad; y2++)
                                if (InArray[x2, y2] == clr && x != x2 && y != y2)
                                    Bresenham8Line(OutArray, clr, x, y, x2, y2);
                        /*for (int x2 = x; x2 < x ; x2++)
                            for (int y2 = y - rad; y2 < y + rad; y2++)
                                if (InArray[x2, y2] == clr && x != x2 && y != y2)
                                    Bresenham8Line(OutArray, clr, x, y, x2, y2);*/
                    }
            return OutArray;
        }

        
    }
}
