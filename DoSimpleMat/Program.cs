using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoSimpleMat
{
    class Program
    {
        static void Main(string[] args)
        {
    
           var  F = new Mat<int>(new int[,]
            {
                {1,2},
                {-1,-3 }
            });

            F.Get_1().Print("矩阵逆");



            Console.ReadLine();
        }
    }
}
