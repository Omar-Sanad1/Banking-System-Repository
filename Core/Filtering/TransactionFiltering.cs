using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filtering
{
    public class TransactionFiltering
    {
        public string? TransactionNumber { get; set; }
        public DateTime? TransactionDateAndTime { get; set; }
        public string? SortBy { get; set; }
        public bool isDescending { get; set; }


    }
}
