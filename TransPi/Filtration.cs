using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransPi
{
    class Filtration
    {

        // медианная фильтрация
        private static  double filt_median(int k, double[] Mass)
        {

            int  i_min1, i_min2,  k2 = k / 2;
            double s,  min = Mass[0];
            for (int i = 0; i < k; i++)
            {
                min = Mass[i]; i_min1 = i; i_min2 = i;
                for (int j = i; j < k; j++) if (Mass[j] < min) { min = Mass[j]; i_min2 = j; }
                if (i_min1 != i_min2) { s = Mass[i_min1]; Mass[i_min1] = Mass[i_min2]; Mass[i_min2] = s; }
            }
            if (k % 2 != 0) s = Mass[k / 2]; else s = (Mass[k2] + Mass[k2 - 1]) / 2;
            return s;
        }

        public static double[,] Filt_Mediana(double[,] InArray, int k_filt)
        {
            double s;
            int k = k_filt;
            int k2 = k / 2;

            int w1 = InArray.GetLength(0);
            int h1 = InArray.GetLength(1);

            int max = w1; if (h1 > max) max = h1;
            double[] k_x1 = new double[max];
            double[] k_x2 = new double[max];

            int all = (w1 + h1) * 2;

            double[,] result = new double[w1, h1];
            double[] f_x = new double[k];

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    k_x1[j] = InArray[i, j];
                }

                for (int j = 0; j < h1 - k; j++)
                {
                    for (int m = 0; m < k; m++) { f_x[m] = k_x1[j + m]; }
                    k_x2[j] = filt_median(k, f_x);
                }

                for (int j = 0; j < h1 - k; j++)
                {
                    s = k_x2[j];
                    InArray[i, j + k2] = s;
                }

            }

            for (int j = 0; j < h1; j++)
            {
                for (int i = 0; i < w1; i++)
                {
                    k_x1[i] = InArray[i, j];
                }

                for (int i = 0; i < w1 - k; i++)
                {
                    for (int m = 0; m < k; m++) { f_x[m] = k_x1[i + m]; }
                    k_x2[i] = filt_median(k, f_x);
                }

                for (int i = 0; i < w1 - k; i++)
                {
                    s = k_x2[i];
                    result[i + k2, j] = s;
                }

            }

            return result;
        }

        //Пороги

        public static double[,] Porogi(double[,] InArray, int porog)
        {
            int w1 = InArray.GetLength(0);
            int h1 = InArray.GetLength(1);
            double[,] result = new double[w1, h1];

            for (int i = 0; i < w1; i++)
                for (int j = 0; j < h1; j++)
                    if (InArray[i,j]>= porog) result[i,j]=255;
                    else result[i,j]=0;
            return result;
            
        }

        // Фильтрация 121
        public static int[,] Filt_121(int[,] InArray, int k_filt)
        {
            int r1;
            int k = k_filt;
            int k_cntr;

            int w1 = InArray.GetLength(0);
            int h1 = InArray.GetLength(1);

            int max = w1; if (h1 > max) max = h1;

            int[] k_x = new int[max];
            int[] k_x1 = new int[max];

            int all = w1 + h1;
            int done = 0;
            

            int[,] someBuffer = new int[w1, h1];

            for (int i = 0; i < w1; i++)
            {
                for (int j = 0; j < h1; j++)
                {
                    k_x[j] = InArray[i,j];
                }

                k_cntr = 1;

                for (int ik = k; ik > 0; ik /= 2)
                {
                    if (k_cntr == 1) { for (int j = ik; j < h1 - ik; j++)   k_x1[j] = (k_x[j - ik] + (k_x[j] << 1) + k_x[j + ik]) >> 2; k_cntr = 2; }
                    else { for (int j = ik; j < h1 - ik; j++)   k_x[j] = (k_x1[j - ik] + (k_x1[j] << 1) + k_x1[j + ik]) >> 2; k_cntr = 1; }
                }

                for (int j = 0; j < h1; j++)
                {
                    if (k_cntr == 1) r1 = k_x[j]; else r1 = k_x1[j];
                    someBuffer[i,j] = r1;
                }

                done++;
                
            }

            int[,] result = new int[w1, h1];

            for (int j = 0; j < h1; j++)
            {
                for (int i = 0; i < w1; i++)
                {
                    k_x[i] = someBuffer[i,j];
                }

                k_cntr = 1;

                for (int ik = k; ik > 0; ik /= 2)
                {
                    if (k_cntr == 1) { for (int i = ik; i < w1 - ik; i++)   k_x1[i] = (k_x[i - ik] + (k_x[i] << 1) + k_x[i + ik]) >> 2; k_cntr = 2; }
                    else { for (int i = ik; i < w1 - ik; i++)   k_x[i] = (k_x1[i - ik] + (k_x1[i] << 1) + k_x1[i + ik]) >> 2; k_cntr = 1; }
                }

                for (int i = 0; i < w1; i++)
                {
                    if (k_cntr == 1) r1 = k_x[i]; else r1 = k_x1[i];
                    result[i,j] = r1;
                }

                done++;
                
            }

            return result;
        }
        //сглаживание
        public static double[,] Filt_smothingSM(double[,] amp, int k_filt)   // Сглаживание
        {

            int k = k_filt;
            int k_cntr = 1;

            int w1 = amp.GetLength(0);
            int h1 = amp.GetLength(1);
            
            double[,] res_array = new double[w1, h1];
            double[] f_x = new double[k];

            int max = w1; if (h1 > max) max = h1;

            double[] k_x = new double[max];
            double[] k_x1 = new double[max];
            double r1 = 0;

            for (int i = 0; i < w1; i++)    // По строкам
            {
                for (int j = 0; j < h1; j++) { k_x[j] = amp[i, j]; }
                k_cntr = 1;
                for (int ik = k; ik > 0; ik /= 2)
                {
                    if (k_cntr == 1)
                    { for (int j = ik; j < h1 - ik; j++)   k_x1[j] = (k_x[j - ik] + k_x[j] * 2 + k_x[j + ik]) / 4; k_cntr = 2; }
                    else
                    { for (int j = ik; j < h1 - ik; j++)   k_x[j] = (k_x1[j - ik] + k_x1[j] * 2 + k_x1[j + ik]) / 4; k_cntr = 1; }
                }
                for (int j = 0; j < h1; j++)
                {
                    if (k_cntr == 1) r1 = k_x[j]; else r1 = k_x1[j];
                    res_array[i, j] = r1;
                }

            }

            for (int j = 0; j < h1; j++)   // По столбцам
            {
                for (int i = 0; i < w1; i++) { k_x[i] = res_array[i, j]; }
                k_cntr = 1;
                for (int ik = k; ik > 0; ik /= 2)
                {
                    if (k_cntr == 1) { for (int i = ik; i < w1 - ik; i++)   k_x1[i] = (k_x[i - ik] + k_x[i] * 2 + k_x[i + ik]) / 4; k_cntr = 2; }
                    else { for (int i = ik; i < w1 - ik; i++)               k_x[i] = (k_x1[i - ik] + k_x1[i] * 2 + k_x1[i + ik]) / 4; k_cntr = 1; }
                }
                for (int i = 0; i < w1; i++)
                {
                    if (k_cntr == 1) r1 = k_x[i]; else r1 = k_x1[i];
                    res_array[i, j] = r1; ;
                }
            }
            //  for (int j = 0; j < h1; j++)
            //      for (int i = 0; i < w1; i++)
            //      { res_array.array[i, j] = amp.array[i, j] - res_array.array[i, j]; }


            return res_array;
        }

        public static double[] Filt_smothingSM1(double[] amp, int k_filt)   // Сглаживание одномерное
        {

            int k = k_filt;
            int k_cntr = 1;

            int w1 = amp.GetLength(0);

            double[] res_array = new double[w1];
            double[] f_x = new double[k];

            int max = w1; 

            double[] k_x = new double[max];
            double[] k_x1 = new double[max];
            double r1 = 0;



            for (int i = 0; i < w1; i++) { k_x[i] = amp[i]; }
            k_cntr = 1;
            for (int ik = k; ik > 0; ik /= 2)
            {
                if (k_cntr == 1) { for (int i = ik; i < w1 - ik; i++)   k_x1[i] = (k_x[i - ik] + k_x[i] * 2 + k_x[i + ik]) / 4; k_cntr = 2; }
                else { for (int i = ik; i < w1 - ik; i++)               k_x[i] = (k_x1[i - ik] + k_x1[i] * 2 + k_x1[i + ik]) / 4; k_cntr = 1; }
            }
            for (int i = 0; i < w1; i++)
            {
                if (k_cntr == 1) r1 = k_x[i]; else r1 = k_x1[i];
                res_array[i] = r1; ;
            }
            

            return res_array;
        }
    }
}
