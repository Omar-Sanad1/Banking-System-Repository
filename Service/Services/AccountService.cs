using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Repository.Context;
using Service.Interfaces;
using Service.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankingSystemDbContext _dbContext;
        private readonly IMapper _mapper;
        public AccountService(BankingSystemDbContext dbContext , IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AccountToReturnDTO>> GetAllAccountsPagedAsync(int pageNumber, int pageSize)
        {
            var accounts =  await _dbContext.Set<Account>()
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            return _mapper.Map<IEnumerable<Account>, IEnumerable<AccountToReturnDTO>>(accounts);
        }

        public IEnumerable<AccountToReturnDTO> GetAllAccountsFiltered(Func<Account, bool> Filter)
        {
            var accounts =  _dbContext.Set<Account>()
                            .Where(Filter)
                            .ToList();

            return _mapper.Map<IEnumerable<Account>, IEnumerable<AccountToReturnDTO>>(accounts);
        }
        public async Task<AccountToReturnDTO> GetAccountByIDAsync(int accountId)
        {
            var specifiedAccount = await _dbContext.Accounts
                                   .Include(a=>a.Customer)
                                   .Include(a=>a.Branch)
                                   .FirstOrDefaultAsync(a => a.ID == accountId);

            if (specifiedAccount is null)
                throw new NotFoundException("This account id isn't exist.");

            return _mapper.Map<Account, AccountToReturnDTO>(specifiedAccount);
        }

        public async Task<AccountToReturnDTO> GetAcountByNumberAsync(string accountNumber)
        {
            var specifiedAccount = await _dbContext.Accounts
                                   .Include(a => a.Customer)
                                   .Include(a => a.Branch)
                                   .FirstOrDefaultAsync(a=>a.AccountNumber == accountNumber);

            if (specifiedAccount is null)
                throw new NotFoundException("This account number isn't exist.");

            return _mapper.Map<Account, AccountToReturnDTO>(specifiedAccount);
        }


        public async Task<decimal> GetAccountBalanceByAccountIDAsync(int accountId)
        {
            var specifiedAccount = await _dbContext.Accounts
                                   .Include(a => a.Customer)
                                   .Include(a => a.Branch)
                                   .FirstOrDefaultAsync(a => a.ID == accountId);

            if (specifiedAccount is null)
                throw new NotFoundException("This account id isn't exist.");

            return specifiedAccount.CurrentBalance;
        }


        public async Task<IEnumerable<AccountToReturnDTO>> GetAllCustomerAccountsAsync(int customerID)
        {
            var specifiedCustomer = await _dbContext.Customers
                                   .FirstOrDefaultAsync(c => c.ID == customerID);

            if(specifiedCustomer is null)
                throw new NotFoundException("This customer id isn't exist.");

            var accounts =  await _dbContext.Accounts
                            .Include(a => a.Customer)
                            .Include(a => a.Branch)
                            .Where(a => a.CustomerID == customerID)
                            .ToListAsync();

            return _mapper.Map<IEnumerable<Account>, IEnumerable<AccountToReturnDTO>>(accounts);
        }


        public async Task<AccountToReturnDTO> AddNewAccountAsync(AddNewAccountModel addNewAccount)
        {
            var existsCustomer = await _dbContext.Customers.FindAsync(addNewAccount.CustomerID);
            if (existsCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            var existsBranch = await _dbContext.Branches.FindAsync(addNewAccount.BranchID);
            if (existsBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            var existsAccount = await _dbContext.Accounts.AnyAsync(a=>a.AccountNumber == addNewAccount.AccountNumber);
            if(existsAccount)
                throw new Exception("This account is already exist.");

            var account = new Account
            {
                AccountNumber = addNewAccount.AccountNumber,
                AccountType = addNewAccount.AccountType,
                CurrentBalance = addNewAccount.CurrentBalance,
                Currency = addNewAccount.Currency,
                OpeningDate = DateTime.Now,
                MinimumRequiredBalance = addNewAccount.MinimumRequiredBalance,
                BranchID = addNewAccount.BranchID,
                CustomerID = addNewAccount.CustomerID,
                AccountStatus = addNewAccount.AccountStatus,
            };

            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            account = await _dbContext.Accounts
                     .Include(a => a.Customer)
                     .Include(a => a.Branch)
                     .FirstAsync(a => a.ID == account.ID);

            return _mapper.Map<Account, AccountToReturnDTO>(account);
        }

        public async Task DeleteAccountByIDAsync(int accountId)
        {
            var specifiedAccount = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.ID == accountId);
            if (specifiedAccount is null)
                throw new NotFoundException("This account isn't exist.");

            _dbContext.Accounts.Remove(specifiedAccount);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<AccountToReturnDTO> UpdateAccountInformationAsync(int accountID , UpdateAccountInformationModel updateAccountInformation)
        {
            var specifiedAccount = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.ID == accountID);
            if (specifiedAccount is null)
                throw new NotFoundException("This account isn't exist.");

            specifiedAccount.AccountNumber = updateAccountInformation.AccountNumber;
            specifiedAccount.AccountType = updateAccountInformation.AccountType;
            specifiedAccount.AccountStatus  = updateAccountInformation.AccountStatus;
            specifiedAccount.CurrentBalance = updateAccountInformation.CurrentBalance;
            specifiedAccount.Currency = updateAccountInformation.Currency;
            specifiedAccount.MinimumRequiredBalance = updateAccountInformation.MinimumRequiredBalance;
            specifiedAccount.BranchID = updateAccountInformation.BranchID;
            specifiedAccount.CustomerID = updateAccountInformation.CustomerID;

            await _dbContext.SaveChangesAsync();

            specifiedAccount = await _dbContext.Accounts
                              .Include(a => a.Customer)
                              .Include(a => a.Branch)
                              .FirstAsync(a => a.ID == specifiedAccount.ID);

            return _mapper.Map<Account, AccountToReturnDTO>(specifiedAccount);
        }

        public async Task<AccountToReturnDTO> ActivateAccountAsync(int accountId)
        {
            var specifiedAccount = await _dbContext.Accounts
                                   .Include(a => a.Customer)
                                   .Include(a => a.Branch)
                                  .FirstOrDefaultAsync(a => a.ID == accountId);

            if (specifiedAccount is null)
                throw new NotFoundException("This account isn't exist.");

            if (specifiedAccount.AccountStatus == "Active")
                throw new ValidationException("This account is already active.");

            specifiedAccount.AccountStatus = "Active";

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Account, AccountToReturnDTO>(specifiedAccount);
        }

        public async Task<AccountToReturnDTO> CloseAccountAsync(int accountId)
        {
            var specifiedAccount = await _dbContext.Accounts
                                   .Include(a => a.Customer)
                                   .Include(a => a.Branch)
                                   .FirstOrDefaultAsync(a => a.ID == accountId);

            if (specifiedAccount is null)
                throw new NotFoundException("This account isn't exist.");

            if(specifiedAccount.AccountStatus == "Closed")
                throw new ValidationException("This account is already closed.");

            specifiedAccount.AccountStatus = "Closed";

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Account, AccountToReturnDTO>(specifiedAccount);
        }


        public async Task<AccountToReturnDTO> FreezeAccountAsync(int accountId)
        {
            var specifiedAccount = await _dbContext.Accounts
                                   .Include(a => a.Customer)
                                   .Include(a => a.Branch)
                                   .FirstOrDefaultAsync(a => a.ID == accountId);

            if (specifiedAccount is null)
                throw new NotFoundException("This account isn't exist.");

            if (specifiedAccount.AccountStatus == "Freezed")
                throw new ValidationException("This account is already freezed.");

            specifiedAccount.AccountStatus = "Freezed";

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Account, AccountToReturnDTO>(specifiedAccount);
        }

    }
}
