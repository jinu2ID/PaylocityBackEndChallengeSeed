using Api.Models;

namespace Api.Services.Calculation;

public class DefaultBenefitCostStrategy : IBenefitCostStrategy
{
    private readonly ISystemClock _systemClock;

    // These can be moved to a config file, database, or cloud stores like AWS/Azure App Config
    // so that the code doesn't need to be recompiled every time there is a change
    private const int PaychecksPerYear = 26;
    private const decimal EmployeeMonthlyBaseCost = 1000m;
    private const decimal DependentMonthlyCost = 600m;
    private const decimal HighSalaryThreshhold = 80000m;
    private const decimal HighSalaryAdditionalPercentage = 0.02m;
    private const int DependentAdditionalCostAge = 50; // Age at which dependents incur an addtional cost (DependentsOverAgeLimitAdditionalCost)
    private const decimal DependentOverAgeLimitAdditionalCost = 200m;

    public DefaultBenefitCostStrategy(ISystemClock systemClock)
    {
        _systemClock = systemClock;
    }

    public Paycheck GetPaycheck(Employee employee)
    {
        decimal salary = employee.Salary;
        ICollection<Dependent> dependents = employee.Dependents;
        DateTime nowUtc = _systemClock.UtcNow;

        decimal totalDependentMonthlyCost = dependents.Count() * DependentMonthlyCost;
        int dependentsOverAgeLimitCount = 0;

        foreach (var d in dependents)
        {
            var age = GetAge(d.DateOfBirth, nowUtc);
            if (age > DependentAdditionalCostAge)
            {
                totalDependentMonthlyCost += DependentOverAgeLimitAdditionalCost;
                dependentsOverAgeLimitCount++;
            }
        }

        decimal highSalaryMonthlyCost = 0m;
        if (salary > HighSalaryThreshhold)
        {
            decimal additionalYearlyCost = salary * HighSalaryAdditionalPercentage;
            highSalaryMonthlyCost = additionalYearlyCost / 12m;
        }

        decimal baseCostPerPaycheck = EmployeeMonthlyBaseCost * 12m / PaychecksPerYear;
        decimal dependentCostPerPaycheck = totalDependentMonthlyCost * 12m / PaychecksPerYear;
        decimal highSalaryCostPerPaycheck = highSalaryMonthlyCost * 12m / PaychecksPerYear;

        decimal totalCostPerPaycheck = baseCostPerPaycheck
                                      + dependentCostPerPaycheck
                                      + highSalaryCostPerPaycheck;

        decimal grossPayPerPaycheck = salary / PaychecksPerYear;
        decimal netPaycheckAmt = grossPayPerPaycheck - totalCostPerPaycheck;

        Paycheck paycheck = new Paycheck()
        {
            GrossPaycheckAmount = Math.Round(grossPayPerPaycheck, 2),
            BaseCost = Math.Round(baseCostPerPaycheck, 2),
            DependentCost = Math.Round(dependentCostPerPaycheck, 2),
            HighSalarySurcharge = salary > HighSalaryThreshhold ? HighSalaryAdditionalPercentage : 0m,
            HighSalaryCost = highSalaryCostPerPaycheck,
            PerPaycheckCost = Math.Round(totalCostPerPaycheck, 2),
            NetPaycheckAmount = Math.Round(netPaycheckAmt, 2),
            DependentsOverAdditionalCostAgeThreshold = dependentsOverAgeLimitCount
        };

        return paycheck;
    }

    // Unit test this for leap year
    private int GetAge(DateTime dateOfBirth, DateTime nowUTC)
    {
        int age = nowUTC.Year - dateOfBirth.Year;

        // Check if birthday has passed this year
        if (nowUTC < dateOfBirth.AddYears(age))
        {
            age--;
        }

        return age;
    }
}