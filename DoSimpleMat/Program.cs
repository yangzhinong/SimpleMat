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
            var b = new int[,] { { 5, 6 }, { 7, 8 } };
                    

            var A = new Mat<int>(a);
            var B = new Mat<int>(b);
            A.Print(nameof(A));
            B.Print(nameof(B));

            var C = A + B;
            var D = A * B;

            C.Print(nameof(C));
            D.Print(nameof(D));

            Console.ReadLine();
        }
    }
}
