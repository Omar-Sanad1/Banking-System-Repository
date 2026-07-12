using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Payment : BaseEntity
    {
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReferenceNumber { get; set; }
        public string ServiceType { get; set; }
        public int AccountID { get; set; } // ==> FK
        public Account Account { get; set; } // ==> Navigation Property
    }
}
