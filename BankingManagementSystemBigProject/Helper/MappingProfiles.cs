using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace BankingManagementSystemBigProject.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Account, AccountToReturnDTO>()
                .ForMember(a => a.CustomerName, a => a.MapFrom(c => c.Customer.FullName))
                .ForMember(a => a.BranchName, a => a.MapFrom(b => b.Branch.Name));
            /////////////////////////////////////////////////////////////////////////////////
            CreateMap<Benificiary, BenificiaryToReturnDTO>();
            /////////////////////////////////////////////////////////////////////////////////
            CreateMap<Branch, BranchToReturnDTO>();
            /////////////////////////////////////////////////////////////////////////////////
            CreateMap<Card, CardToReturnDTO>();
            /////////////////////////////////////////////////////////////////////////////////
            CreateMap<Customer, CustomerToReturnDTO>();
            /////////////////////////////////////////////////////////////////////////////////
            CreateMap<Employee, EmployeeToReturnDTO>()
                .ForMember(e => e.BranchName, e => e.MapFrom(b => b.Branch.Name));
            /////////////////////////////////////////////////////////////////////////////////
            CreateMap<Loan, LoanToReturnDTO>()
                .ForMember(l => l.CustomerName, l => l.MapFrom(c => c.Customer.FullName));
            /////////////////////////////////////////////////////////////////////////////////
            CreateMap<Payment, PaymentToReturnDTO>();
            /////////////////////////////////////////////////////////////////////////////////
            CreateMap<Transaction, TransactionToReturnDTO>();
            /////////////////////////////////////////////////////////////////////////////////
            CreateMap<User, UserToReturnDTO>()
                .ForMember(u => u.RoleName, u => u.MapFrom(r => r.Role.RoleName));
            /////////////////////////////////////////////////////////////////////////////////
            CreateMap<Role, RoleToReturnDTO>();
        }
    }
}
