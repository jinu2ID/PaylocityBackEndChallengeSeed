using Api.Models;

namespace Api.Services.Calculation;

public interface IBenefitCostStrategy
{
    Paycheck GetPaycheck(Employee employee);
}