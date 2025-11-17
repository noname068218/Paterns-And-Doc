using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.OopPrinciples.Polymorphism
{
    /// <summary>
    /// Concrete <see cref="Vehicle"/> showing polymorphic overrides.
    /// </summary>
    public class Motorcycle : Vehicle
    {
        /// <inheritdoc />
        public override void StartEngine()
        {
            Console.WriteLine("Starting the motorcycle's engine with a vroom!");
        }
        /// <inheritdoc />
        public override void StopEngine()
        {
            Console.WriteLine("Stopping the motorcycle's engine gently.");
        }
    }
}
