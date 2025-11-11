using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.OopPrinciples.Encapsulation
{
    // Encapsulation Principle
    // Encapsulation is the principle of bundling the data (attributes) and methods (functions) that operate on the data into a single unit or class.
    // It restricts direct access to some of the object's components, which helps prevent unintended interference and misuse of the methods and data.
    
    /// <summary>
    /// Demonstrates Encapsulation by keeping state private and exposing
    /// behavior through methods that validate inputs and preserve invariants.
    /// </summary>
    public class BankAccount
    {
        private decimal balance;

        /// <summary>
        /// Creates a new bank account and deposits the initial balance via
        /// the public API to reuse validation.
        /// </summary>
        /// <param name="balance">Initial amount to deposit.</param>
        public BankAccount(decimal balance)
        {
            Deposit(balance);
        }

        /// <summary>
        /// Deposits a positive amount into the account.
        /// </summary>
        /// <param name="amount">Amount to add. Must be positive.</param>
        /// <exception cref="ArgumentException">Thrown when amount is not positive.</exception>
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive.");
            }
            this.balance += amount;
        }

        /// <summary>
        /// Returns the current account balance.
        /// </summary>
        public decimal GetBalance()
        {
            return balance;
        }
        
        /// <summary>
        /// Withdraws a positive amount up to the available balance.
        /// </summary>
        /// <param name="amount">Amount to withdraw.</param>
        /// <exception cref="ArgumentException">Thrown when amount is not positive.</exception>
        /// <exception cref="InvalidOperationException">Thrown when funds are insufficient.</exception>
        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be positive.");
            }
            if (amount > balance)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }
            this.balance -= amount;
        }
    }
}
