using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.O
{
    /// <summary>
    /// Rectangle extension of <see cref="Shape"/> that implements area.
    /// </summary>
    public class Rectangle : Shape
    {
        /// <summary>Rectangle length.</summary>
        public double Length { get; set; }
        /// <summary>Rectangle width.</summary>
        public double Width { get; set; }

        /// <inheritdoc />
        public override double Area()
        {
            return Length * Width;
        }
    }
}
