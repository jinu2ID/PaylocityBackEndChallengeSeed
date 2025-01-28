using Api.Models;

namespace Api.Services.Calculation.Strategy;

public interface IBenefitCostStrategy
{
    Paycheck GetPaycheck(Employee employee);
}