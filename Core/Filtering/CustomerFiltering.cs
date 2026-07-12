using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filtering
{
    public class CustomerFiltering
    {
        public string? FullName { get; set; }
        public string? NationalID { get; set; }
        public string? SortBy { get; set; }
        public bool isDescending { get; set; }

    }
}
