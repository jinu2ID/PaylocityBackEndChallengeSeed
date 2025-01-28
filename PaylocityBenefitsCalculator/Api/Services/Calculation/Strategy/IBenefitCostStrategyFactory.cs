using Api.Models;

namespace Api.Services.Calculation.Strategy;

public interface IBenefitCostStrategyFactory
{
    IBenefitCostStrategy GetStrategy(Employee employee);
}
