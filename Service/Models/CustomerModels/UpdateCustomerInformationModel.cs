using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.CustomerModels
{
    public class UpdateCustomerInformationModel
    {
        public string FullName { get; set; }
        public string NationalID { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ResidintialAddress { get; set; }
        public string Occuption { get; set; }
        public DateTime RegisterationDate { get; set; }
        public string AccountStatus { get; set; }
        public int UserID { get; set; } 
    }
}
