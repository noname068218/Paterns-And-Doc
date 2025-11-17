using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.OopPrinciples.Polymorphism
{
    /// <summary>
    /// Concrete <see cref="Vehicle"/> that overrides engine behavior.
    /// </summary>
    public class Car : Vehicle
    {
        /// <summary>
        /// Number of doors on the car (property name preserved as-is).
        /// </summary>
        public int NumberPfDoors { get; set; }
        
        /// <inheritdoc />
        public override void StartEngine()
        {
            Console.WriteLine("Starting the car's engine with a roar!");
        }
        /// <inheritdoc />
        public override void StopEngine()
        {
            Console.WriteLine("Stopping the car's engine smoothly.");
        }

    }
}
