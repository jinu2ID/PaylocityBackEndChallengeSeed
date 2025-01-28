using Api.Models;

namespace Api.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllEmployeesAsync();
    Task<List<Employee>> GetAllEmployeesPagedAsync(int pageIndex, int pageSize);
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<Employee?> AddEmployeeAsync(Employee employee);
    Task<Dependent?> AddDependentAsync(Dependent dependent);
    Task<Dependent?> GetDependentByIdAsync(int dependentId);
    Task<List<Dependent>> GetAllDependentsAsync();
    Task<List<Dependent>> GetAllDependentsPagedAsync(int pageIndex, int pageSize);
}
