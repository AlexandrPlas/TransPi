using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
//using System.Numerics;
using ClassLibrary;

namespace TransPi
{
    class Unrup
    {
        private static int M1 ;
        private static int M2 ;
        private static int N1 ;
        private static int N2 ;

        public class ZIntDescriptor   // Массив с отрицательными индексами
        {
            public int[] array;
            public int ind1;
            public int ind2;

            public ZIntDescriptor()
            {

            }

            public ZIntDescriptor(int i1, int i2)
            {
                ind1 = i1;
                ind2 = i2;
                array = new int[i1 + i2 + 1];                            // [-i1, i2] Всего элементов -2 -1 0 1 2 3 -  i1 + i2 + 1
                for (int j = 0; j < i1 + i2 + 1; j++) array[j] = -1;     // адресация от  -i1 до i2
            }

            public int GetValue(int i1)                                // Взять из массива 
            {
                //int i=i1+ind1;
                //if ( (i1 >= (-ind1 + 1)) && (i1 < ind2+1)) return (array[i1 + ind1]);
                if ((i1 >= -ind1) && (i1 <= ind2)) return (array[i1 + ind1]);
                else
                {
                    MessageBox.Show("Get Индексы не в диапазоне i1 = " + i1 + " ind1 = " + (-ind1) + " ind2 = " + (ind2));
                    return (0);
                }
            }

            public void SetValue(int i1, int a)                     // Поместить в массив
            {
                //int i=i1+ind1;
                //if ((i1 >= (-ind1 + 1)) && (i1 < ind2+1)) array[i1 + ind1] = a;
                if ((i1 >= -ind1) && (i1 <= ind2)) array[i1 + ind1] = a;
                else
                {
                    MessageBox.Show("Set Индексы не в диапазоне i1 = " + i1 + " ind1 = " + (-ind1) + " ind2 = " + (ind2));
                }
            }

            //---------------------------------------------------------------

        }




        /*      
        Назначение: Нахождение наибольшего общего делителя двух чисел N и M по рекуррентному соотношению
        N0 = max(|N|, |M|) N1 = min(|N|, |M|)
        N k = N k-2 - INT(N k-2 / N k-1)*N                   k-1 k=2,3 ...
       
        Если Nk = 0 => НОД = N k-1
        (N=23345 M=9135 => 1015 N=238 M=347 => 34)
        */
        private static Int32 Evklid(int N1, int N2)
        {
            int n0 = Math.Max(N1, N2);
            int n1 = Math.Min(N1, N2);

            do { Int32 n = n0 - (int)((n0 / n1) * n1); n0 = n1; n1 = n; } while (n1 != 0);

            return n0;
        }

        private static void China(int n1, int n2)
        {
            int n;
          
            int NOD = Evklid(n1, n2);   // Если NOD == 1 числа взаимно просты
            //if (NOD != 1) { MessageBox.Show("Числа не взаимно просты"); return; }
            while (NOD != 1) { n1 = n1 / NOD; n2 = n2 / NOD; NOD = Evklid(n1, n2); }

             M1 = n2;
             M2 = n1;
             N1 = 0;
             N2 = 0;
            for (int i = 0; i < n1; i++) { n = (M1 * i) % n1; if (n == 1) { N1 = i; break; } }
            for (int i = 0; i < n2; i++) { n = (M2 * i) % n2; if (n == 1) { N2 = i; break; } }
        }

        //  Рисование таблицы чисел по двум массивам k1 и k2 с сдвигом

        public static void Tabl_int(PictureMass Pic1, PictureMass Pic2, PictureBox pictureBox1, int N1_sin, int N2_sin, int scale, int d, int sdvig)
        {
            China(N1_sin, N2_sin);
            //MessageBox.Show(" M1 " + M1 + " N1 " + N1 + " M2 " + M2 + " N2 " + N2);

            if (Pic1 == null) { MessageBox.Show("Изображение Pic1 отсутствует "); return; }
            if (Pic2 == null) { MessageBox.Show("Изображение Pic2 отсутствует "); return; }

            int nx = Pic1.Width;
            int ny = Pic1.Height;

            //ZArrayDescriptor res = new ZArrayDescriptor(N1_sin, N2_sin);
            double max1 = PictureMass.GetMax(Pic1.Array);
            double min1 = PictureMass.GetMin(Pic1.Array);
            double max2 = PictureMass.GetMax(Pic2.Array);
            double min2 = PictureMass.GetMin(Pic2.Array); 
           // China(N1_sin, N2_sin);
            Graphics g = Graphics.FromHwnd(pictureBox1.Handle);
            g.Clear(Color.White);

            ZArrayDescriptor arr = new ZArrayDescriptor(N2_sin, N1_sin);
      
            int M = M1*N1;
            int N = M2*N2;
            int MN = N1_sin * N2_sin;

            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++)
                {
                    //double a = res.array[i, j];
                    int x = (int)((Pic1.Array[i, j] - min1) * (N2_sin-1) / (max1 - min1));
                    int b2 = sdv(x, sdvig, N2_sin); 
                    int y = (int)((Pic2.Array[i, j] - min1) * (N1_sin-1) / (max1 - min1));
                    int X0 = (M * b2 + N * y) % MN;
                    //if (X0 < d ) Point(b2 * scale, y * scale, g);
                    if (X0 < d) arr.array[b2, y] += 1;
                }
            }


        }

        public static int sdv(int k, int sdvig, int N)
        {
            k = k + sdvig; 
            if (k > 0) k = k % N;
            if (k < 0) k = N + k;
                
            return k;
        }

        public static void Point_N(PictureMass arr, int N1_sin, int N2_sin, int scale, Graphics g)
        {
            double max = PictureMass.GetMax(arr.Array);
            double c4= max/4;
            double c2 = max / 2;
            double c3 = c4 + c2;
            for (int i = 0; i < N2_sin; i++)
            {
                for (int j = 0; j < N1_sin; j++)
                   {
                      int x = i * scale;
                      int y = j * scale;
                      if (arr.Array[i,j] == 0 ) continue;
                      if (arr.Array[i, j] < c4) { g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0)), x, y, 1, 1); continue; }
                      if (arr.Array[i, j] < c2) { g.FillRectangle(new SolidBrush(Color.FromArgb(0, 250, 0)), x, y, 1, 1); continue; }
                      if (arr.Array[i, j] < c3) { g.FillRectangle(new SolidBrush(Color.FromArgb(250, 150, 0)), x, y, 1, 1); continue; }
                   }

                   
            }
        }

        //  Рисование таблицы диагоналей 

        public static void Point(int x, int y, Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.Black), x, y, 1, 1);
        }
        // ------------------------------------------------------------------------------------------------------------
        //     ------------------------     Развертка
        //-------------------------------------------------------------------------------------------------------------
        public static PictureMass Unrup_array(PictureMass Pic1, PictureMass Pic2, int N1_sin, int N2_sin, int N_diag, int sdvig, int Mngtl)
        {
            if (Pic1 == null) { MessageBox.Show("Изображение Pic1 отсутствует "); return null; }
            if (Pic2 == null) { MessageBox.Show("Изображение Pic2 отсутствует "); return null; }

            int NX = Pic1.Width;
            int NY = Pic1.Height;

            PictureMass res = new PictureMass();
            res.SetSize(NX, NY);
            ZIntDescriptor a = new ZIntDescriptor(N1_sin, N2_sin);  // Для считывания отрицательных значений

           a = Diag(a, N1_sin, N2_sin, N_diag );                   // Формирование массива для развертки с -1
           a = Diag_array(a);

           ZIntDescriptor a1 = new ZIntDescriptor(N1_sin * Mngtl, N2_sin * Mngtl);   // класс для считывания с отрицательными индексами  
           a1 = Diag_array_Mngtl(a, Mngtl); 

            //a.SetValue(-N1_sin, 14);
           //double max1 = PictureMass.GetMax(Pic1.Array);
           //double min1 = PictureMass.GetMin(Pic1.Array); max1 = max1 - min1;
           //double max2 = PictureMass.GetMax(Pic2.Array);
           //double min2 = PictureMass.GetMin(Pic2.Array); max2 = max2 - min2;
           double max1 = 2 * Math.PI;
           double min1 = -Math.PI;
           double max2 = 2 * Math.PI;
           double min2 = -Math.PI;


            China(N1_sin, N2_sin);
            int M = M1 * N1;
            int N = M2 * N2;
            int MN = N1_sin * N2_sin;
            int d1 = N_diag * N1_sin;
                    

           for (int i = 0; i < NX; i++)
               for (int j = 0; j < NY; j++)
               {
                   double b2x = (Pic1.Array[i, j] - min1) * (N2_sin - 1) * Mngtl / max1;
                   int b2 = (int)b2x;
                   b2 = sdv(b2, sdvig, N2_sin * Mngtl);
                   double b1x = (Pic2.Array[i, j] - min2) * (N1_sin - 1) * Mngtl / max2;
                   int b1 = (int)b1x;
                   //double b2x = (Pic1.Array[i, j] - min1) * (N2_sin - 1) / max1;
                   //int b2 = (int)b2x;
                   //int b2 = (int)Pic1.Array[i, j];
                   //b2 = sdv(b2, sdvig, N2_sin);
                   //double b1x = (Pic2.Array[i, j] - min2) * (N1_sin - 1) / max2;
                   //double b1x = Pic2.Array[i, j];
                   //int b1 = (int)b1x;
                  
                   //int b1 = (int)Pic2.Array[i, j];
                   //b1 = sdv(b1, 1, N1_sin);
                   int X = a1.GetValue(b2 - b1 );
                   res.Array[i, j] = X * N1_sin * Mngtl + b1x;

                   /*int x = (int)((Pic1.Array[i, j] - min1) * (N2_sin - 1) / (max1 - min1));
                   int y = (int)((Pic2.Array[i, j] - min2) * (N1_sin - 1) / (max2 - min2));
                   x = sdv(x, sdvig, N2_sin); 
                   y = y + 1;
                   int X0 = (M * x + N * y) % MN;
                   if (X0 < d1) res.Array[i, j] = X0;*/
               }

           return res;
        }

        public static ZIntDescriptor Diag(ZIntDescriptor a, int N1_sin, int N2_sin, int N_diag)
        {
            int M = M1 * N1;
            int N = M2 * N2;
            int MN = N1_sin * N2_sin;

            China(N1_sin, N2_sin);
            for (int i = 0; i < N2_sin; i++)   // Строка
            {
                int b = ((M2 * N2 * i) % MN) / N1_sin;
                if (b < N_diag) a.SetValue(i, b); else a.SetValue(i, -1);
            }
            int i1 = N1_sin;
            for (int j = -N1_sin + 1; j < 0; j++)  // Столбец
            {
                int b = ((M1 * N1 * (-j)) % MN) / N1_sin;
                if (b < N_diag) a.SetValue(j, b); else a.SetValue(j, -1);
                i1--;
            }
            return a;
        }

       public static ZIntDescriptor Diag_array(ZIntDescriptor a)  // Заполнение -1 ближайшей не равной 0 величиной
       {
           int ind1 = a.ind1;
           int ind2 = a.ind2;
           ZIntDescriptor b = new ZIntDescriptor(ind1, ind2);
           //MessageBox.Show(" ind1 -- " + ind1 + " ind2 -- " + ind2);
           for (int j = -ind1+1; j < ind2; j++)
           {
              //a.SetValue(j, -2);
              //MessageBox.Show(" j= " + j + " a.GetValue(j) = " + a.GetValue(j)); 
               if ( a.GetValue(j) < 0)
               {
                  par par1 = Left(a,j);
                  par par2 = Right(a, j);
                  int bmin = a.GetValue(par1.i);                // Индекс левого элемента
                  int bmax = a.GetValue(par2.i);                // Индекс правого элемента
                  //MessageBox.Show(" j= " + j + " min_left= " + par1.s + " min_right= " + par2.s + " il= " + bmin + " ir= " + bmax);
                  if (par1.s < par2.s) b.SetValue(j, bmin); else b.SetValue(j, bmax);
                    
               }
               else b.SetValue(j, a.GetValue(j));

           }
           return b;
       }

       public static par Left(ZIntDescriptor a, int j)
         {
           int s=0;
           int ind1 = a.ind1;
           int ind2 = a.ind2;
           int b=j;
           int i1=0;
           int cntr = 0;
           for (int i = j; i > -ind1; i--, s++) { b = i; if (a.GetValue(i) >= 0) { cntr = 1; i1 = i; break; } }
           
           if (cntr != 1) 
                  { for (int i = ind2 - 1; i > -ind1; i--,s++) {  if (a.GetValue(i) >= 0) { i1 = i; break; } }  }

           par c;
           c.s = s;
           c.i = i1;
           return c;
         }

       public static par Right(ZIntDescriptor a, int j)
         {
           int s=0;
           int ind1 = a.ind1;
           int ind2 = a.ind2;
           int b=j;
           int i1=0;
           int cntr = 0;
           for (int i = j; i < ind2; i++, s++) { b = i; if (a.GetValue(i) >= 0) { cntr = 1; i1 = i; break; } }
           if (cntr != 1 )
              {  for (int i = -ind1+1; i < ind2; i++, s++) { if (a.GetValue(i) >= 0) { i1 = i; break; } } }

           par c;
           c.s = s;
           c.i = i1;
           return c;
         }
        
       public struct par
        {
            public int s;  // Расстояние
            public int i;  // Индекс
        }


       public static ZIntDescriptor Diag_array_Mngtl(ZIntDescriptor a, int Mngtl)
       {
           if (Mngtl == 1) return a;

           int n1 = a.ind1 * Mngtl;
           int n2 = a.ind2 * Mngtl;
           ZIntDescriptor b = new ZIntDescriptor(n1, n2);
           int i1 = a.ind1;
           for (int i = -n1; i < 0; i += Mngtl)
           {
               i1 = i / Mngtl;
               //if (i1>a.ind2) MessageBox.Show("i " + i + " i1 " + i1);
               int c = a.GetValue(i1);

               for (int j = 0; j < Mngtl; j++) b.SetValue(i + j, c);
           }

           for (int i = 0; i <= n2 - Mngtl; i += Mngtl)
           {
               i1 = i / Mngtl;
               //if (i1>a.ind2) MessageBox.Show("i " + i + " i1 " + i1);
               int c = a.GetValue(i1);

               for (int j = 0; j < Mngtl; j++) b.SetValue(i + j, c);
           }

           return b;
       }

 //------------------------------------------------------------------------------------

    }
}
//               Pen myPen = new Pen(Color.Black, 1);
//               Graphics g = Graphics.FromHwnd(pictureBox1.Handle);
//               g.DrawRectangle(myPen, 10, 10, 50, 50);
//               Point(100, 100, g);