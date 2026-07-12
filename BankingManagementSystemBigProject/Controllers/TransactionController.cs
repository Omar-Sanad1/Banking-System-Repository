using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Filtering;
using Core.Interfaces;
using Core.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.TransactionModels;

namespace BankingManagementSystemBigProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpGet("GetAllTransactions")]
        public async Task<IActionResult> GetAllTransactionsPagedFiltered([FromQuery] TransactionFiltering transactionFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var transactions = _transactionService.GetAllTransactionsFiltered(t =>
            // فلترة ب TransactionNumber
            (string.IsNullOrEmpty(transactionFiltering.TransactionNumber) || t.TransactionNumber == transactionFiltering.TransactionNumber) &&
            // فلترة ب TransactionDateAndTime
            (!transactionFiltering.TransactionDateAndTime.HasValue || t.TransactionDateAndTime == transactionFiltering.TransactionDateAndTime)
            );

            transactions = transactionFiltering.SortBy?.ToLower() switch
            {
                "transactiondateandtime" => transactionFiltering.isDescending
                ? transactions.OrderByDescending(t => t.TransactionDateAndTime)
                : transactions.OrderBy(t => t.TransactionDateAndTime),

                "referencenumber" => transactionFiltering.isDescending
                ? transactions.OrderByDescending(t => t.ReferenceNumber)
                : transactions.OrderBy(t => t.ReferenceNumber),

                "transactiontumber" => transactionFiltering.isDescending
                ? transactions.OrderByDescending(t => t.TransactionNumber)
                : transactions.OrderBy(t => t.TransactionNumber),

            };

            var totalFilteredTransactions = transactions.Count();

            var result = new PaginationResponse<TransactionToReturnDTO>
                (
                data: transactions,
                totalItems: totalFilteredTransactions,
                pageNumber: paginationParameters.PageNumber,
                pageSize: paginationParameters.PageSize
                );

            return Ok(result);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpGet("GetTransactionByID{id}")]
        public async Task<IActionResult> GetTransactionByIDAsync(int id)
        {
            var transaction = await _transactionService.GetTransactionByIDAsync(id);
            return Ok(transaction);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpGet("GetTransactionByNumber")]
        public async Task<IActionResult> GetTransactionByNumberAsync(string transactionNumber)
        {
            var transaction = await _transactionService.GetTransactionByNumberAsync(transactionNumber);
            return Ok(transaction);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPost("AddNewTransaction")]
        public async Task<IActionResult> AddNewTransactionAsync([FromBody]AddNewTransactionModel addNewTransaction)
        {
            var addedTransaction = await _transactionService.AddNewTransactionAsync(addNewTransaction);
            return Ok(addedTransaction);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPut("UpdateTransactionInformation")]
        public async Task<IActionResult> UpdateTransactionInformationAsync(int transactionID, [FromBody]UpdateTransactionInformationModel updateTransactionInformation)
        {
            var updatedTransaction = await _transactionService.UpdateTransactionInformationAsync(transactionID, updateTransactionInformation);
            return Ok(updatedTransaction);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpDelete("DeleteTransactionByID{id}")]
        public async Task DeleteTransactionByIDAsync(int transactionID)
        {
            await _transactionService.DeleteTransactionByIDAsync(transactionID);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPost("DepositMoney")]
        public async Task<IActionResult> DepositAsync([FromBody]DepositModel depositModel)
        {
           var depositedMoney =  await _transactionService.DepositAsync(depositModel);
            return Ok(new
            {
                Message = depositModel
            });
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPost("WithdrawMoney")]
        public async Task<IActionResult> WithdrawAsync([FromBody] WithdrawModel withdrawModel)
        {
            var withdrawedMoney = await _transactionService.WithdrawAsync(withdrawModel);
            return Ok(new
            {
                Message = withdrawedMoney
            });
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPost("TransferMoney")]
        public async Task<IActionResult> TransferAsync([FromBody] TransferModel transferModel)
        {
            var transferedMoney = await _transactionService.TransferAsync(transferModel);
            return Ok(new
            {
                Message = transferedMoney
            });
        }
    }
}
