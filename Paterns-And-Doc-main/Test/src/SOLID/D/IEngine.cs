using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.D
{
    /// <summary>
    /// Abstraction for engine behavior used to invert dependencies.
    /// High-level modules depend on this interface rather than concrete engines.
    /// </summary>
    public interface IEngine
    {
        /// <summary>Starts the engine.</summary>
        void Start();    }
}
