using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class LoanToReturnDTO
    {
        public string LoanType { get; set; }
        public decimal RequestedAmount { get; set; }
        public int RepaymentPeriod { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ApprovalStatus { get; set; }
        public decimal MonthlyInstallmentAmount { get; set; }
        public string CustomerName { get; set; } 
    }
}
