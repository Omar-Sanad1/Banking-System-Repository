using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Account : BaseEntity
    {
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Currency { get; set; }
        public DateTime OpeningDate { get; set; }
        public string AccountStatus { get; set; }
        public decimal MinimumRequiredBalance { get; set; }
        public int BranchID { get; set; } // ==> FK
        public Branch Branch { get; set; } // ==> Navigation Property
        public int CustomerID { get; set; } // ==> FK
        public Customer Customer { get; set; } // ==> Navigation Property
        public List<Benificiary> Benificiaries { get; set; } = new();
        public List<Transaction> Transactions { get; set; } = new();
        public List<Card> Cards { get; set; } = new();
        public List<Payment> Payments { get; set; } = new();


    }
}
