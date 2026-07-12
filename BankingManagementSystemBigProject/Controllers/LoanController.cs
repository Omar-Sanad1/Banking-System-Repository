using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Filtering;
using Core.Interfaces;
using Core.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingManagementSystemBigProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly IGenericRepository<Loan> _repo;
        private readonly IMapper _mapper;
        public LoanController(IGenericRepository<Loan> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("GetAllLoans")]
        public IActionResult GetAllLoans([FromQuery] LoanFiltering loanFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var loans = _repo.GetAllFiltered(l =>
            // فلترة ب LoanType
            (string.IsNullOrEmpty(loanFiltering.LoanType) || l.LoanType == loanFiltering.LoanType) &&
            // فلترة ب ApplicationDate
            (!loanFiltering.ApplicationDate.HasValue || l.ApplicationDate == loanFiltering.ApplicationDate)
            );

            loans = loanFiltering.SortBy?.ToLower() switch
            {
                "requestedamount" => loanFiltering.isDescending
                ? loans.OrderByDescending(l => l.RequestedAmount)
                : loans.OrderBy(l => l.RequestedAmount),

                "applicationdate" => loanFiltering.isDescending
                ? loans.OrderByDescending(l => l.ApplicationDate)
                : loans.OrderBy(l => l.ApplicationDate),

                "loantype" => loanFiltering.isDescending
                ? loans.OrderByDescending(l => l.LoanType)
                : loans.OrderBy(l => l.LoanType),

                _ => loans.OrderBy(l => l.ID)
            };

            var totalFilteredLoans = loans.Count();

            var result = new PaginationResponse<LoanToReturnDTO>
                (
                data: _mapper.Map<IEnumerable<Loan>, IEnumerable<LoanToReturnDTO>>(loans),
                totalItems: totalFilteredLoans,
                pageNumber: paginationParameters.PageNumber,
                pageSize: paginationParameters.PageSize
                );

            return Ok(result);
        }


        [HttpGet("GetLoanByID{id}")]
        public IActionResult GetLoanByID(int id)
        {
            var loan = _repo.GetByID(id);
            var loanMapping = _mapper.Map<Loan, LoanToReturnDTO>(loan);
            return Ok(loanMapping);
        }


        [HttpPost("Add")]
        public void AddNewLoan(Loan loan)
        {
            _repo.Add(loan);
        }

        [HttpPut("Update")]
        public void UpdateLoan(Loan loan)
        {
            _repo.Update(loan);
        }


        [HttpDelete("DeleteLoan")]
        public void DeleteLoan(Loan loan)
        {
            _repo.Delete(loan);
        }
    }
}
