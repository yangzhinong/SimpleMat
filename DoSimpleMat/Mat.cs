using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoSimpleMat
{
    class Mat<T> where T : struct, IComparable, IComparable<T>, IEquatable<T>
    {

        public T[,] Array { get; set; }


        public  Mat(T[,] array)
        {
            Array = array;
        }

        /// <summary>
        ///  矩阵相加
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Mat<T> operator +(Mat<T> a , Mat<T> b)
        {
            return new Mat<T>(MatAdd(a.Array, b.Array));
        }
        /// <summary>
        /// 矩阵相乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Mat<T> operator *(Mat<T> a, Mat<T> b)
        {
            return new Mat<T>(MatMult(a.Array, b.Array));
        }

        /// <summary>
        /// 矩阵数乘(几何解释为向量缩放)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public int[,] MatScale(T[,] a, T scale)
        {
            var r1 = GetRowCount(a); //行数
            var c1 = a.GetUpperBound(1); //列数
            var ret = new int[r1, c1];
            for (var i = 0; i <= r1; i++)
            {
                for (var j = 0; j <= c1; j++)
                {
                    ret[i, j] = (dynamic)scale * a[i, j];
                }
            }
            return ret;
        }

        /// <summary>
        /// 矩阵乘法(几何解释是从右往左,先b线性变换再a线性变换)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T[,]  MatMult(T[,] a, T[,] b)
        {
            if (a == null || b == null) throw new ArgumentNullException("参数不能为空");
            var r1 = GetRowCount(a); //行数
            var c1 = GetColCount(a); //列数
            var r2 = GetRowCount(b); //行数
            var c2 = GetColCount(b); //列数
            if (c1 != r2)
            {
                throw new ArgumentException("矩阵乘法,第1个数的列数必须等于第2个数的行数!");
            }
            var ret = new T[r1, c2];
            for (var r = 0; r < r1; r++)
            {
                for (var c = 0; c < c2; c++)
                {
                    T sum = default;
                    for (var s = 0; s < c1; s++)
                    {
                        sum += (dynamic)a[r, s] * b[s, c];
                    }
                    ret[r, c] = sum;
                }
            }
            return ret;
        }
        /// <summary>
        /// 矩阵相加, (满足加法交换律)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T[,] MatAdd(T[,] a, T[,] b)
        {
            var r1 = GetRowCount(a);
            var c1 = GetColCount(a);
            var r2 = GetRowCount(b);
            var c2 = GetRowCount(b);

            if (r1 != r2 || c1 != c2)
            {
                throw new ArgumentException("矩阵相加必须行列数一样");
            }
            T[,] ret = new T[r1, c1];
            for (var i = 0; i < r1; i++)
            {
                for (var j = 0; j < c1; j++)
                {
                    ret[i, j] = (dynamic)a[i, j] + b[i, j];
                }
            }

            return ret;
        }

        /// <summary>
        /// 获取矩阵行数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int GetRowCount(T[,] a)
        {
            return a.GetUpperBound(0) + 1;
        }
        /// <summary>
        /// 获取矩阵列数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int GetColCount(T[,] a)
        {
            return a.GetUpperBound(1) + 1;
        }

        /// <summary>
        /// 输出矩阵图形显示
        /// </summary>
        /// <param name="c"></param>
        /// <param name="name"></param>
        public static void Print(T[,] c, string name)
        {
            Console.WriteLine($"-------{name} Start------------");
            for (var i = 0; i < Mat<T>.GetRowCount(c); i++)
            {
                for (var j = 0; j < Mat<T>.GetColCount(c); j++)
                {
                    Console.Write($" {c[i, j]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"-------{name} End--------------");
        }

        public void Print(string name)
        {
            Print(Array, name);
        }

        
    }
}
