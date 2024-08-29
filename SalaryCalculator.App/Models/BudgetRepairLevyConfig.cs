
namespace SalaryCalculatorApp.Models
{
    /// <summary>
    /// Configuration settings for the Budget Repair Levy.
    /// </summary>
    public class BudgetRepairLevyConfig
    {
        /// <summary>
        /// Gets or sets the income threshold above which the Budget Repair Levy is applied.
        /// </summary>
        public decimal Threshold { get; set; }

        /// <summary>
        /// Gets or sets the rate at which the Budget Repair Levy is calculated for income above the threshold.
        /// </summary>
        public decimal Rate { get; set; }
    }

}
