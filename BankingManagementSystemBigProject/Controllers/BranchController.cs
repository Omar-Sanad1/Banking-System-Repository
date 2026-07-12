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
using Service.Models.BranchModels;

namespace BankingManagementSystemBigProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpGet("GetAllBranches")]
        public async Task<IActionResult> GetAllBranchesFilteredPaged([FromQuery] BranchFiltering branchFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var branches = _branchService.GetAllBranchesFiltered(b =>
            // فلترة ب Address
            (string.IsNullOrEmpty(branchFiltering.Address) || b.Address == branchFiltering.Address) 
            );

            branches = branchFiltering.SortBy?.ToLower() switch
            {
                "name" => branchFiltering.isDescending ?
                branches.OrderByDescending(b => b.Name)
                : branches.OrderBy(b => b.Name),

                "operatighours" => branchFiltering.isDescending ?
                branches.OrderByDescending(b => b.OperatigHours)
                : branches.OrderBy(b => b.OperatigHours),

            };

            var totalFilteredBranches = branches.Count();

            var result = new PaginationResponse<BranchToReturnDTO>
                (
                data: branches,
                totalItems: totalFilteredBranches,
                pageNumber: paginationParameters.PageNumber,
                pageSize: paginationParameters.PageSize
                );

            return Ok(result);
        }


        [Authorize(Roles = "Admin , Employee")]
        [HttpGet("GetBranchByID{id}")]
        public async Task<IActionResult> GetBranchByIDAsync(int id)
        {
            var branch = await _branchService.GetBranchByIDAsync(id);
            return Ok(branch);
        }

        [HttpGet("GetBranchByEmailAddress")]
        public async Task<IActionResult> GetBranchByEmailAddressAsync(string emailAddress)
        {
            var branch = await _branchService.GetBranchByEmailAddressAsync(emailAddress);
            return Ok(branch);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPost("AddNewBranch")]
        public async Task<IActionResult> AddNewBranchAsync([FromBody]AddNewBranchModel addNewBranch)
        {
            var addedBranch = await _branchService.AddNewBranchAsync(addNewBranch);
            return Ok(addedBranch);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPut("UpdateBranchInformation")]
        public async Task<IActionResult> UpdateBranchInformationAsync(int branchID,[FromBody]UpdateBranchInformationModel updateBranchInformation)
        {
            var updatedBranch = await _branchService.UpdateBranchInformationAsync(branchID , updateBranchInformation);
            return Ok(updatedBranch);

        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpDelete("DeleteBranchByID")]
        public async Task DeleteBranchByIDAsync(int branchID)
        {
            await _branchService.DeleteBranchByIDAsync(branchID);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPost("ActivateBranch")]
        public async Task<IActionResult> ActivateBranchAsync(int branchID)
        {
            var activatedBranch = await _branchService.ActivateBranchAsync(branchID);
            return Ok(activatedBranch);
        }

        [Authorize(Roles = "Admin , Employee")]
        [HttpPost("DeActivateBranch")]
        public async Task<IActionResult> DeActivateBranchAsync(int branchID)
        {
            var deActivatedBranch = await _branchService.DeActivateBranchAsync(branchID);
            return Ok(deActivatedBranch);
        }
    }
}
