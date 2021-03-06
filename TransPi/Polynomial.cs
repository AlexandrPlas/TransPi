﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransPi
{
    class Polynomial
    {
        /// <summary>
        /// Сетка по X
        /// </summary>
        double[] _x;

        /// <summary>
        /// Значения функции в точках _x
        /// </summary>
        double[] _y;

        /// <summary>
        /// Значения коэффициентов
        /// </summary>
        double[] _coefficients;

        public double[] Coefficients
        {
            get { return _coefficients; }
        }

        /// <summary>
        /// Степень полинома
        /// </summary>
        int _amount;

        public int Amount
        {
            get { return _amount; }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="x">Сетка по X</param>
        /// <param name="y">Значения функции в точках X</param>
        /// <param name="amount"></param>
        /// <remarks>Размер массива X должен равняться размеру массива Y. В масивах должно быть хотя бы по три элемента. Массив X должен быть упорядочен по возрастанию.</remarks>
        public Polynomial(double[] yval, int amount)
        {
            
            if (yval.Length < 3)
            {
                throw new ArgumentException();
            }

            // 1, 2
            _amount = amount;
            _x = new double[yval.Length];
            _y = yval;
            for (int i = 0; i<yval.Length; i++)
                _x[i] = i;

            int n = _amount + 1;
            int count = _x.Length;

            double[,] a = new double[n, n];
            double[] b = new double[n];
            double[] c = new double[2 * n];
            _coefficients = new double[n];

            // 3
            for (int i = 0; i < count; i++)
            {
                double x = _x[i];
                double y = _y[i];

                double f = 1;

                for (int j = 0; j < 2 * n - 1; j++)
                {
                    if (j < n)
                    {
                        b[j] += y;
                        y *= x;
                    }

                    c[j] += f;
                    f *= x;
                }
            }

            // 4
            for (int i = 0; i < n; i++)
            {
                int k = i;

                for (int j = 0; j < n; j++)
                {
                    a[i, j] = c[k];
                    k++;
                }
            }

            //5
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    a[j, i] /= -a[i, i];

                    for (int k = i + 1; k < n; k++)
                    {
                        a[j, k] += a[j, i] * a[i, k];
                    }

                    b[j] += a[j, i] * b[i];
                }
            }

            // 6
            _coefficients[n - 1] = b[n - 1] / a[n - 1, n - 1];

            //7
            for (int i = n - 2; i >= 0; i--)
            {
                double h = b[i];

                for (int j = i + 1; j < n; j++)
                {
                    h -= _coefficients[j] * a[i, j];
                }

                _coefficients[i] = h / a[i, i];
            }
        }


        #region IApproximation Members

        public double GetValue(double xpoint)
        {
            double s = 0;
            for (int i = _amount; i >= 1; i--)
            {
                s = (s + _coefficients[i]) * xpoint;
            }

            return s + _coefficients[0];
        }

        #endregion
    }
}
