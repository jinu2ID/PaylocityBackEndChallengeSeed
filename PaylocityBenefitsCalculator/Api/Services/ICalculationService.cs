namespace Api.Services;

public interface ICalculationService
{
    Task<decimal> GetPaycheck(int employeeId);
}
