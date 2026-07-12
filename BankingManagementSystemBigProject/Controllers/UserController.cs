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
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<User> _repo;
        private readonly IMapper _mapper;
        public UserController(IGenericRepository<User> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsersFilteredPaged([FromQuery] UserFiltering userFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var users = _repo.GetAllFiltered(u =>
            // فلترة ب UserName
            (string.IsNullOrEmpty(userFiltering.UserName) || u.UserName == userFiltering.UserName) &&
            // فلترة ب EmailAddress
            (string.IsNullOrEmpty(userFiltering.EmailAddress) || u.EmailAddress == userFiltering.EmailAddress)
            );

            users = userFiltering.SortBy?.ToLower() switch
            {
                "phonenumber" => userFiltering.isDescending
                ? users.OrderByDescending(u => u.PhoneNumber)
                : users.OrderBy(u => u.PhoneNumber),

                "roleid" => userFiltering.isDescending
                ? users.OrderByDescending(u => u.RoleID)
                : users.OrderBy(u => u.RoleID),

                _ => users.OrderBy(u => u.ID)
            };

            var totalFilteredUsers = users.Count();

            var result = new PaginationResponse<UserToReturnDTO>
                (
                data: _mapper.Map<IEnumerable<User>, IEnumerable<UserToReturnDTO>>(users),
                totalItems: totalFilteredUsers,
                pageNumber: paginationParameters.PageNumber,
                pageSize: paginationParameters.PageSize
                );

            return Ok(result);
        }


        [HttpGet("GetUserByID{id}")]
        public IActionResult GetUserByID(int id)
        {
            var user = _repo.GetByID(id);
            var userMapping = _mapper.Map<User, UserToReturnDTO>(user);
            return Ok(userMapping);
        }


        [HttpPost("Add")]
        public void AddNewUser(User user)
        {
            _repo.Add(user);
        }

        [HttpPut("Update")]
        public void UpdateUser(User user)
        {
            _repo.Update(user);
        }


        [HttpDelete("DeleteUser")]
        public void DeleteUser(User user)
        {
            _repo.Delete(user);
        }
    }
}
