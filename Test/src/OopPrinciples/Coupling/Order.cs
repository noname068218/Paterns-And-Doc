using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.OopPrinciples.Coupling
{
    // Coupling Principle
    // Coupling refers to the degree of direct knowledge that one class has about another.
    // Low coupling is a design goal that seeks to reduce the interdependencies between classes.
        
    /// <summary>
    /// Example class to discuss coupling. As written, it directly creates
    /// an <see cref="EmailSender"/>, which increases coupling. A common
    /// improvement is to inject <c>EmailSender</c> (or an interface) so the
    /// code depends on an abstraction rather than a concrete type.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Places an order and notifies via email.
        /// </summary>
        public void PlaceOrder(string product)
        {
            EmailSender emailSender = new EmailSender();
            // Logic to place the order
            emailSender.SendEmail();
        }
    }
}
