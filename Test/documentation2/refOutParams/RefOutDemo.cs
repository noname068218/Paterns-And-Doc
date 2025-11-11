using System;

namespace Test.documentation2.refOutParams
{
    /// <summary>
    /// Demonstrates the <c>ref</c> and <c>out</c> parameter modifiers in C#.
    /// </summary>
    /// <remarks>
    /// ref: Passes a variable by reference so the callee can read and write it.
    ///      Caller must assign the variable before the call.
    /// out: Passes a variable by reference for output; the callee must assign it
    ///      before returning. Caller does not need to assign it before the call.
    /// Common uses: Try-pattern methods (e.g., <c>int.TryParse</c>), swapping values,
    /// and in-place mutations without returning tuples.
    /// </remarks>
    public static class RefOutDemo
    {
        /// <summary>
        /// Increments the provided integer in place via <c>ref</c>.
        /// </summary>
        /// <param name="value">Value to increment. Must be assigned before the call.</param>
        public static void IncrementRef(ref int value)
        {
            value++;
        }

        /// <summary>
        /// Divides two integers and returns the quotient and remainder via <c>out</c> parameters.
        /// </summary>
        /// <param name="dividend">Dividend.</param>
        /// <param name="divisor">Divisor.</param>
        /// <param name="quotient">Output quotient. Assigned inside the method.</param>
        /// <param name="remainder">Output remainder. Assigned inside the method.</param>
        /// <returns><c>true</c> if division succeeds; <c>false</c> on divide-by-zero.</returns>
        public static bool TryDivide(int dividend, int divisor, out int quotient, out int remainder)
        {
            if (divisor == 0)
            {
                quotient = 0;
                remainder = 0;
                return false;
            }
            quotient = dividend / divisor;
            remainder = dividend % divisor;
            return true;
        }

        /// <summary>
        /// Swaps two integers in place using <c>ref</c> parameters.
        /// </summary>
        /// <param name="a">First value (will receive the original value of <paramref name="b"/>).</param>
        /// <param name="b">Second value (will receive the original value of <paramref name="a"/>).</param>
        public static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// Console demonstration that you can call from a Main method to observe
        /// the differences between <c>ref</c> and <c>out</c>.
        /// </summary>
        public static void RunDemo()
        {
            Console.WriteLine("-- ref: variable must be assigned before call --");
            int x = 10; // must be definitely assigned
            IncrementRef(ref x);
            Console.WriteLine($"x after IncrementRef = {x} (expected 11)");

            Console.WriteLine();
            Console.WriteLine("-- out: callee assigns outputs; caller may leave uninitialized --");
            int quotient; // not assigned before the call
            int remainder; // not assigned before the call
            bool ok = TryDivide(7, 3, out quotient, out remainder);
            Console.WriteLine($"ok = {ok}, quotient = {quotient}, remainder = {remainder} (expected 2,1)");

            Console.WriteLine();
            Console.WriteLine("-- 'out var' inline declaration --");
            if (TryDivide(10, 0, out var q, out var r))
            {
                Console.WriteLine($"10 / 0 = {q} (unexpected)");
            }
            else
            {
                Console.WriteLine("Division by zero is not allowed (expected).");
            }

            Console.WriteLine();
            Console.WriteLine("-- Swap with ref --");
            int a = 1, b = 2;
            Swap(ref a, ref b);
            Console.WriteLine($"a = {a}, b = {b} (expected 2,1)");
        }

        // Notes:
        // - Both ref and out pass by reference; the difference is in definite assignment rules.
        // - You can combine 'in' for by-ref read-only, but that's outside this example's scope.
        // - Prefer Try-pattern with out for parse/compute operations that may fail.
    }
}

