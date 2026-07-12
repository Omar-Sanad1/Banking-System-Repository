using Core.DTOs;
using Core.Entities;
using Service.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        public Task<IEnumerable<AccountToReturnDTO>> GetAllAccountsPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<AccountToReturnDTO> GetAllAccountsFiltered(Func<Account , bool> Filter);
        public Task<IEnumerable<AccountToReturnDTO>> GetAllCustomerAccountsAsync(int customerID);
        public Task<AccountToReturnDTO> GetAccountByIDAsync(int accountId);
        public Task<AccountToReturnDTO> GetAcountByNumberAsync(string accountNumber);
        public Task<decimal> GetAccountBalanceByAccountIDAsync(int accountId);
        public Task<AccountToReturnDTO> AddNewAccountAsync(AddNewAccountModel addNewAccount);
        public Task<AccountToReturnDTO> UpdateAccountInformationAsync(int accountID , UpdateAccountInformationModel updateAccountInformation);
        public Task DeleteAccountByIDAsync(int accountId);
        public Task<AccountToReturnDTO> CloseAccountAsync(int accountId);
        public Task<AccountToReturnDTO> FreezeAccountAsync(int accountId);
        public Task<AccountToReturnDTO> ActivateAccountAsync(int accountId);


    }
}
