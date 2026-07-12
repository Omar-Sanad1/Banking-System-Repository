using Core.DTOs;
using Core.Entities;
using Service.Models.EmployeeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<EmployeeToReturnDTO>> GetAllEmployeesPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<EmployeeToReturnDTO> GetAllEmployeesFiltered(Func<Employee, bool> Filter);
        public Task<EmployeeToReturnDTO> GetEmployeeByIDAsync(int employeeID);
        public Task<EmployeeToReturnDTO> GetEmployeeByNationalIDAsync(string nationalID);
        public Task<string> AddNewEmployeeAsync(AddNewEmployeeModel addNewEmployee);
        public Task<EmployeeToReturnDTO> UpdateEmployeeInformationAsync(int employeeID , UpdateEmployeeInformationModel updateEmployeeInformation);
        public Task DeleteEmployeeByIDAsync(int employeeID);

    }
}
