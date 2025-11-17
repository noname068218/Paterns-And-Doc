using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.src.SOLID.S
{
    //Single Responsibility Principle
    //A class should have only one reason to change. 
    //This means that a class should only have one job or responsibility.
    //In this example, the User class is responsible only for user-related data and behavior.
    // Any other responsibilities, such as data access or validation, should be handled by separate classes.
    ///This keeps the User class focused and easier to maintain.
    // If we need to change how user data is stored or validated, we can do so without affecting the User class itself.
    /// <summary>
    /// Holds user-related data only to illustrate the Single Responsibility
    /// Principle: this class has one reason to change (user data shape).
    /// </summary>
    public class User
    {
        /// <summary>User display name.</summary>
        public string Name { get; set; }
        /// <summary>User email address.</summary>
        public string Email { get; set; }

       

    }
}
