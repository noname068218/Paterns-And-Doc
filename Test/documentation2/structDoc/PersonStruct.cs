using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.documentation2.structDoc
{
    /// <summary>
    /// Simple value type (struct) example. In C#, structs are value types,
    /// so assignments and method calls copy the value by default. Changes to a
    /// copy do not affect the original instance.
    /// </summary>
    /// <remarks>
    /// The commented usage below shows initialization and printing. The
    /// <c>with</c> expression works with record types (e.g., <c>record struct</c>),
    /// not with a regular <c>struct</c> like this.
    /// </remarks>
    public struct PersonStruct
    {
        /// <summary>Person name.</summary>
        public string name;
        /// <summary>Person age.</summary>
        public int age;

        /// <summary>
        /// Prints the person data in the format "name, age".
        /// </summary>
        public void Print() => System.Console.WriteLine($"{name}, {age}");
    }

    
// using Test.documentation2.structDoc;

// PersonStruct tom = new PersonStruct { name = "Tom", age = 22 };

// PersonStruct bob = tom with { name = "Bob" }; // requires a record type

// bob.Print();

}
