using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.OopPrinciples.Polymorphism
{
    // Polymorphism Principle
    // Polymorphism is the ability of different classes to be treated as instances of the same class through a common interface.
    // It allows methods to do different things based on the object it is acting upon, even though they share the same name.
    // For example, consider a base class Vehicle with a method StartEngine().
    /// <summary>
    /// Base type demonstrating Polymorphism via virtual methods. Subclasses
    /// override the same method signatures to provide type-specific behavior,
    /// yet can be treated uniformly as <see cref="Vehicle"/>.
    /// </summary>
    public class Vehicle
    {
        /// <summary>Vehicle manufacturer.</summary>
        public string Brand { get; set; }
        /// <summary>Vehicle model name.</summary>
        public string Model { get; set; }

        /// <summary>Production year (property name kept as-is for example).</summary>
        public int year { get; set; }
        
        
        
        
        /// <summary>
        /// Starts the engine. Derived classes override to customize behavior.
        /// </summary>
        public virtual void StartEngine()
        {
            Console.WriteLine("Starting the vehicle's engine.");
        }

        /// <summary>
        /// Stops the engine. Derived classes override to customize behavior.
        /// </summary>
        public virtual void StopEngine()
        {
            Console.WriteLine("Stopping the vehicle's engine.");
        }

    }
}
