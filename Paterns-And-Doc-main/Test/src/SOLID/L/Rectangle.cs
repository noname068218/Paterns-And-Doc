using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.L
{
    /// <summary>
    /// Rectangle that respects the <see cref="Shape"/> contract for area.
    /// </summary>
    public class Rectangle : Shape
    {
        /// <summary>Rectangle width.</summary>
        public virtual double Width { get; set; }

        /// <summary>Rectangle height.</summary>
        public virtual double Height { get; set; }
        
        /// <inheritdoc />
        public override double Area => Width * Height;
    }
}
