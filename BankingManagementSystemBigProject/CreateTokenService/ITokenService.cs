using Core.Entities;
using System.Security.Claims;

namespace BankingManagementSystemBigProject.CreateTokenService
{
    public interface ITokenService
    {
        public Task<string> CreateTokenAsync(User user);   
    }
}
