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
using Service.Models.EmployeeModels;

namespace BankingManagementSystemBigProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
           _employeeService = employeeService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployeesFilteredPaged([FromQuery] EmployeeFiltering employeeFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var employees = _employeeService.GetAllEmployeesFiltered(e =>
            // فلترة ب FullName
            (string.IsNullOrEmpty(employeeFiltering.FullName) || e.FullName == employeeFiltering.FullName) &&
            // فلترة ب NationalID
            (string.IsNullOrEmpty(employeeFiltering.NationalID) || e.NationalID == employeeFiltering.NationalID)
            );

            employees = employeeFiltering.SortBy?.ToLower() switch
            {
                "dateofbirth" => employeeFiltering.isDescending
                ? employees.OrderByDescending(e => e.DateOfBirth)
                : employees.OrderBy(e => e.DateOfBirth),

                "hiringdate" => employeeFiltering.isDescending
                ? employees.OrderByDescending(e => e.HiringDate)
                : employees.OrderBy(e => e.HiringDate),

                "salary" => employeeFiltering.isDescending
                ? employees.OrderByDescending(e => e.Salary)
                : employees.OrderBy(e => e.Salary),

                "gender" => employeeFiltering.isDescending
                ? employees.OrderByDescending(e => e.Gender)
                : employees.OrderBy(e => e.Gender),

            };

            var totalFilteredEmployees = employees.Count();

            var result = new PaginationResponse<EmployeeToReturnDTO>
                (
                data: employees,
                totalItems: totalFilteredEmployees,
                pageNumber: paginationParameters.PageNumber,
                pageSize: paginationParameters.PageSize
                );

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetEmployeeByID{id}")]
        public async Task<IActionResult> GetEmployeeByIDAsync(int id)
        {
            var employee = await _employeeService.GetEmployeeByIDAsync(id);
            return Ok(employee);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetEmployeeByNationalID{id}")]
        public async Task<IActionResult> GetEmployeeByNationalIDAsync(string nationalID)
        {
            var employee = await _employeeService.GetEmployeeByNationalIDAsync(nationalID);
            return Ok(employee);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddNewEmployee")]
        public async Task<IActionResult> AddNewEmployeeAsync([FromBody]AddNewEmployeeModel addNewEmployee)
        {
            var addedEmployee = await _employeeService.AddNewEmployeeAsync(addNewEmployee);
            return Ok(addedEmployee);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateEmployeeInformation")]
        public async Task<IActionResult> UpdateEmployeeInformationAsync(int employeeID,[FromBody]UpdateEmployeeInformationModel updateEmployeeInformation)
        {
            var updatedEmployee = await _employeeService.UpdateEmployeeInformationAsync(employeeID, updateEmployeeInformation);
            return Ok(updatedEmployee);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteEmployeeByID")]
        public async Task DeleteEmployeeByIDAsync(int employeeID)
        {
           await _employeeService.DeleteEmployeeByIDAsync(employeeID);
        }
    }
}
