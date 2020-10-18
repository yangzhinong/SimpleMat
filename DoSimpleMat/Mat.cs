using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DoSimpleMat
{
    class Mat<K> where K : struct, IComparable, IComparable<K>, IEquatable<K>
    {

        public K[,] Array { get; set; }


        public  Mat(K[,] array)
        {
            Array = array;
        }

        public Mat(int rows, int cols)
        {
            Array = InitArray(rows,cols);
        }

        /// <summary>
        /// 创建单行矩阵,或单列矩阵;
        /// </summary>
        /// <param name="array">初始数组</param>
        /// <param name="colMode">True:单列矩阵, False单行矩阵</param>
        public Mat(K[] array, bool colMode)
        {
            if (colMode)
            {
                //列模式
                Array = InitArray(array.Length, 1);
                for(var i=0; i < array.Length; i++)
                {
                    Array[i, 0] = array[i];
                }
            } else
            {
                //行模式
                Array = InitArray(1, array.Length);
                for (var j = 0; j < array.Length; j++)
                {
                    Array[0, j] = array[j];
                }
            }
        }

        private void SetRowColArray(K[] array, int index, bool colMode)
        {
            
            if (colMode)
            {
                if (array.Length != GetRowCount()) throw new ArgumentException($"参数:{nameof(array)} 长度 必须等于 矩阵行数");
                for(var i=0; i < array.Length; i++)
                {
                    Array[i, index] = array[index];
                }
            } else
            {
                if (array.Length != GetColCount()) throw new ArgumentException($"参数:{nameof(array)} 长度 必须等于 矩阵列数");
                {
                    for (var j=0; j<array.Length; j++)
                    {
                        Array[index, j] = array[j];
                    }
                }
            }
        }

        /// <summary>
        /// 设置行数据
        /// </summary>
        /// <param name="array"></param>
        /// <param name="rowIndex"></param>
        public void SetRowArray(K[] array, int rowIndex)
        {
            SetRowColArray(array, rowIndex, colMode: false);
        }
        /// <summary>
        /// 设置列数据
        /// </summary>
        /// <param name="array"></param>
        /// <param name="colIndex"></param>
        public void SetColArray(K[] array, int colIndex)
        {
            SetRowColArray(array, colIndex, colMode: true);
        }

        /// <summary>
        /// 获取指定维度的新数组
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        private static K[,] InitArray(int rows, int cols)
        {
            K[,] a = new K[rows, cols];
            return a;
        }

        

        /// <summary>
        ///  矩阵相加
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Mat<K> operator +(Mat<K> a , Mat<K> b)
        {
            return new Mat<K>(MatAdd(a.Array, b.Array));
        }
        /// <summary>
        /// 矩阵相乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Mat<K> operator *(Mat<K> a, Mat<K> b)
        {
            return new Mat<K>(MatMult(a.Array, b.Array));
        }

        /// <summary>
        /// 矩阵数乘(几何解释为向量缩放)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public int[,] MatScale(K[,] a, K scale)
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
        public static K[,]  MatMult(K[,] a, K[,] b)
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
            var ret = new K[r1, c2];
            for (var r = 0; r < r1; r++)
            {
                for (var c = 0; c < c2; c++)
                {
                    K sum = default;
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
        public static K[,] MatAdd(K[,] a, K[,] b)
        {
            var r1 = GetRowCount(a);
            var c1 = GetColCount(a);
            var r2 = GetRowCount(b);
            var c2 = GetRowCount(b);

            if (r1 != r2 || c1 != c2)
            {
                throw new ArgumentException("矩阵相加必须行列数一样");
            }
            K[,] ret = new K[r1, c1];
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
        public static int GetRowCount(K[,] a)
        {
            return a.GetUpperBound(0) + 1;
        }

        public static int GetRowCount(Mat<K> A)
        {
            return GetRowCount(A.Array);
        }

        public int GetRowCount()
        {
            return GetRowCount(Array);
        }
        /// <summary>
        /// 获取矩阵列数
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int GetColCount(K[,] a)
        {
            return a.GetUpperBound(1) + 1;
        }

        public static int GetColCount(Mat<K> A)
        {
            return GetColCount(A.Array);
        }

        public int GetColCount()
        {
            return GetColCount(Array);
        }

        /// <summary>
        /// 输出矩阵图形显示
        /// </summary>
        /// <param name="c"></param>
        /// <param name="name"></param>
        public static void Print(K[,] c, string name)
        {
            Console.WriteLine($"-------{name} Start------------");
            for (var i = 0; i < Mat<K>.GetRowCount(c); i++)
            {
                for (var j = 0; j < Mat<K>.GetColCount(c); j++)
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

        /// <summary>
        /// 单位矩阵
        /// </summary>
        /// <typeparam name="VType"></typeparam>
        /// <param name="dimCount">维数</param>
        /// <returns></returns>
        public static Mat<K> I(int dimCount)
        {
            if (dimCount < 1) throw new ArgumentOutOfRangeException("维数必须大于0");
            K[,] a = new K[dimCount, dimCount];

            var c = GetColCount(a);
            var r = GetRowCount(a);

            for (var i=0; i<r; i++)
            {
                for(var j=0; j<c; j++)
                {
                    if (i == j)
                    {
                        a[i, j] = (dynamic) 1;
                    }
                }
            }
            return new Mat<K>(a);
        }

        /// <summary>
        /// 当前矩阵关连的单位矩阵
        /// </summary>
        /// <returns></returns>
        public Mat<K> I()
        {
            var c = GetColCount(this);
            var r = GetRowCount(this);

            if (c < 1 || r < 1) throw new ArgumentException("维数必须大于0");
            if (c != r) throw new ArgumentException("单位矩阵必须行数与列数相等");
            return I(c);
        }
        
        /// <summary>
        /// 得到列向量数组
        /// </summary>
        /// <param name="colIndex">列索引</param>
        /// <returns></returns>
        public K[] GetColArray(int colIndex)
        {
            if (colIndex> GetColCount()-1 || colIndex<0) 
                throw new ArgumentOutOfRangeException(nameof(colIndex));
            var rows = GetColCount(); 
            K[] ret = new K[rows];
            for (var i=0; i < rows; i++)
            {
                ret[i] = Array[i, colIndex];
            }
            return ret;
        }


        /// <summary>
        /// 得到行向量数组
        /// </summary>
        /// <param name="colIndex">列索引</param>
        /// <returns></returns>
        public K[] GetRowArray(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > GetRowCount() - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex));
            }
            var cols = GetColCount();
            K[] ret = new K[cols];
            for (var j=0; j<cols; j++)
            {
                ret[j] = Array[rowIndex, j];
            }
            return ret;
        }
        /// <summary>
        /// 得到列向量
        /// </summary>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public Mat<K> GetColVector(int colIndex)
        {
            var colArray = GetColArray(colIndex);

            var ret = new Mat<K>(1, colArray.Length);
            for(var i=0; i < colArray.Length; i++)
            {
                ret.Array[i, 0] = colArray[i];
            }
            return ret;
        }

        /// <summary>
        /// 逆矩阵
        /// </summary>
        /// <returns></returns>
        public Mat<double> Get_1()
        {
            var rows = GetRowCount();
            var cols = GetColCount();
            var ret = new Mat<double>(new double[rows, cols]);
            if (rows == cols || rows == 2)
            {
                //二维矩阵逆
                double scale = (dynamic)1.0d / ( ( dynamic) Array[0, 0] * Array[1, 1] - (dynamic) Array[0, 1] * Array[1, 0]);

                ret.Array[0, 0] = (dynamic)scale * Array[1, 1];
                ret.Array[0, 1] = - ((dynamic)scale * Array[0, 1]);
                ret.Array[1, 0] = -((dynamic)scale * Array[1, 0]);
                ret.Array[1, 1] = (dynamic)scale * Array[0, 0];
            }

            return ret;
        }
        /// <summary>
        /// 矩阵所示的行列式值
        /// </summary>
        /// <returns></returns>
        public K Value()
        {
            K ret = default;

            var rows = GetRowCount();
            var cols = GetColCount();
            if (rows != cols) throw new Exception("矩阵值只能求解行数与列数相等的行列式");
            if (rows == 1) ret = Array[0, 0];
            if (rows == 2)
            {
                ret = (dynamic)Array[0, 0] * Array[1, 1] - (dynamic) Array[0, 1] * Array[1, 0];
            }
            if (rows == 3)
            {
                ret = (dynamic)Array[0, 0] * Array[1, 1] * Array[2, 2] +
                      (dynamic)Array[0, 1] * Array[1, 2] * Array[2, 0] +
                      (dynamic)Array[0, 2] * Array[1, 0] * Array[2, 1] -
                      (dynamic)Array[0, 2] * Array[1, 1] * Array[2, 0] -
                      (dynamic)Array[1, 2] * Array[2, 1] * Array[0, 0] -
                      (dynamic)Array[2, 2] * Array[1, 0] * Array[0, 1];
            }

            if (rows > 3)
            {
                throw new Exception("暂未实现更高维度的行列式求值");
            }
            return ret;
        }

        public double ToDouble(K value)
        {
            return (dynamic)value;
        }

        /// <summary>
        /// 复制一个矩阵
        /// </summary>
        /// <returns></returns>
        public Mat<K> Copy()
        {
            var rows = GetRowCount();
            var cols = GetColCount();
            var a = new K[rows, cols];
            for (var i=0; i<rows; i++)
            {
                for(var j=0; j<cols; j++)
                {
                    a[i, j] = Array[i, j];
                }
            }
            return new Mat<K>(a);
        }

        /// <summary>
        /// 矩阵转置(行列交换)
        /// </summary>
        /// <returns>返加一个转置后的矩阵</returns>
        public Mat<K> T()
        {
            var rows = GetRowCount();
            var cols = GetColCount();

            var a = new K[cols, rows];
            for (var i = 0; i < cols; i++)
            {
                for (var j = 0; j < rows; j++)
                {
                    a[i, j] = Array[j, i];
                }
            }
            return new Mat<K>(a);
        }
    }
}
 