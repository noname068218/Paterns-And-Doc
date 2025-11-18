using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.OopPrinciples.Coupling
{
    /// <summary>
    /// A simple collaborator used by other classes to send emails.
    /// In coupling discussions, this often represents a dependency that
    /// could be injected rather than directly constructed by consumers.
    /// </summary>
    public class EmailSender
    {
        /// <summary>
        /// Simulates sending an email. In real systems, this would talk to
        /// an SMTP server or external email service.
        /// </summary>
        public void SendEmail()
        {
            Console.WriteLine("Sending email");
        }
    }
}
