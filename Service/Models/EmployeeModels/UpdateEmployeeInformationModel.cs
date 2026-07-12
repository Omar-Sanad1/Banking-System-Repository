using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.EmployeeModels
{
    public class UpdateEmployeeInformationModel
    {
        public string FullName { get; set; }
        public string NationalID { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public DateTime HiringDate { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; }
        public int BranchID { get; set; }
        public int UserID { get; set; }
    }
}
