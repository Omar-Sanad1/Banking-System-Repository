using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class EmployeeToReturnDTO
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public DateTime HiringDate { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; }
        public int UserID { get; set; } 
        public string BranchName { get; set; } 
    }
}
