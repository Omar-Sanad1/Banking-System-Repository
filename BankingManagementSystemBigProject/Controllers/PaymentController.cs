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
    public class PaymentController : ControllerBase
    {
        private readonly IGenericRepository<Payment> _repo;
        private readonly IMapper _mapper;
        public PaymentController(IGenericRepository<Payment> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("GetAllPayments")]
        public IActionResult GetAllPayments([FromQuery] PaymentFiltering paymentFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var payments = _repo.GetAllFiltered(p =>
            // فلترة ب PaymentDate
            (!paymentFiltering.PaymentDate.HasValue || p.PaymentDate == paymentFiltering.PaymentDate)
            );

            payments = paymentFiltering.SortBy?.ToLower() switch
            {
                "paymentdate" => paymentFiltering.isDescending
                ? payments.OrderByDescending(p => p.PaymentDate)
                : payments.OrderBy(p => p.PaymentDate),

                "paymentamount" => paymentFiltering.isDescending
                ? payments.OrderByDescending(p => p.PaymentAmount)
                : payments.OrderBy(p => p.PaymentAmount),

                _ => payments.OrderBy(p => p.ID)
            };

            var totalFilteredPayments = payments.Count();

            var result = new PaginationResponse<PaymentToReturnDTO>
                (
                data: _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentToReturnDTO>>(payments),
                totalItems: totalFilteredPayments,
                pageNumber: paginationParameters.PageNumber,
                pageSize: paginationParameters.PageSize
                );

            return Ok(result);
        }


        [HttpGet("GetPaymentByID{id}")]
        public IActionResult GetPaymentByID(int id)
        {
            var payment = _repo.GetByID(id);
            var paymentMapping = _mapper.Map<Payment, PaymentToReturnDTO>(payment);
            return Ok(paymentMapping);
        }


        [HttpPost("Add")]
        public void AddNewPayment(Payment payment)
        {
            _repo.Add(payment);
        }

        [HttpPut("Update")]
        public void UpdatePayment(Payment payment)
        {
            _repo.Update(payment);
        }


        [HttpDelete("DeletePayment")]
        public void DeletePayment(Payment payment)
        {
            _repo.Delete(payment);
        }
    }
}
