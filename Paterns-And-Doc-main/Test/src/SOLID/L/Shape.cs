using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.L
{
    // Liskov Substitution Principle
    // Objects of a superclass should be replaceable with objects of a subclass without affecting the correctness of the program.
    // This means that subclasses should extend the behavior of the superclass without changing its original functionality.
    // For example, if you have a class Shape with a method Area(), any subclass like Circle
    // or Rectangle should implement Area() in a way that is consistent with the expectations set by Shape.
    // Violating LSP can lead to unexpected behaviors and bugs in the code.
    // Here is an example of adhering to LSP:
    /// <summary>
    /// LSP-compliant base type that exposes an <c>Area</c> contract which
    /// subclasses honor without changing expectations.
    /// </summary>
    public abstract class Shape
    {
        /// <summary>Computed area of the shape.</summary>
        public abstract double Area{ get; }
    }
}
