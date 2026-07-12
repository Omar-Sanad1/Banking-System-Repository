using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.EmployeeModels
{
    public class AddNewEmployeeModel
    {
        public string FullName { get; set; }
        public string NationalID { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public DateTime HiringDate { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public int BranchID { get; set; } 

        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }

    }
}
