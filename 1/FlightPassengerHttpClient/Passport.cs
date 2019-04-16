using System;
using System.Collections.Generic;
using System.Text;

namespace FlightPassengerHttpClient
{
    public enum Sex
    {
        Male,
        Female
    }
    public class Passport
    {
        public Passport()
        {

        }
        public Passport(Guid guid, string surname, string givenNames, DateTime dateOfBirth, Sex sex)
        {
            Guid = guid;
            Surname = surname;
            GivenNames = givenNames;
            DateOfBirth = dateOfBirth;
            Sex = sex;
        }
        public Guid Guid { get; set; }
        public string Surname { get; set; }
        public string GivenNames { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
    }
}
