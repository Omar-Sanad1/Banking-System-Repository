using Core.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Models.TransactionModels;

namespace Service.Interfaces
{
    public interface ITransactionService
    {
        public Task<IEnumerable<TransactionToReturnDTO>> GetAllTransactionsPagedAsync(int pageNumber,int pageSize);
        public IEnumerable<TransactionToReturnDTO> GetAllTransactionsFiltered(Func<Transaction, bool> Filter);
        public Task<TransactionToReturnDTO> GetTransactionByIDAsync(int transactionID);
        public Task<TransactionToReturnDTO> GetTransactionByNumberAsync(string transactionNumber);
        public Task<TransactionToReturnDTO> AddNewTransactionAsync(AddNewTransactionModel addNewTransaction);
        public Task<TransactionToReturnDTO> UpdateTransactionInformationAsync(int transactionID,UpdateTransactionInformationModel updateTransactionInformation);
        public Task DeleteTransactionByIDAsync(int transactionID);
        public Task<string> DepositAsync(DepositModel depositModel);
        public Task<string> WithdrawAsync(WithdrawModel withdrawModel);
        public Task<string> TransferAsync(TransferModel transferModel);

    }
}
