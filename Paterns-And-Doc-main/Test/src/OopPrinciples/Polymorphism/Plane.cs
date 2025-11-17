using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.OopPrinciples.Polymorphism
{
    /// <summary>
    /// Concrete <see cref="Vehicle"/> representing a plane; overrides engine
    /// behavior and includes plane-specific data.
    /// </summary>
    public class Plane : Vehicle
    {
        /// <summary>Wingspan length (arbitrary units for example).</summary>
        public int WingSpan { get; set; }
        
        /// <inheritdoc />
        public override void StartEngine()
        {
            Console.WriteLine("Starting the plane's engine with a powerful thrust!");
        }
        /// <inheritdoc />
        public override void StopEngine()
        {
            Console.WriteLine("Stopping the plane's engine safely.");
        }
        
    }
}
