using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Interfaces;
using SalaryCalculatorApp.Models;
using System;

namespace SalaryCalculatorApp.Strategies
{
    /// <summary>
    /// Implements the calculation of Medicare Levy as a deduction strategy.
    /// </summary>
    public class MedicareLevyStrategy : IDeductionStrategy
    {
        private readonly MedicareLevyConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicareLevyStrategy"/> class.
        /// </summary>
        /// <param name="config">Configuration options for Medicare Levy calculation.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="config"/> is null.</exception>
        public MedicareLevyStrategy(IOptions<MedicareLevyConfig> config)
        {
            _config = config?.Value ?? throw new ArgumentNullException(nameof(config));

        }

        /// <summary>
        /// Calculates the Medicare Levy based on taxable income.
        /// </summary>
        /// <param name="taxableIncome">The taxable income to base the levy calculation on.</param>
        /// <returns>The calculated Medicare Levy.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the taxable income is less than or equal to zero.</exception>
        public decimal CalculateDeduction(decimal taxableIncome)
        {
            // Validate input: taxable income must be greater than zero.
            if (taxableIncome <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(taxableIncome), "Taxable income must be greater than zero.");
            }

            // Calculate Medicare Levy based on taxable income brackets.
            return taxableIncome switch
            {
                // No Medicare Levy for income up to the lower threshold.
                _ when taxableIncome <= _config.Threshold1 => 0,

                // Medicare Levy is a percentage of the amount above the lower threshold but up to the higher threshold.
                _ when taxableIncome <= _config.Threshold2 => Math.Ceiling((taxableIncome - _config.Threshold1) * _config.Rate1),

                // Medicare Levy is a percentage of the entire taxable income above the higher threshold.
                _ => Math.Ceiling(taxableIncome * _config.Rate2),
            };
        }
    }
}
