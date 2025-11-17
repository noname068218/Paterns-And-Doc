using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.I
{
    /// <summary>
    /// Example of Interface Segregation: a small, focused interface for 2D shapes.
    /// In a real design this might declare members like <c>double Area()</c>,
    /// leaving unrelated members (e.g., Volume) to separate interfaces.
    /// </summary>
    public interface IShape2D
    {
        
    }
}
