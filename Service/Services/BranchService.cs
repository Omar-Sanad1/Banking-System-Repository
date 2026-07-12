using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.BranchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BranchService : IBranchService
    {
        private readonly BankingSystemDbContext _dbContext;
        private readonly IMapper _mapper;
        public BranchService(BankingSystemDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public IEnumerable<BranchToReturnDTO> GetAllBranchesFiltered(Func<Branch, bool> Filter)
        {
            var branchesFiltered = _dbContext.Branches
                                   .Where(Filter)
                                   .ToList();

            return _mapper.Map<IEnumerable<Branch>, IEnumerable<BranchToReturnDTO>>(branchesFiltered);
        }

        public async Task<IEnumerable<BranchToReturnDTO>> GetAllBranchesPagedAsync(int pageNumber, int pageSize)
        {
            var branchesPaged = await _dbContext.Branches
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            return _mapper.Map<IEnumerable<Branch>, IEnumerable<BranchToReturnDTO>>(branchesPaged);
        }

        public async Task<BranchToReturnDTO> GetBranchByEmailAddressAsync(string emailAddress)
        {
            var specificBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.EmailAddress == emailAddress);
            if (specificBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            return _mapper.Map<Branch, BranchToReturnDTO>(specificBranch);
        }

        public async Task<BranchToReturnDTO> GetBranchByIDAsync(int branchID)
        {
            var specificBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == branchID);
            if (specificBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            return _mapper.Map<Branch, BranchToReturnDTO>(specificBranch);
        }
        public async Task<BranchToReturnDTO> AddNewBranchAsync(AddNewBranchModel addNewBranch)
        {
            var specificBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.EmailAddress == addNewBranch.EmailAddress);
            if (specificBranch is not null)
                throw new NotFoundException("This branch is already exist.");

            var existsCode = await _dbContext.Branches.FirstOrDefaultAsync(b=>b.BranchCode == addNewBranch.BranchCode);
            if (existsCode is not null)
                throw new NotFoundException("This branch is already exist.");

            var branch = new Branch
            {
                Name = addNewBranch.Name,
                Address = addNewBranch.Address,
                EmailAddress = addNewBranch.EmailAddress,
                OperatigHours = addNewBranch.OperatigHours,
                OperationalStatus = "Active",
                BranchCode = addNewBranch.BranchCode,
                PhoneNumber = addNewBranch.PhoneNumber
            };

            await _dbContext.Branches.AddAsync(branch);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Branch, BranchToReturnDTO>(branch);
        }


        public async Task DeleteBranchByIDAsync(int branchID)
        {
            var specificBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == branchID);
            if (specificBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            _dbContext.Branches.Remove(specificBranch);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<BranchToReturnDTO> UpdateBranchInformationAsync(int branchID, UpdateBranchInformationModel updateBranchInformation)
        {
            var specificBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == branchID);
            if (specificBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            specificBranch.Name = updateBranchInformation.Name;
            specificBranch.Address = updateBranchInformation.Address;
            specificBranch.EmailAddress = updateBranchInformation.EmailAddress;
            specificBranch.PhoneNumber = updateBranchInformation.PhoneNumber;
            specificBranch.BranchCode = updateBranchInformation.BranchCode;
            specificBranch.OperatigHours = updateBranchInformation.OperatigHours;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Branch, BranchToReturnDTO>(specificBranch);
        }

        public async Task<BranchToReturnDTO> ActivateBranchAsync(int branchID)
        {
            var specificBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == branchID);
            if (specificBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            specificBranch.OperationalStatus = "Active";
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Branch, BranchToReturnDTO>(specificBranch);
        }

        public async Task<BranchToReturnDTO> DeActivateBranchAsync(int branchID)
        {
            var specificBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == branchID);
            if (specificBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            specificBranch.OperationalStatus = "DeActive";
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<Branch, BranchToReturnDTO>(specificBranch);
        }
    }
}
