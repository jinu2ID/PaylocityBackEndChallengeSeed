using System.Threading.Tasks;
using Api.Services.Employees;

namespace Api.Services;

public class CalculationService : ICalculationService
{
    private readonly IEmployeeService _employeeService;
    
    public CalculationService(IEmployeeService employeeService) 
    {
        _employeeService = employeeService;
    }

    public async Task<decimal> GetPaycheck(int employeeId)
    {
        var employee = await _employeeService.GetByIdAsync(employeeId);
        var salary = employee.Salary;

        throw new NotImplementedException();

    }
}
