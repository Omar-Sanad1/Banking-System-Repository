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
    public class CardController : ControllerBase
    {
        private readonly IGenericRepository<Card> _repo;
        private readonly IMapper _mapper;
        public CardController(IGenericRepository<Card> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("GetAllCards")]
        public IActionResult GetAllCardsFilteredPaged([FromQuery] CardFiltering cardFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var cards = _repo.GetAllFiltered(c =>
            // فلترة ب CardNumber
            (string.IsNullOrEmpty(cardFiltering.CardNumber) || c.CardNumber == cardFiltering.CardNumber)
            );

            cards = cardFiltering.SortBy?.ToLower() switch
            {
                "issuedate" => cardFiltering.isDescending 
                ? cards.OrderByDescending(c => c.IssueDate)
                : cards.OrderBy(c => c.IssueDate),

                "expirationdate" => cardFiltering.isDescending 
                ? cards.OrderByDescending(c => c.ExpirationDate)
                : cards.OrderBy(c => c.ExpirationDate),

                _ => cards.OrderBy(c => c.ID)
            };

            var totalFilteredCards = cards.Count();

            var result = new PaginationResponse<CardToReturnDTO>
                (
                data: _mapper.Map<IEnumerable<Card>, IEnumerable<CardToReturnDTO>>(cards),
                totalItems: totalFilteredCards,
                pageNumber: paginationParameters.PageNumber,
                pageSize: paginationParameters.PageSize
                );

            return Ok(result);
        }


        [HttpGet("GetCardByID{id}")]
        public IActionResult GetCardByID(int id)
        {
            var card = _repo.GetByID(id);
            var cardMapping = _mapper.Map<Card, CardToReturnDTO>(card);
            return Ok(cardMapping);
        }


        [HttpPost("Add")]
        public void AddNewCard(Card card)
        {
            _repo.Add(card);
        }

        [HttpPut("Update")]
        public void UpdateCard(Card card)
        {
            _repo.Update(card);
        }


        [HttpDelete("DeleteCard")]
        public void DeleteCard(Card card)
        {
            _repo.Delete(card);
        }
    }
}
