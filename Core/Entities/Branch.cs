using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Branch : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int OperatigHours { get; set; }
        public string OperationalStatus { get; set; }
        public string BranchCode { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public List<Employee> Employees { get; set; } = new();
        public List<Account> Accounts { get; set; } = new();

    }
}
