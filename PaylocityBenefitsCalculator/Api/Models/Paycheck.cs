namespace Api.Models;

public class Paycheck
{
    public decimal GrossPaycheckAmount { get; set; }
    public decimal BaseCost { get; set; }
    public decimal DependentCost { get; set; }
    public decimal HighSalarySurcharge { get; set; }
    public decimal HighSalaryCost { get; set; }
    public decimal PerPaycheckCost { get; set; }
    public decimal NetPaycheckAmount { get; set; }
    public int DependentsOverAdditionalCostAgeThreshold { get; set; }
}
