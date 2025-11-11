using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.documentation.recursFunctions
{

    /// <summary>
    /// Demonstrates recursion with factorial and Fibonacci examples.
    /// </summary>
    public class Recurs
    {
        /// <summary>
        /// Computes n! recursively. Base case: 1! = 1.
        /// </summary>
        /// <param name="n">Positive integer.</param>
        /// <returns>Factorial of <paramref name="n"/>.</returns>
        public int Factorial(int n)
        {
            if (n == 1) return 1;
            return n * Factorial(n - 1);
        }

      /// <summary>
      /// Computes the nth Fibonacci number recursively (naive approach).
      /// Uses base cases F(0) = 0 and F(1) = 1.
      /// </summary>
      /// <param name="n">Index in the Fibonacci sequence.</param>
      /// <returns>Fibonacci number at position <paramref name="n"/>.</returns>
      public int Fibonachi(int n)
{
    if (n == 0 || n == 1) return n;
     
    return Fibonachi(n - 1) + Fibonachi(n - 2);
}
    }
}
