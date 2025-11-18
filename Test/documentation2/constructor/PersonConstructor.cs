using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.documentation.constructor
{
    /// <summary>
    /// Demonstrates constructor overloading, default values, and composing
    /// objects (a person has a company).
    /// </summary>
    public class PersonConstructor
    {
        /// <summary>Person name.</summary>
        public string name;
        /// <summary>Associated company object.</summary>
        public CompanyConstructor company;
        /// <summary>Person age.</summary>
        public int age;

        // public Person() { name = "Unknow"; age = 18; }
        // public Person(string n) { name = n; age = 18; }
        // public Person(string n, int a) { name = n; age = a; }

        /// <summary>
        /// Default constructor initializing name and company.
        /// </summary>
        public PersonConstructor()
        {
            name = "Unknow";
            company = new CompanyConstructor();
        }

        /// <summary>
        /// Prints the person's name and company title.
        /// </summary>
        public void Print() => System.Console.WriteLine($"Name: {name}, Company: {company.title}");
    }
}
