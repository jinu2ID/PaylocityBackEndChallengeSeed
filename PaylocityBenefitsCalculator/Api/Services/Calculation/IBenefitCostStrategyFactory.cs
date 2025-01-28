using Api.Models;

namespace Api.Services.Calculation;

public interface IBenefitCostStrategyFactory
{
    IBenefitCostStrategy GetStrategy(Employee employee);
}
