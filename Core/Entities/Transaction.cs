using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Transaction : BaseEntity
    {
        public string TransactionNumber { get; set; }
        public string TransactionType { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDateAndTime { get; set; }
        public string TransactionDescription { get; set; }
        public string TransactionStatus { get; set; }
        public string ReferenceNumber { get; set; }
        public int AccountID { get; set; } // ==> FK
        public Account Account { get; set; } // ==> Navigation Property 
    }
}
