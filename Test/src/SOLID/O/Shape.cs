using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.O
{
    //Open/Closed Principle
    //Software entities (classes, modules, functions, etc.) should be open for extension but
    // closed for modification.
    //This means that you should be able to add new functionality to a class without changing its
   /// <summary>
   /// Base type for shapes. New shapes can extend this type and implement
   /// <see cref="Area"/> without modifying existing code, illustrating the
   /// Open/Closed Principle.
   /// </summary>
   public  abstract class Shape
    {
        /// <summary>Computes the area of the shape.</summary>
        public abstract double Area();
    }
}
