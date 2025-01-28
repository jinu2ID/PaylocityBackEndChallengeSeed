using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services.Employees;

public interface IEmployeeService
{ 
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<List<Employee>> GetAllEmployeesAsync();
    Task<GetEmployeeDto?> AddEmployeeAsync(CreateNewEmployeeDto employeeDto);
    Task<Dependent> AddDependentAsync(CreateNewDependentDto dependentDto);
    Task<Dependent> GetDependentByIdAsync(int dependentId);
    Task<List<Dependent>> GetAllDependentsAsync();
}
