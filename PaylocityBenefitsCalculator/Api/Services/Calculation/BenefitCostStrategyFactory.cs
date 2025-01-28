using Api.Models;

namespace Api.Services.Calculation;

public class BenefitCostStrategyFactory : IBenefitCostStrategyFactory
{
    public IBenefitCostStrategy GetStrategy(Employee employee)
    {
        // If more strategies for calculating costs and paycheck are required they can be added here
        //if (employee.Type == EmployeeType.Standard)
        //{
        //    return new DefaultBenefitCostStrategy();
        //}
        //else if (employee.Type == EmployeeType.Executive)
        //{
        //    return new HighSalaryBenefitCostStrategy();
        //}
        //// etc...

        // Fallback
        ISystemClock clock = new SystemClock();
        return new DefaultBenefitCostStrategy(clock);
    }
}
