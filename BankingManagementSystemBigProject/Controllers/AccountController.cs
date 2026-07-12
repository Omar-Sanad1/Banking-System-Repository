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
using Service.Models.AccountModels;

namespace BankingManagementSystemBigProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpGet("GetAllAccounts")]
        public async Task<IActionResult> GetAllAccountsPagedFilteredAsync([FromQuery] AccountFiltering accountFiltering,[FromQuery] PaginationParameters paginationParameters)
        {
            var accounts = _accountService.GetAllAccountsFiltered(a =>
            // فلترة ب AccountNumber
            (string.IsNullOrEmpty(accountFiltering.AccountNumber) || a.AccountNumber == accountFiltering.AccountNumber) &&
            // فلترة ب OpeningDate
            (!accountFiltering.OpeningDate.HasValue || a.OpeningDate == accountFiltering.OpeningDate) &&
            // فلترة ب OpeningDate
            (!accountFiltering.CustomerID.HasValue || a.CustomerID == accountFiltering.CustomerID) 
            );

            accounts = accountFiltering.SortBy?.ToLower() switch
            {
                "accountnumber" => accountFiltering.isDescending?
                accounts.OrderByDescending(a=>a.AccountNumber)
                : accounts.OrderBy(a=>a.AccountNumber),

                "openingdate" => accountFiltering.isDescending ?
                accounts.OrderByDescending(a => a.OpeningDate)
                : accounts.OrderBy(a => a.OpeningDate),

                "accountstatus" => accountFiltering.isDescending ?
               accounts.OrderByDescending(a => a.AccountStatus)
               : accounts.OrderBy(a => a.AccountStatus),

            };

            var totalFilteredAccounts = accounts.Count();

            var result = new PaginationResponse<AccountToReturnDTO>
                (
                data: accounts,
                totalItems: totalFilteredAccounts,
                pageNumber: paginationParameters.PageNumber,
                pageSize: paginationParameters.PageSize
                );

            return Ok(result);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpGet("GetAccountByID{id}")]
        public async Task<IActionResult> GetAccountByIDAsync(int id)
        {
            var account = await _accountService.GetAccountByIDAsync(id);
            return Ok(account);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpGet("GetAccountByNumber")]
        public async Task<IActionResult> GetAccountByNumberAsync(string accountNumber)
        {
            var account = await _accountService.GetAcountByNumberAsync(accountNumber);
            return Ok(account);
        }

        [Authorize(Roles = "Admin , Employee , Customer")]
        [HttpGet("GetAccountBalance")]
        public async Task<decimal> GetAccountBalanceByAccountIDAsync(int accountId)
        {
            var accountBalance = await _accountService.GetAccountBalanceByAccountIDAsync(accountId);
            return accountBalance;
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPost("AddNewAccount")]
        public async Task<IActionResult> AddNewAccount([FromBody]AddNewAccountModel addNewAccount)
        {
            var addedAccount = await _accountService.AddNewAccountAsync(addNewAccount);
            return Ok(addedAccount);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPut("UpdateAccountInformation")]
        public async Task<IActionResult> UpdateAccountInformationAsync(int accountID,[FromBody]UpdateAccountInformationModel updateAccountInformation)
        {
            var updatedAccount = await _accountService.UpdateAccountInformationAsync(accountID, updateAccountInformation);
            return Ok(updatedAccount);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpDelete("DeleteAccount")]
        public async Task DeleteAccountAsync(int accountID)
        {
            await _accountService.DeleteAccountByIDAsync(accountID);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPut("CloseAccount")]
        public async Task<IActionResult> CloseAccountAsync(int accountId)
        {
            var closedAccount = await _accountService.CloseAccountAsync(accountId);
            return Ok(closedAccount);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPut("FreezeAccount")]
        public async Task<IActionResult> FreezeAccountAsync(int accountId)
        {
            var freezedAccount = await _accountService.FreezeAccountAsync(accountId);
            return Ok(freezedAccount);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPut("ActivateAccount")]
        public async Task<IActionResult> ActivateAccountAsync(int accountId)
        {
            var activatedAccount = await _accountService.ActivateAccountAsync(accountId);
            return Ok(activatedAccount);
        }
    }
}
