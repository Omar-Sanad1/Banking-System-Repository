using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Customer : BaseEntity
    {
        public string FullName { get; set; }
        public string NationalID { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ResidintialAddress { get; set; }
        public string Occuption { get; set; }
        public DateTime RegisterationDate { get; set; }
        public string AccountStatus { get; set; }
        public int UserID { get; set; } // ==> FK
        public User User { get; set; } // ==> Navigation Property
        public List<Account> Accounts { get; set; } = new();
        public List<Loan> Loans { get; set; } = new();


    }
}
