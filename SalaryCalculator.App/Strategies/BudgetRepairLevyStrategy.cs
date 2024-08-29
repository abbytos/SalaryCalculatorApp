using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Interfaces;
using SalaryCalculatorApp.Models;

namespace SalaryCalculatorApp.Strategies
{
    /// <summary>
    /// Strategy for calculating the Budget Repair Levy based on taxable income.
    /// </summary>
    public class BudgetRepairLevyStrategy : IDeductionStrategy
    {
        private readonly BudgetRepairLevyConfig _config;

        public BudgetRepairLevyStrategy(IOptions<BudgetRepairLevyConfig> config)
        {
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));

        }


        /// <summary>
        /// Calculates the Budget Repair Levy based on the taxable income.
        /// </summary>
        /// <param name="taxableIncome">The taxable income to calculate the levy against.</param>
        /// <returns>The calculated Budget Repair Levy.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="taxableIncome"/> is less than or equal to zero.</exception>
        public decimal CalculateDeduction(decimal taxableIncome)
        {
            if (taxableIncome <= 0)
                throw new ArgumentOutOfRangeException(nameof(taxableIncome), "Income must be greater than zero.");

            return taxableIncome > _config.Threshold
                ? Math.Floor((taxableIncome - _config.Threshold) * _config.Rate)
                : 0;
        }
    }
}
