using System;

namespace Test.documentation2.properties
{
    /// <summary>
    /// Explains C# properties and the <c>get</c>/<c>set</c> accessors, including
    /// auto-properties, backing fields, validation, private setters, init-only
    /// setters, and computed (getter-only) properties.
    /// This is a test of the GetSetDemo class.
    /// </summary>
    public static class GetSetDemo
    {

               public static    void Increment1(ref int val)
{
    val++;
    Console.WriteLine(val);
}
 
public static void Increment2(int val)
{
    val++;
    Console.WriteLine(val);
}
        /// <summary>
        /// Example class with different kinds of properties.
        /// </summary>
        public class Person
        {



            private string _name = string.Empty; // backing field to add validation

            /// <summary>
            /// Property with custom <c>get</c>/<c>set</c> using a backing field.
            /// Demonstrates validation in the setter.
            /// </summary>
            public string Name
            {
                get => _name;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Name must be non-empty.");
                    _name = value;
                }
            }

            /// <summary>
            /// Auto-property with a private setter: callers can read the value
            /// but only methods on this class can modify it.
            /// </summary>
            public int Age { get; private set; }

            /// <summary>
            /// Init-only property: can be set during object initialization but
            /// becomes read-only thereafter (C# 9+).
            /// </summary>
            public string Nickname { get; init; } = string.Empty;

            /// <summary>
            /// Computed, read-only property using an expression-bodied getter.
            /// </summary>
            public double Bmi => HeightMeters <= 0 ? 0 : Math.Round(WeightKg / (HeightMeters * HeightMeters), 2);

            /// <summary>
            /// Auto-properties (no validation). Defaults are 0 for numeric types.
            /// </summary>
            public double HeightMeters { get; set; }
            /// <summary>Weight in kilograms.</summary>
            public double WeightKg { get; set; }

            /// <summary>
            /// Getter-only auto-property with an initializer. No setter provided,
            /// so this value is read-only after construction.
            /// </summary>
            public Guid Id { get; } = Guid.NewGuid();

            /// <summary>
            /// Property with restricted setter access; illustrates accessor-level
            /// accessibility. Public getter, private setter.
            /// </summary>
            public string Secret { get; private set; } = "shh";

            /// <summary>
            /// Example method that can change a property with a private setter.
            /// </summary>
            public void HaveBirthday() => Age++;
        }

        /// <summary>
        /// Console demonstration that you can call from a Main method to observe
        /// the behavior of get/set accessors.
        /// </summary>
        public static void RunDemo()
        {
            var p = new Person
            {
                Nickname = "Ace",
                HeightMeters = 1.80,
                WeightKg = 75,
            };
            p.Name = "Alex";                 // uses setter with validation

            Console.WriteLine($"Name = {p.Name}");
            Console.WriteLine($"Age = {p.Age}");
            Console.WriteLine($"Nickname = {p.Nickname}");
            Console.WriteLine($"Id = {p.Id}");
            Console.WriteLine($"BMI = {p.Bmi}");

            // Modify a property with a private setter via a method
            p.HaveBirthday();
            Console.WriteLine($"Age after birthday = {p.Age}");

            // Demonstrate validation error in the setter
            try
            {
                p.Name = ""; // will throw
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Setting empty Name failed: {ex.Message}");
            }

            // The following would be a compile error (init-only):
            // p.Nickname = "NewNick"; // cannot assign after initialization

            // Accessor accessibility example
            Console.WriteLine($"Secret (read): {p.Secret}");
            // p.Secret = "oops"; // compile error: private setter
        }
    }
}

