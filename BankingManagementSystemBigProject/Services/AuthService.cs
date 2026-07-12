using AutoMapper;
using BankingManagementSystemBigProject.CreateTokenService;
using BankingManagementSystemBigProject.Models;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

namespace BankingManagementSystemBigProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly BankingSystemDbContext _dbContext;
        private readonly ITokenService _tokenService;
        public AuthService(BankingSystemDbContext dbContext,ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }
        public async Task<string> RegisterCustomerAsync(RegisterCustomerModel registerCustomer)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var existsCustomer = await _dbContext.Users.FirstOrDefaultAsync(u => u.EmailAddress == registerCustomer.EmailAddress);
                if (existsCustomer is not null)
                    throw new ValidationException("This email is already exists.");

                var existsUserName = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == registerCustomer.UserName);
                if (existsUserName is not null)
                    throw new ValidationException("This username is already exists.");

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerCustomer.Password);

                var user = new User
                {
                    UserName = registerCustomer.UserName,
                    EmailAddress = registerCustomer.EmailAddress,
                    PhoneNumber = registerCustomer.PhoneNumber,
                    PasswordHash = hashedPassword,
                    RoleID = 3
                };

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                var customer = new Customer
                {
                    FullName = registerCustomer.FullName,
                    NationalID = registerCustomer.NationalID,
                    Gender = registerCustomer.Gender,
                    DateOfBirth = registerCustomer.DateOfBirth,
                    ResidintialAddress = registerCustomer.ResidintialAddress,
                    Occuption = registerCustomer.Occuption,
                    RegisterationDate = DateTime.Now,
                    AccountStatus = "Active",
                    UserID = user.ID
                };

                await _dbContext.Customers.AddAsync(customer);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return "Customer Registered Successfully.";
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return ex.Message;
            }
        }


        public async Task<string> LoginAsync(LoginModel loginModel)
        {
            var checkEmail = await _dbContext.Users
                             .Include(u=>u.Role)
                             .FirstOrDefaultAsync(u => u.EmailAddress == loginModel.EmailAddress);
            if (checkEmail is null)
                throw new ValidationException("The emailaddress or password isn't correct.");

            var verifyPassword = BCrypt.Net.BCrypt.Verify(loginModel.Password, checkEmail.PasswordHash);

            if(!verifyPassword)
                throw new ValidationException("The emailaddress or password isn't correct.");

            var token = await _tokenService.CreateTokenAsync(checkEmail);

            return $"The Token : {token}";
        }

    }
}
