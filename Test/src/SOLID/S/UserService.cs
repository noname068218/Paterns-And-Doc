using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.S
{
    /// <summary>
    /// Service that handles user workflows (e.g., registration). In SRP terms,
    /// this separates orchestration from data (<see cref="User"/>) and delivery
    /// (<see cref="EmailSender"/>).
    /// </summary>
    public class UserService : User
    {
         /// <summary>
         /// Registers the given user and sends a welcome email.
         /// </summary>
         /// <param name="user">User data to register.</param>
         public void Register(User user)
        {
            // Registration logic here
            EmailSender emailSender = new EmailSender();
            emailSender.SendEmail(user.Email, "Welcome to our service!");
        }
    }
}
