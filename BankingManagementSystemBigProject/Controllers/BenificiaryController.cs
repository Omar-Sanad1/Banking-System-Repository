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
    public class BenificiaryController : ControllerBase
    {
        private readonly IGenericRepository<Benificiary> _repo;
        private readonly IMapper _mapper;
        public BenificiaryController(IGenericRepository<Benificiary> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("GetAllBenificiaries")]
        public IActionResult GetAllBenificiariesFilteredPaged([FromQuery] BenificiaryFiltering benificiaryFiltering , [FromQuery] PaginationParameters paginationParameters)
        {
            var benificiaries = _repo.GetAllFiltered(b =>
            // فلترة ب BenificiaryName
            (string.IsNullOrEmpty(benificiaryFiltering.BenificiaryName) || b.BenificiaryName == benificiaryFiltering.BenificiaryName) &&
            // فلترة ب AccountNumber
            (string.IsNullOrEmpty(benificiaryFiltering.AccountNumber) || b.AccountNumber == benificiaryFiltering.AccountNumber)
            );

            benificiaries = benificiaryFiltering.SortBy?.ToLower() switch
            {
                "creationdate" => benificiaryFiltering.isDescending
                ? benificiaries.OrderByDescending(b=>b.CreationDate)
                : benificiaries.OrderBy(b=>b.CreationDate),

                "accountnumber" => benificiaryFiltering.isDescending
                ? benificiaries.OrderByDescending(b=>b.AccountNumber)
                : benificiaries.OrderBy(b=>b.AccountNumber),

                _ => benificiaries.OrderBy(b=>b.ID)
            };

            var totalBenificiariesFiltered = benificiaries.Count();
            var result = new PaginationResponse<BenificiaryToReturnDTO>
                (
                data: _mapper.Map<IEnumerable<Benificiary>, IEnumerable<BenificiaryToReturnDTO>>(benificiaries),
                pageNumber: paginationParameters.PageNumber,
                pageSize: paginationParameters.PageSize,
                totalItems: totalBenificiariesFiltered
                );

            return Ok(result);
        }

        [HttpGet("GetBenificiaryByID{id}")]
        public IActionResult GetBenificiartyByID(int id)
        {
            var benificiary = _repo.GetByID(id);
            var benificiaryMapping = _mapper.Map<Benificiary, BenificiaryToReturnDTO>(benificiary);
            return Ok(benificiaryMapping);
        }

        [HttpPost("Add")]
        public void AddNewBenificiary(Benificiary benificiary)
        {
             _repo.Add(benificiary);
        }

        [HttpPut("Update")]
        public void UpdateBenificiary(Benificiary benificiary)
        {
            _repo.Update(benificiary);
        }


        [HttpDelete("DeleteAccount")]
        public void DeleteBenificiary(Benificiary benificiary)
        {
            _repo.Delete(benificiary);
        }
    }
}
