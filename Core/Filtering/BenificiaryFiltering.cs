using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filtering
{
    public class BenificiaryFiltering
    {
        public string? BenificiaryName { get; set; }
        public string? AccountNumber { get; set; }
        public string? SortBy { get; set; }
        public bool isDescending { get; set; }
    }
}
