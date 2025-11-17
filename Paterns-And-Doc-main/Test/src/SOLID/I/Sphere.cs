using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.I
{
    /// <summary>
    /// 3D sphere implementing a focused 3D interface.
    /// </summary>
    public class Sphere : IShape3D
    {
        /// <summary>Radius of the sphere.</summary>
        public double Radius { get; set; }
        
        /// <inheritdoc />
        public double Area()
        {
            return 4 * Math.PI * Radius * Radius;
        }

        /// <inheritdoc />
        public double Volume()
        {
            return (4.0 / 3.0) * Math.PI * Math.Pow(Radius, 3);
        }
    }
}
