using Api.Models;

namespace Api.Services.Calculation;

public interface ICalculationService
{
    Task<Paycheck> GetPaycheckAsync(int employeeId);

    Task<decimal> GetPaycheckNetAmountAsync(int employeeId);
}
