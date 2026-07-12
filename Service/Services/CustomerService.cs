using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly BankingSystemDbContext _dbContext;
        private readonly IMapper _mapper;
        public CustomerService(BankingSystemDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerToReturnDTO>> GetAllCustomersPagedAsync(int pageNumber, int pageSize)
        {
            var customersPaged = await _dbContext.Customers
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();

            return _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerToReturnDTO>>(customersPaged);
        }

        public IEnumerable<CustomerToReturnDTO> GetAllCustomersFiltered(Func<Customer, bool> Filter)
        {
            var customersFiltered = _dbContext.Customers
                                    .Where(Filter)
                                    .ToList();

            return _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerToReturnDTO>>(customersFiltered);
        }

        public async Task<CustomerToReturnDTO> GetCustomerByIDAsync(int customerID)
        {
            var specifiedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c=>c.ID == customerID);
            if (specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            return _mapper.Map<Customer, CustomerToReturnDTO>(specifiedCustomer);
        }

        public async Task<CustomerToReturnDTO> GetCustomerByNationalIDAsync(string nationalID)
        {
            var specifiedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c=>c.NationalID == nationalID);
            if (specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            return _mapper.Map<Customer, CustomerToReturnDTO>(specifiedCustomer);
        }


        public async Task DeleteCustomerByIDAsync(int customerID)
        {
            var specifiedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.ID == customerID);
            if (specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            _dbContext.Customers.Remove(specifiedCustomer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CustomerToReturnDTO> UpdateCustomerInformationAsync(int customerID, UpdateCustomerInformationModel updateCustomerInformation)
        {
            var specifiedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c=>c.ID == customerID);
            if(specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            specifiedCustomer.FullName = updateCustomerInformation.FullName;
            specifiedCustomer.Occuption = updateCustomerInformation.Occuption;
            specifiedCustomer.DateOfBirth = updateCustomerInformation.DateOfBirth;
            specifiedCustomer.Gender = updateCustomerInformation.Gender;
            specifiedCustomer.NationalID = updateCustomerInformation.NationalID;
            specifiedCustomer.ResidintialAddress = updateCustomerInformation.ResidintialAddress;
            specifiedCustomer.UserID = updateCustomerInformation.UserID;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Customer, CustomerToReturnDTO>(specifiedCustomer);
        }
    }
}
