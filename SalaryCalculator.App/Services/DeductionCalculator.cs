using SalaryCalculatorApp.Interfaces;

namespace SalaryCalculatorApp.Services
{
    /// <summary>
    /// Provides methods to calculate various deductions based on provided strategies.
    /// </summary>
    public class DeductionCalculator
    {
        private readonly IDeductionStrategy _incomeTaxStrategy;
        private readonly IDeductionStrategy _medicareLevyStrategy;
        private readonly IDeductionStrategy _budgetRepairLevyStrategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeductionCalculator"/> class.
        /// </summary>
        /// <param name="incomeTaxStrategy">Strategy for calculating income tax deductions.</param>
        /// <param name="medicareLevyStrategy">Strategy for calculating Medicare levy deductions.</param>
        /// <param name="budgetRepairLevyStrategy">Strategy for calculating budget repair levy deductions.</param>
        public DeductionCalculator(
            IDeductionStrategy incomeTaxStrategy,
            IDeductionStrategy medicareLevyStrategy,
            IDeductionStrategy budgetRepairLevyStrategy)
        { 
            _incomeTaxStrategy = incomeTaxStrategy ?? throw new ArgumentNullException(nameof(incomeTaxStrategy));
            _medicareLevyStrategy = medicareLevyStrategy ?? throw new ArgumentNullException(nameof(medicareLevyStrategy));
            _budgetRepairLevyStrategy = budgetRepairLevyStrategy ?? throw new ArgumentNullException(nameof(budgetRepairLevyStrategy));
        }

        /// <summary>
        /// Calculates the total deductions for income tax, Medicare levy, and budget repair levy.
        /// </summary>
        /// <param name="taxableIncome">The income amount to calculate deductions for.</param>
        /// <returns>A tuple containing rounded values for income tax, Medicare levy, and budget repair levy.</returns>
        public (decimal incomeTax, decimal medicareLevy, decimal budgetRepairLevy) CalculateDeductions(decimal taxableIncome)
        {
            // Calculate deductions using the provided strategies
            decimal incomeTax = _incomeTaxStrategy.CalculateDeduction(taxableIncome);
            decimal medicareLevy = _medicareLevyStrategy.CalculateDeduction(taxableIncome);
            decimal budgetRepairLevy = _budgetRepairLevyStrategy.CalculateDeduction(taxableIncome);

            // Round the calculated deductions to 2 decimal places and return as a tuple
            return (
                incomeTax: Math.Round(incomeTax, 2),
                medicareLevy: Math.Round(medicareLevy, 2),
                budgetRepairLevy: Math.Round(budgetRepairLevy, 2)
            );
        }
    }
}
