using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.O
{
    /// <summary>
    /// Circle extension of <see cref="Shape"/> that provides an area
    /// calculation without modifying the base type (Open/Closed).
    /// </summary>
    public class Circle : Shape
    {
        /// <summary>Radius of the circle.</summary>
        public double Radius { get; set; }

        public Circle(double radius)
        {
            Radius = radius;
        }

        /// <inheritdoc />
        public override double Area()
        {
            return Math.PI * Radius * Radius;
        }
    }
}
