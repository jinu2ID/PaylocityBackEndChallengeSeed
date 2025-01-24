namespace Api.Services
{
    public interface ICalculationService
    {
        decimal GetPaycheck(int employeeId);
    }
}
