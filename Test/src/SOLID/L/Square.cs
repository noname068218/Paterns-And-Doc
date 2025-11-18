using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.L
{
    /// <summary>
    /// Square with a single side length, consistent with the base
    /// <see cref="Shape"/> expectations for area.
    /// </summary>
    public class Square : Shape
    {
        /// <summary>Length of one side.</summary>
        public double Side { get; set; }

        /// <inheritdoc />
        public override double Area => Side * Side;
    }
}
