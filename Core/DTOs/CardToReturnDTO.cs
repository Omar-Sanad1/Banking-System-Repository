using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CardToReturnDTO
    {
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int CCV { get; set; }
        public decimal DailyTransactionLimit { get; set; }
        public string CardStatus { get; set; }
        public int AccountID { get; set; } 
    }
}
