using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.AccountModels
{
    public class UpdateAccountInformationModel
    {
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Currency { get; set; }
        public string AccountStatus { get; set; }
        public decimal MinimumRequiredBalance { get; set; }
        public int BranchID { get; set; }
        public int CustomerID { get; set; }
    }
}
