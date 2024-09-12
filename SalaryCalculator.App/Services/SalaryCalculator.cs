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

        private const int DecimalPrecision = 2;

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
            if (grossPackage <= 0)
                throw new ArgumentOutOfRangeException(nameof(grossPackage), grossPackage, "Gross package must be greater than zero.");

            if (!Enum.IsDefined(typeof(PayFrequency), payFrequency))
                throw new ArgumentException($"The provided pay frequency '{payFrequency}' is not valid.", nameof(payFrequency));

            decimal superannuation = grossPackage * _salarySettings.SuperannuationRate / _salarySettings.SuperannuationDenominator;
            decimal taxableIncome = grossPackage - superannuation;

            var (incomeTax, medicareLevy, budgetRepairLevy) = _deductionCalculator.CalculateDeductions(taxableIncome);

            decimal totalDeductions = incomeTax + medicareLevy + budgetRepairLevy;
            decimal netIncome = Math.Round(grossPackage - superannuation - totalDeductions, DecimalPrecision);

            decimal payPacketAmount = payFrequency switch
            {
                PayFrequency.Weekly => Math.Round(netIncome / (int)PayFrequency.Weekly, DecimalPrecision),
                PayFrequency.Fortnightly => Math.Round(netIncome / (int)PayFrequency.Fortnightly, DecimalPrecision),
                PayFrequency.Monthly => Math.Round(netIncome / (int)PayFrequency.Monthly, DecimalPrecision),
                _ => throw new ArgumentException("Invalid pay frequency.")
            };

            // Return the detailed salary breakdown
            return new SalaryBreakdown
            {
                GrossPackage = grossPackage,
                SuperContribution = Math.Round(superannuation, DecimalPrecision),
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
