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
using Service.Models.CustomerModels;

namespace BankingManagementSystemBigProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("GetAllCustomers")]
        public IActionResult GetAllCustomersPagedFiltered([FromQuery] CustomerFiltering customerFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var customers = _customerService.GetAllCustomersFiltered(c =>
            // فلترة ب FullName
            (string.IsNullOrEmpty(customerFiltering.FullName) || c.FullName == customerFiltering.FullName) &&
            // فلترة ب NationalID
            (string.IsNullOrEmpty(customerFiltering.NationalID) || c.NationalID == customerFiltering.NationalID)
            );

            customers = customerFiltering.SortBy?.ToLower() switch
            {
                "dateofbirth" => customerFiltering.isDescending 
                ? customers.OrderByDescending(c => c.DateOfBirth)
                : customers.OrderBy(c => c.DateOfBirth),

                "registerationdate" => customerFiltering.isDescending
                ? customers.OrderByDescending(c => c.RegisterationDate)
                : customers.OrderBy(c => c.RegisterationDate),

                "gender" => customerFiltering.isDescending
                ? customers.OrderByDescending(c => c.Gender)
                : customers.OrderBy(c => c.Gender),
            };

            var totalFilteredCustomers = customers.Count();

            var result = new PaginationResponse<CustomerToReturnDTO>
                (
                data: customers,
                totalItems: totalFilteredCustomers,
                pageNumber: paginationParameters.PageNumber,
                pageSize: paginationParameters.PageSize
                );

            return Ok(result);
        }

        [Authorize(Roles = "Employee , Admin")]
        [HttpGet("GetCustomerByID{id}")]
        public async Task<IActionResult> GetCustomerByIDAsync(int customerID)
        {
            var customer = await _customerService.GetCustomerByIDAsync(customerID);
            return Ok(customer);
        }

        [Authorize(Roles = "Employee , Admin")]
        [HttpGet("GetCustomerByNationalID{id}")]
        public async Task<IActionResult> GetCustomerByNationalIDAsync(string nationalID)
        {
            var customer = await _customerService.GetCustomerByNationalIDAsync(nationalID);
            return Ok(customer);
        }

        [Authorize(Roles = "Employee , Admin")]
        [HttpPut("UpdateCustomerInformation")]
        public async Task<IActionResult> UpdateCustomerInformationAsync(int customerID, [FromBody]UpdateCustomerInformationModel updateCustomerInformation)
        {
            var updatedCustomer = await _customerService.UpdateCustomerInformationAsync(customerID, updateCustomerInformation);
            return Ok(updatedCustomer);
        }

        [Authorize(Roles = "Employee , Admin")]
        [HttpDelete("DeleteCustomer")]
        public async Task DeleteCustomerByIDAsync(int customerID)
        {
            await _customerService.DeleteCustomerByIDAsync(customerID);
        }
    }
}
