using Api.Models;

namespace Api.Services
{
    public interface IBenefitCostStrategy
    {
        decimal CalculatePaycheck(Employee employee);
    }
}