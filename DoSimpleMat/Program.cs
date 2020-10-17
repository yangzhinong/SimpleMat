using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoSimpleMat
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new int[,] { { 1, 2 }, { 3, 4 } };

            Mat<int>.Print(a, nameof(a));
            var b = new int[,] { { 5, 6 }, { 7, 8 } };
            Mat<int>.Print(b, nameof(b));

            var oMat = new Mat<int>();
            var c = oMat.MatMult(a, b);
            Mat<int>.Print(c, nameof(c));

            Console.ReadLine();
        }
    }
}
