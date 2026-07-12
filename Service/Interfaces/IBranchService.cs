using Core.DTOs;
using Core.Entities;
using Service.Models.BranchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IBranchService
    {
        public Task<IEnumerable<BranchToReturnDTO>> GetAllBranchesPagedAsync(int pageNumber,int pageSize);
        public IEnumerable<BranchToReturnDTO> GetAllBranchesFiltered(Func<Branch , bool> Filter);
        public Task<BranchToReturnDTO> GetBranchByIDAsync(int branchID);
        public Task<BranchToReturnDTO> GetBranchByEmailAddressAsync(string emailAddress);
        public Task<BranchToReturnDTO> AddNewBranchAsync(AddNewBranchModel addNewBranch);
        public Task<BranchToReturnDTO> UpdateBranchInformationAsync(int branchID,UpdateBranchInformationModel updateBranchInformation);
        public Task DeleteBranchByIDAsync(int branchID);
        public Task<BranchToReturnDTO> ActivateBranchAsync(int branchID);
        public Task<BranchToReturnDTO> DeActivateBranchAsync(int branchID);
    }
}
