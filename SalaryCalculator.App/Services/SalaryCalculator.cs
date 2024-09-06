using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Interfaces;
using SalaryCalculatorApp.Models;

namespace SalaryCalculatorApp.Services
{
    /// <summary>
    /// Implementation of the salary calculator that computes the salary breakdown
    /// including deductions and pay packet amounts based on provided strategies and settings.
    /// </summary>
    public class SalaryCalculator : ISalaryCalculator
    {
        private readonly DeductionCalculator _deductionCalculator;
        private readonly SalarySettingsConfig _salarySettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalaryCalculator"/> class.
        /// </summary>
        /// <param name="deductionCalculator">The calculator for various deductions (income tax, Medicare levy, budget repair levy).</param>
        /// <param name="salarySettings">Configuration options for salary settings including superannuation rate and denominator.</param>
        public SalaryCalculator(
            DeductionCalculator deductionCalculator,
            IOptions<SalarySettingsConfig> salarySettings)
        {
            _deductionCalculator = deductionCalculator ?? throw new ArgumentNullException(nameof(deductionCalculator));
            _salarySettings = salarySettings?.Value ?? throw new ArgumentNullException(nameof(salarySettings));

            // Validate the salary settings to ensure they are correct
            _salarySettings.Validate();
        }

        /// <summary>
        /// Calculates the detailed salary breakdown based on the gross package and pay frequency.
        /// </summary>
        /// <param name="grossPackage">The total gross salary package.</param>
        /// <param name="payFrequency">The frequency of the salary payments (e.g., Weekly, Fortnightly, Monthly).</param>
        /// <returns>A <see cref="SalaryBreakdown"/> object with detailed salary breakdown.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="grossPackage"/> is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="payFrequency"/> is invalid.</exception>
        public SalaryBreakdown CalculateSalaryBreakdown(decimal grossPackage, PayFrequency payFrequency)
        {
            // Validate that grossPackage is greater than zero
            if (grossPackage <= 0)
                throw new ArgumentOutOfRangeException(nameof(grossPackage), "Gross package must be greater than zero.");

            // Validate that payFrequency is a defined enum value
            if (!Enum.IsDefined(typeof(PayFrequency), payFrequency))
                throw new ArgumentException("Invalid pay frequency.", nameof(payFrequency));

            // Calculate superannuation based on the provided rate and denominator
            decimal superannuation = grossPackage * _salarySettings.SuperannuationRate / _salarySettings.SuperannuationDenominator;
            decimal taxableIncome = grossPackage - superannuation;

            // Calculate deductions using the DeductionCalculator
            var (incomeTax, medicareLevy, budgetRepairLevy) = _deductionCalculator.CalculateDeductions(taxableIncome);

            // Calculate total deductions and net income
            decimal totalDeductions = incomeTax + medicareLevy + budgetRepairLevy;
            decimal netIncome = Math.Round(grossPackage - superannuation - totalDeductions, 2);

            // Calculate the pay packet amount based on the pay frequency
            decimal payPacketAmount = payFrequency switch
            {
                PayFrequency.Weekly => Math.Round(netIncome / 52, 2),
                PayFrequency.Fortnightly => Math.Round(netIncome / 26, 2),
                PayFrequency.Monthly => Math.Round(netIncome / 12, 2),
                _ => throw new ArgumentException("Invalid pay frequency.")
            };

            // Return the detailed salary breakdown
            return new SalaryBreakdown
            {
                GrossPackage = grossPackage,
                SuperContribution = Math.Round(superannuation, 2),
                TaxableIncome = taxableIncome,
                IncomeTax = incomeTax,
                MedicareLevy = medicareLevy,
                BudgetRepairLevy = budgetRepairLevy,
                NetIncome = netIncome,
                PayPacketAmount = payPacketAmount
            };
        }
    }
}
