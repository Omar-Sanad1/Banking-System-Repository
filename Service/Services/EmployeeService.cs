using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.EmployeeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly BankingSystemDbContext _dbContext;
        private readonly IMapper _mapper;
        public EmployeeService(BankingSystemDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<EmployeeToReturnDTO>> GetAllEmployeesPagedAsync(int pageNumber, int pageSize)
        {
            var employeesPaged = await _dbContext.Employees
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();

            return _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeToReturnDTO>>(employeesPaged);
        }

        public IEnumerable<EmployeeToReturnDTO> GetAllEmployeesFiltered(Func<Employee, bool> Filter)
        {
            var employeesFiltered = _dbContext.Employees
                                    .Where(Filter)
                                    .ToList();

            return _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeToReturnDTO>>(employeesFiltered);
        }

        public async Task<EmployeeToReturnDTO> GetEmployeeByIDAsync(int employeeID)
        {
            var specifiedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.ID == employeeID);
            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            return _mapper.Map<Employee, EmployeeToReturnDTO>(specifiedEmployee);
        }

        public async Task<EmployeeToReturnDTO> GetEmployeeByNationalIDAsync(string nationalID)
        {
            var specifiedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.NationalID == nationalID);
            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            return _mapper.Map<Employee, EmployeeToReturnDTO>(specifiedEmployee);
        }

        public async Task<string> AddNewEmployeeAsync(AddNewEmployeeModel addNewEmployee)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var existsEmployee = await _dbContext.Users.FirstOrDefaultAsync(e => e.EmailAddress == addNewEmployee.EmailAddress);
                if (existsEmployee is not null)
                    throw new ValidationException("This employee is already exist.");

                var existsUserName = await _dbContext.Users.FirstOrDefaultAsync(e => e.UserName == addNewEmployee.UserName);
                if (existsUserName is not null)
                    throw new ValidationException("This employee is already exist.");

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(addNewEmployee.PasswordHash);

                var user = new User
                {
                    UserName = addNewEmployee.UserName,
                    EmailAddress = addNewEmployee.EmailAddress,
                    PhoneNumber = addNewEmployee.PhoneNumber,
                    PasswordHash = hashedPassword,
                    RoleID = 2
                };

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                var employee = new Employee
                {
                    FullName = addNewEmployee.FullName,
                    NationalID = addNewEmployee.NationalID,
                    Gender = addNewEmployee.Gender,
                    DateOfBirth = addNewEmployee.DateOfBirth,
                    HiringDate = addNewEmployee.HiringDate,
                    Address = addNewEmployee.Address,
                    Position = addNewEmployee.Position,
                    Salary = addNewEmployee.Salary,
                    Status = "Active",
                    BranchID = addNewEmployee.BranchID,
                    UserID = user.ID
                };


                await _dbContext.Employees.AddAsync(employee);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return "Employee Added Successfully.";

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task DeleteEmployeeByIDAsync(int employeeID)
        {
            var specifiedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.ID == employeeID);
            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            _dbContext.Employees.Remove(specifiedEmployee);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<EmployeeToReturnDTO> UpdateEmployeeInformationAsync(int employeeID, UpdateEmployeeInformationModel updateEmployeeInformation)
        {
            var specifiedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.ID == employeeID);
            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            string[]validStatuses = {"Active" , "Inactive"};

            specifiedEmployee.FullName = updateEmployeeInformation.FullName;
            specifiedEmployee.NationalID = updateEmployeeInformation.NationalID;
            specifiedEmployee.Gender = updateEmployeeInformation.Gender;
            specifiedEmployee.DateOfBirth = updateEmployeeInformation.DateOfBirth;
            specifiedEmployee.Address = updateEmployeeInformation.Address;
            specifiedEmployee.HiringDate = updateEmployeeInformation.HiringDate;
            specifiedEmployee.Position = updateEmployeeInformation.Position;
            specifiedEmployee.Salary = updateEmployeeInformation.Salary;

            if (!validStatuses.Contains(updateEmployeeInformation.Status))
                throw new Exception("This status isn't valid. Valid statuses : (Active , Inactive)");
            specifiedEmployee.Status = updateEmployeeInformation.Status;

            specifiedEmployee.BranchID = updateEmployeeInformation.BranchID;
            specifiedEmployee.UserID = updateEmployeeInformation.UserID;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Employee, EmployeeToReturnDTO>(specifiedEmployee);
        }
    }
}
