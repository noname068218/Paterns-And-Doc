using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.I
{
    // Interface Segregation Principle
    // Clients should not be forced to depend on interfaces they do not use.
    // This example keeps 3D responsibilities separate from 2D ones.
    
    /// <summary>
    /// Focused interface for 3D shapes. Separates 3D responsibilities
    /// (Area and Volume) from 2D concerns.
    /// </summary>
    public interface IShape3D
    {
        /// <summary>Surface area of the 3D shape.</summary>
        double Area();
        /// <summary>Volume of the 3D shape.</summary>
        double Volume();
    }
}
