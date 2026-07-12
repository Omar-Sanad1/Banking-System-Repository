using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class AccountToReturnDTO
    {
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string Currency { get; set; }
        public DateTime OpeningDate { get; set; }
        public string AccountStatus { get; set; }
        public decimal MinimumRequiredBalance { get; set; }
        public string BranchName { get; set; } 
        public string CustomerName { get; set; } 
    }
}
