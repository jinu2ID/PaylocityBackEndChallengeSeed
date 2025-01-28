using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services.Employees;

public interface IEmployeeService
{ 
    Task<Employee?> GetByIdAsync(int id);
    Task<List<Employee>> GetAllAsync();
    Task<GetEmployeeDto?> AddEmployeeAsync(CreateNewEmployeeDto employeeDto);
}
