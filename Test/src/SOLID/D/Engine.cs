using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.D
{
    // Dependency Inversion Principle example
    // High-level modules should not depend on low-level modules. Both should depend on abstractions.
    // Abstractions should not depend on details. Details should depend on abstractions.
/// This class is a placeholder to demonstrate the principle.
    /// <summary>
    /// Concrete engine implementation. Code that consumes <see cref="IEngine"/>
    /// does not need to know about this concrete type.
    /// </summary>
    public class Engine : IEngine
    {
        /// <inheritdoc />
        public void Start()
        {
            Console.WriteLine("Engine started.");
        }
    }
}
