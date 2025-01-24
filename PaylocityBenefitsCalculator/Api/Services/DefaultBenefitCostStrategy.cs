using Api.Models;

namespace Api.Services;

public class DefaultBenefitCostStrategy : IBenefitCostStrategy
{
    private readonly int PaychecksPerYear;
    private readonly decimal EmployeeMonthlyBaseCost;
    private readonly decimal DependentMonthlyCost;
    private readonly decimal HighSalaryThreshhold;
    private readonly decimal HighSalaryAdditionalPercentage;

    public decimal CalculatePaycheck(Employee employee)
    {
        throw new NotImplementedException();
    }
}