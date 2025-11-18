using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.OopPrinciples.Abstraction
{
    // Abstraction Principle
    // Abstraction is the concept of hiding the complex implementation details and showing only the essential features of the object.
    // It helps in reducing programming complexity and effort.
    // For example, when you use a smartphone, you don't need to understand the internal workings of the device.
    // You just interact with the user interface to perform tasks like making calls, sending messages, etc.
    /// <summary>
    /// Demonstrates Abstraction: exposes a simple <c>SendEmail</c> operation
    /// while hiding connection/authentication details behind private methods.
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// Public API that sends an email by internally handling connect,
        /// authenticate, and disconnect steps. The implementation details are
        /// abstracted away from callers.
        /// </summary>
        public void SendEmail()
        {
            Connect();
            Authenticate();
            Disconnect();
            Console.WriteLine("Email sent successfully!");
        }

        /// <summary>
        /// Simulates connecting to an email server. Kept private to hide the
        /// operational detail from consumers of <see cref="EmailService"/>.
        /// </summary>
        private void Connect()
        {
            // Simulate connecting to an email server
            Console.WriteLine("Connecting to email server...");
            Console.WriteLine("Connected successfully!");

        }

        /// <summary>
        /// Simulates disconnecting from the email server.
        /// </summary>
        private void Disconnect()
        {
            // Simulate disconnecting from an email server
            Console.WriteLine("Disconnecting from email server...");
            Console.WriteLine("Disconnected successfully!");
        }
        
        /// <summary>
        /// Simulates authenticating with the email server. Hidden detail to
        /// keep the public API small and focused.
        /// </summary>
        private void Authenticate()
        {
            Console.WriteLine("Authentication successful!");
        }
    }
}
