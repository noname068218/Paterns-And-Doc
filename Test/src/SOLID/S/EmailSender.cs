using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.S
{
    /// <summary>
    /// Dedicated email sending responsibility, separated from user data or
    /// registration workflows to adhere to SRP.
    /// </summary>
    public class EmailSender
    {
        /// <summary>
        /// Sends an email message to the specified address.
        /// </summary>
        /// <param name="email">Recipient address.</param>
        /// <param name="message">Message body text.</param>
        public void SendEmail(string email, string message)
        {
            // Email sending logic here
            Console.WriteLine($"Sending email to {email}: {message}");
        }
    }
}
