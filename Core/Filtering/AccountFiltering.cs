using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filtering
{
    public class AccountFiltering
    {
        public string? AccountNumber { get; set; }
        public DateTime? OpeningDate { get; set; }
        public int? CustomerID { get; set; }
        public string? SortBy { get; set; }
        public bool isDescending { get; set; }

    }
}
