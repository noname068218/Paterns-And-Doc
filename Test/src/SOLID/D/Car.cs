using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.D
{
    /// <summary>
    /// High-level module depending on the abstraction <see cref="IEngine"/>.
    /// The concrete engine is provided from the outside (constructor injection),
    /// demonstrating Dependency Inversion.
    /// </summary>
    public class Car
    {
        private IEngine engine;

        /// <summary>
        /// Creates a car with the given engine dependency.
        /// </summary>
        /// <param name="engine">Engine implementation to use.</param>
        public Car(IEngine engine)
        {
            this.engine = engine;
        }
        /// <summary>
        /// Starts the car by delegating to the injected engine.
        /// </summary>
        public void Start()
        {
            engine.Start();
            Console.WriteLine("Car started.");
        }
    }
}
