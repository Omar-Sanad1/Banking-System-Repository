using BankingManagementSystemBigProject.Models;
using BankingManagementSystemBigProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingManagementSystemBigProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("RegisterCustomer")]
        public async Task<IActionResult> RegisterCustomerAsync([FromBody]RegisterCustomerModel registerCustomer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var registeredCustomer = await _authService.RegisterCustomerAsync(registerCustomer);
            return Ok(registeredCustomer);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var loginedUser = await _authService.LoginAsync(loginModel);
            return Ok(loginedUser);
        }


    }
}
