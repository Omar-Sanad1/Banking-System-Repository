namespace BankingManagementSystemBigProject.Models
{
    public class RegisterCustomerModel
    {
        public string FullName { get; set; }
        public string NationalID { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ResidintialAddress { get; set; }
        public string Occuption { get; set; }

        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
