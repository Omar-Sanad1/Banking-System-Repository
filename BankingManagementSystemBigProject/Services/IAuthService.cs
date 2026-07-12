using BankingManagementSystemBigProject.Models;
using Core.DTOs;

namespace BankingManagementSystemBigProject.Services
{
    public interface IAuthService
    {
        public Task<string> RegisterCustomerAsync(RegisterCustomerModel registerCustomer);
        public Task<string> LoginAsync(LoginModel loginModel);
    }
}
