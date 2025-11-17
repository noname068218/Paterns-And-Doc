using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.I
{
    /// <summary>
    /// 2D circle used to illustrate Interface Segregation with a focused interface.
    /// Note: <see cref="IShape2D"/> is empty here for demonstration purposes; the
    /// <c>Area</c> method is shown as a typical 2D responsibility.
    /// </summary>
    public class Circle : IShape2D
    {
        /// <summary>Radius of the circle.</summary>
        public double Radius { get; set; }

        /// <summary>
        /// Computes the area of the circle: πr²
        /// </summary>
        public double Area()
        {
            return Math.PI * Radius * Radius;
        }
    }
}
