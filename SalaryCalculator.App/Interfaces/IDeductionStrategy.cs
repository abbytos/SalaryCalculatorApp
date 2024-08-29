namespace SalaryCalculatorApp.Interfaces
{
    /// <summary>
    /// Defines the contract for deduction strategies.
    /// </summary>
    public interface IDeductionStrategy
    {
        /// <summary>
        /// Calculates the deduction amount based on the provided taxable income.
        /// </summary>
        /// <param name="taxableIncome">The taxable income for which the deduction is to be calculated.</param>
        /// <returns>The calculated deduction amount.</returns>
        decimal CalculateDeduction(decimal taxableIncome);
    }
}
