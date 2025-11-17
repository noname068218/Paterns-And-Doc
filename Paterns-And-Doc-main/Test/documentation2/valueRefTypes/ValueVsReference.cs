using System;

namespace Test.documentation2.valueRefTypes
{
    /// <summary>
    /// Explains how value types (struct) and reference types (class) behave in C#.
    /// </summary>
    /// <remarks>
    /// Key ideas:
    /// - Value types are copied by value. Assigning one variable to another creates a copy.
    /// - Reference types are accessed via references. Assignments copy the reference, not the object.
    /// - Mutating a copy of a value type does not affect the original, while mutating through a
    ///   reference affects the same underlying object.
    /// </remarks>
    public static class ValueVsReference
    {
        /// <summary>
        /// Simple value type. When assigned or passed by value, a copy is made.
        /// </summary>
        public struct PointStruct
        {
            /// <summary>X coordinate.</summary>
            public int X;
        }

        /// <summary>
        /// Simple reference type. Variables hold a reference to the same object instance
        /// after assignment.
        /// </summary>
        public class PointClass
        {
            /// <summary>X coordinate.</summary>
            public int X { get; set; }
        }

        /// <summary>
        /// Minimal console-based demonstration of assignment semantics for value vs reference types.
        /// You can call this from a Main method to observe the differences.
        /// </summary>
        public static void RunDemo()
        {
            Console.WriteLine("-- Value type (struct) assignment copies the value --");
            var a = new PointStruct { X = 10 };
            var b = a;                // copy of the value
            b.X = 20;                 // mutate the copy
            Console.WriteLine($"a.X = {a.X} (10), b.X = {b.X} (20)");

            Console.WriteLine();
            Console.WriteLine("-- Reference type (class) assignment copies the reference --");
            var c = new PointClass { X = 10 };
            var d = c;                // same object via two references
            d.X = 20;                 // mutate through one reference
            Console.WriteLine($"c.X = {c.X} (20), d.X = {d.X} (20)");
        }

        // Additional notes:
        // - Passing a value type by 'ref' or 'in'/'out' changes semantics (no copy or by-ref).
        // - Arrays, strings, and most framework types are reference types (strings are immutable).
        // - Nullable value types (int?) are still value types that can represent 'no value'.
    }
}

