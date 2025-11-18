using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.OopPrinciples.Encapsulation
{
    /// <summary>
    /// Anti-example for Encapsulation. Exposes a public field that callers
    /// can mutate freely, which makes it hard to enforce invariants.
    /// Prefer using private fields with public methods or properties.
    /// </summary>
    public class BadBankAccount
    {
        /// <summary>
        /// Public field (not recommended). Anyone can set this directly
        /// without validation.
        /// </summary>
        public decimal Balance;
    }
}
