using System.Threading.Tasks;
using Api.Exceptions;
using Api.Models;
using Api.Services.Employees;

namespace Api.Services.Calculation;

public class CalculationService : ICalculationService
{
    private readonly IEmployeeService _employeeService;
    private readonly IBenefitCostStrategyFactory _benfitCostStrategyFactory;

    public CalculationService(IEmployeeService employeeService, IBenefitCostStrategyFactory benefitCostStrategyFactory)
    {
        _employeeService = employeeService;
        _benfitCostStrategyFactory = benefitCostStrategyFactory;
    }

    public async Task<Paycheck> GetPaycheckAsync(int employeeId)
    {
        Employee? employee = await _employeeService.GetByIdAsync(employeeId);
        if (employee == null)
        {
            throw new EmployeeNotFoundException(employeeId);
        }

        var strategy = _benfitCostStrategyFactory.GetStrategy(employee);

        var paycheck = strategy.GetPaycheck(employee);
        return paycheck;
    }

    public async Task<decimal> GetPaycheckNetAmountAsync(int employeeId)
    {
        var paycheck = await GetPaycheckAsync(employeeId);
        return paycheck.NetPaycheckAmount;
    }
}
