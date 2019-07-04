using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMP255FinalProject
{
    public class CustomerAccount
    {
        public CustomerAccount(int AccountNumber,
                               string FirstName, 
                               string LastName,
                               string Email, 
                               string Phone)

        {
            this.AccountNumber = AccountNumber;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
        }

        public int AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public override string ToString() => $"{AccountNumber} {FirstName} {LastName} {Email} {Phone}";
    }
}