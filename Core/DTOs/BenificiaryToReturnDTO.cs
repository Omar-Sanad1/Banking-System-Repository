using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class BenificiaryToReturnDTO
    {
        public string BenificiaryName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string RelationshipType { get; set; }
        public DateTime CreationDate { get; set; }
        public string VerificationStatus { get; set; }
        public int AccountID { get; set; } 

    }
}
