using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Interfaces;
using SalaryCalculatorApp.Models;

namespace SalaryCalculatorApp.Strategies
{
    /// <summary>
    /// Strategy for calculating income tax based on taxable income.
    /// </summary>
    public class IncomeTaxStrategy : IDeductionStrategy
    {
        // Tax brackets and rates
        private readonly IncomeTaxConfig _incomeTaxConfig;

        public IncomeTaxStrategy(IOptions<IncomeTaxConfig> options)
        {
            _incomeTaxConfig = options.Value;
        }


        /// <summary>
        /// Calculates the income tax based on the taxable income.
        /// </summary>
        /// <param name="taxableIncome">The taxable income to calculate the tax against.</param>
        /// <returns>The calculated income tax.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="taxableIncome"/> is less than or equal to zero.</exception>
        public decimal CalculateDeduction(decimal taxableIncome)
        {
            if (taxableIncome <= 0)
                throw new ArgumentOutOfRangeException(nameof(taxableIncome), "Income must be greater than zero.");

            return taxableIncome switch
            {
                _ when taxableIncome <= _incomeTaxConfig.Threshold1 => 0,
                _ when taxableIncome <= _incomeTaxConfig.Threshold2 => Math.Floor(_incomeTaxConfig.BaseTax1 + (taxableIncome - _incomeTaxConfig.Threshold1) * _incomeTaxConfig.Rate1),
                _ when taxableIncome <= _incomeTaxConfig.Threshold3 => Math.Floor(_incomeTaxConfig.BaseTax2 + (taxableIncome - _incomeTaxConfig.Threshold2) * _incomeTaxConfig.Rate2),
                _ when taxableIncome <= _incomeTaxConfig.Threshold4 => Math.Floor(_incomeTaxConfig.BaseTax3 + (taxableIncome - _incomeTaxConfig.Threshold3) * _incomeTaxConfig.Rate3),
                _ => Math.Floor(_incomeTaxConfig.BaseTax4 + (taxableIncome - _incomeTaxConfig.Threshold4) * _incomeTaxConfig.Rate4),
            };

        }
    }
}
