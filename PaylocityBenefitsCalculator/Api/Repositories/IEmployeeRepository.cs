using Api.Models;

namespace Api.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();
    Task<List<Employee>> GetPagedAsync(int pageIndex, int pageSize);
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> AddEmployeeAsync(Employee employee);
    Task<Dependent?> AddDependentAsync(Dependent dependent);
}
