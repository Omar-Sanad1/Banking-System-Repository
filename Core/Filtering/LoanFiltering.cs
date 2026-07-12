using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filtering
{
    public class LoanFiltering
    {
        public string? LoanType { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string? SortBy { get; set; }
        public bool isDescending { get; set; }


    }
}
