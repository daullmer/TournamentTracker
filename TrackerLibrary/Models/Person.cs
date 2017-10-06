using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAdress { get; set; }
        public string FullName
        {
            get => $"{FirstName} {LastName}";
        }

        public override string ToString() => FullName;

    }
}
