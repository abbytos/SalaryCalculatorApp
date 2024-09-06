using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Interfaces;
using SalaryCalculatorApp.Models;

namespace SalaryCalculatorApp.Services
{
    /// <summary>
    /// Implementation of the salary calculator that computes the salary breakdown
    /// including deductions and pay packet amounts based on provided strategies.
    /// </summary>
    public class SalaryCalculator : ISalaryCalculator
    {
        private readonly IDeductionStrategy _incomeTaxStrategy;
        private readonly IDeductionStrategy _medicareLevyStrategy;
        private readonly IDeductionStrategy _budgetRepairLevyStrategy;

        private readonly decimal _superannuationRate;
        private readonly decimal _superannuationDenominator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalaryCalculator"/> class.
        /// </summary>
        /// <param name="incomeTaxStrategy">Strategy for calculating income tax.</param>
        /// <param name="medicareLevyStrategy">Strategy for calculating Medicare levy.</param>
        /// <param name="budgetRepairLevyStrategy">Strategy for calculating budget repair levy.</param>
        public SalaryCalculator(
            IDeductionStrategy incomeTaxStrategy,
            IDeductionStrategy medicareLevyStrategy,
            IDeductionStrategy budgetRepairLevyStrategy,
            IOptions<SalarySettingsConfig> config)
        {
            _incomeTaxStrategy = incomeTaxStrategy ?? throw new ArgumentNullException(nameof(incomeTaxStrategy));
            _medicareLevyStrategy = medicareLevyStrategy ?? throw new ArgumentNullException(nameof(medicareLevyStrategy));
            _budgetRepairLevyStrategy = budgetRepairLevyStrategy ?? throw new ArgumentNullException(nameof(budgetRepairLevyStrategy));
            
            // Validate and set configuration values
            var salaryConfig = config?.Value ?? throw new ArgumentNullException(nameof(config), "Salary settings configuration cannot be null.");
            _superannuationRate = salaryConfig.SuperannuationRate > 0
                ? salaryConfig.SuperannuationRate
                : throw new ArgumentException("Superannuation rate must be greater than zero.", nameof(salaryConfig.SuperannuationRate));

            _superannuationDenominator = salaryConfig.SuperannuationDenominator > 0
                ? salaryConfig.SuperannuationDenominator
                : throw new ArgumentException("Superannuation denominator must be greater than zero.", nameof(salaryConfig.SuperannuationDenominator));
        }

        /// <summary>
        /// Calculates the detailed salary breakdown based on the gross package and pay frequency.
        /// </summary>
        /// <param name="grossPackage">The total gross salary package.</param>
        /// <param name="payFrequency">The frequency of the salary payments.</param>
        /// <returns>A <see cref="SalaryBreakdown"/> object with detailed salary breakdown.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="grossPackage"/> is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="payFrequency"/> is invalid.</exception>
        public SalaryBreakdown CalculateSalaryBreakdown(decimal grossPackage, PayFrequency payFrequency)
        {
            if (grossPackage <= 0)
                throw new ArgumentOutOfRangeException(nameof(grossPackage), "Gross package must be greater than zero.");

            if (!Enum.IsDefined(typeof(PayFrequency), payFrequency))
                throw new ArgumentException("Invalid pay frequency.", nameof(payFrequency));

            // Calculate superannuation based on a fixed rate
            decimal superannuation = grossPackage * _superannuationRate / _superannuationDenominator;
            decimal taxableIncome = grossPackage - superannuation;

            // Calculate deductions using the provided strategies
            
            decimal incomeTax = _incomeTaxStrategy.CalculateDeduction(taxableIncome);
            decimal medicareLevy = _medicareLevyStrategy.CalculateDeduction(taxableIncome);
            decimal budgetRepairLevy = _budgetRepairLevyStrategy.CalculateDeduction(taxableIncome);

            // Round deductions to 2 decimal places
            decimal roundedIncomeTax = Math.Round(incomeTax, 2);
            decimal roundedMedicareLevy = Math.Round(medicareLevy, 2);
            decimal roundedBudgetRepairLevy = Math.Round(budgetRepairLevy, 2);

            // Calculate total deductions and net income
            decimal totalDeductions = roundedIncomeTax + roundedMedicareLevy + roundedBudgetRepairLevy;
            decimal netIncome = Math.Round(grossPackage - superannuation - totalDeductions, 2);

            // Calculate pay packet amount based on pay frequency
            decimal payPacketAmount = payFrequency switch
            {
                PayFrequency.Weekly => Math.Round(netIncome / 52, 2),
                PayFrequency.Fortnightly => Math.Round(netIncome / 26, 2),
                PayFrequency.Monthly => Math.Round(netIncome / 12, 2),
                _ => throw new ArgumentException("Invalid pay frequency.")
            };

            return new SalaryBreakdown
            {
                GrossPackage = grossPackage,
                SuperContribution = Math.Round(superannuation, 2),
                TaxableIncome = taxableIncome,
                IncomeTax = roundedIncomeTax,
                MedicareLevy = roundedMedicareLevy,
                BudgetRepairLevy = roundedBudgetRepairLevy,
                NetIncome = netIncome,
                PayPacketAmount = payPacketAmount
            };
        }
    }
}
