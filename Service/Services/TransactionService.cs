using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.TransactionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly BankingSystemDbContext _dbContext;
        private readonly IMapper _mapper;
        public TransactionService(BankingSystemDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<TransactionToReturnDTO> GetAllTransactionsFiltered(Func<Transaction, bool> Filter)
        {
            var transactionsFiltered = _dbContext.Transactions
                                       .Where(Filter)
                                       .ToList();

            return _mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionToReturnDTO>>(transactionsFiltered);
        }

        public async Task<IEnumerable<TransactionToReturnDTO>> GetAllTransactionsPagedAsync(int pageNumber, int pageSize)
        {
            var transactionsPaged = await _dbContext.Transactions
                                       .Skip((pageNumber - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

            return _mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionToReturnDTO>>(transactionsPaged);
        }

        public async Task<TransactionToReturnDTO> GetTransactionByIDAsync(int transactionID)
        {
            var specificTransaction = await _dbContext.Transactions.FirstOrDefaultAsync(t=>t.ID == transactionID);
            if (specificTransaction is null)
                throw new NotFoundException("This transaction isn't exist.");

            return _mapper.Map<Transaction, TransactionToReturnDTO>(specificTransaction);
        }

        public async Task<TransactionToReturnDTO> GetTransactionByNumberAsync(string transactionNumber)
        {
            var specificTransaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.TransactionNumber == transactionNumber);
            if (specificTransaction is null)
                throw new NotFoundException("This transaction isn't exist.");

            return _mapper.Map<Transaction, TransactionToReturnDTO>(specificTransaction);
        }
        public async Task<TransactionToReturnDTO> AddNewTransactionAsync(AddNewTransactionModel addNewTransaction)
        {
            var existsNumber = await _dbContext.Transactions.FirstOrDefaultAsync(t=>t.TransactionNumber == addNewTransaction.TransactionNumber);
            if(existsNumber is not null)
                throw new Exception("This transaction is already exist.");

            var existsReferenceNumber = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.ReferenceNumber == addNewTransaction.ReferenceNumber);
            if (existsReferenceNumber is not null)
                throw new Exception("This transaction is already exist.");

            var transaction = new Transaction
            {
                TransactionNumber = addNewTransaction.TransactionNumber,
                TransactionType = addNewTransaction.TransactionType,
                TransactionAmount = addNewTransaction.TransactionAmount,
                TransactionDateAndTime = addNewTransaction.TransactionDateAndTime,
                TransactionDescription = addNewTransaction.TransactionDescription,
                TransactionStatus = addNewTransaction.TransactionStatus,
                ReferenceNumber = addNewTransaction.ReferenceNumber,
                AccountID = addNewTransaction.AccountID,
            };

            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Transaction, TransactionToReturnDTO>(transaction);
        }

        public async Task DeleteTransactionByIDAsync(int transactionID)
        {
            var specificTransaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.ID == transactionID);
            if (specificTransaction is null)
                throw new NotFoundException("This transaction isn't exist.");

            _dbContext.Transactions.Remove(specificTransaction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TransactionToReturnDTO> UpdateTransactionInformationAsync(int transactionID, UpdateTransactionInformationModel updateTransactionInformation)
        {
            var specificTransaction = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.ID == transactionID);
            if (specificTransaction is null)
                throw new NotFoundException("This transaction isn't exist.");

            specificTransaction.TransactionNumber = updateTransactionInformation.TransactionNumber;
            specificTransaction.TransactionType = updateTransactionInformation.TransactionType;
            specificTransaction.TransactionAmount = updateTransactionInformation.TransactionAmount;
            specificTransaction.TransactionDateAndTime = updateTransactionInformation.TransactionDateAndTime;
            specificTransaction.TransactionDescription = updateTransactionInformation.TransactionDescription;
            specificTransaction.TransactionStatus = updateTransactionInformation.TransactionStatus;
            specificTransaction.ReferenceNumber = updateTransactionInformation.ReferenceNumber;
            specificTransaction.AccountID = updateTransactionInformation.AccountID;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Transaction, TransactionToReturnDTO>(specificTransaction);
        }


        public async Task<string> DepositAsync(DepositModel depositModel)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                if (depositModel.Amount < 0)
                    throw new Exception("Amount must be greater than zero.");

                var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.ID == depositModel.AccountID);
                if(account is null)
                    throw new Exception("This account isn't exist.");

                if (account.AccountStatus != "Active")
                    throw new ValidationException("This account isn't active.");

                account.CurrentBalance += depositModel.Amount;

                var Banktransaction = new Transaction
                {
                    TransactionNumber = Guid.NewGuid().ToString(),
                    TransactionType = "Deposit",
                    TransactionAmount = depositModel.Amount,
                    TransactionDateAndTime = DateTime.Now,
                    TransactionDescription = depositModel.Description,
                    TransactionStatus = "Completed",
                    ReferenceNumber = Guid.NewGuid().ToString("N")[..10] ,
                    AccountID = account.ID,
                };

                await _dbContext.Transactions.AddAsync(Banktransaction);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return "Deposit completed successfully";

            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<string> WithdrawAsync(WithdrawModel withdrawModel)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                if (withdrawModel.Amount < 0)
                    throw new Exception("Amount must be greater than zero");

                var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.ID == withdrawModel.AccountID);
                if (account is null)
                    throw new NotFoundException("This account isn't exist.");

                if (account.AccountStatus != "Active")
                    throw new ValidationException("This account isn't active.");

                account.CurrentBalance -= withdrawModel.Amount;

                var Banktransaction = new Transaction
                {
                    TransactionNumber = Guid.NewGuid().ToString(),
                    TransactionType = "Withdraw",
                    TransactionStatus = "Completed",
                    TransactionDateAndTime = DateTime.Now,
                    TransactionDescription = withdrawModel.Description,
                    TransactionAmount = withdrawModel.Amount,
                    ReferenceNumber = Guid.NewGuid().ToString("N")[..10],
                    AccountID = account.ID,
                };

                await _dbContext.Transactions.AddAsync(Banktransaction);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return "Withdraw completed successfully";
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ex.Message;
            }
           

        }
        

        public async Task<string> TransferAsync(TransferModel transferModel)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                if (transferModel.Amount < 0)
                    throw new Exception("Amount must be greater than zero");

                var fromAccount = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.ID == transferModel.SourceAccountID);
                if (fromAccount is null)
                    throw new NotFoundException("This account isn't exist.");

                var ToAccount = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.ID == transferModel.DestinationAccountID);
                if (ToAccount is null)
                    throw new NotFoundException("This account isn't exist.");

                if (fromAccount.AccountStatus != "Active")
                    throw new ValidationException("This account isn't active");

                if (ToAccount.AccountStatus != "Active")
                    throw new ValidationException("This account isn't active");

                if (fromAccount.CurrentBalance - transferModel.Amount < fromAccount.MinimumRequiredBalance)
                    throw new Exception("Not enough balance.");

                fromAccount.CurrentBalance -= transferModel.Amount;
                ToAccount.CurrentBalance += transferModel.Amount;

                var Banktransaction = new Transaction
                {
                    TransactionNumber = Guid.NewGuid().ToString(),
                    TransactionType = "Transfer",
                    TransactionStatus = "Completed",
                    TransactionDateAndTime = DateTime.Now,
                    TransactionDescription = transferModel.Description,
                    TransactionAmount = transferModel.Amount,
                    ReferenceNumber = Guid.NewGuid().ToString("N")[..10],
                    AccountID = fromAccount.ID,
                };

                await _dbContext.Transactions.AddAsync(Banktransaction);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return "Transfer completed successfully";
            }

            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return ex.Message;
            }
        }


    }
}
