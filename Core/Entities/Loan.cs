using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Loan : BaseEntity
    {
        public string LoanType { get; set; }
        public decimal RequestedAmount { get; set; }
        public int RepaymentPeriod { get; set; } 
        public decimal InterestRate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ApprovalStatus { get; set; }
        public decimal MonthlyInstallmentAmount { get; set; }
        public int CustomerID { get; set; } // ==> FK
        public Customer Customer { get; set; } // ==> Navigation Property
    }
}
