using SalaryCalculatorApp.Models;

namespace SalaryCalculatorApp.Interfaces
{
    /// <summary>
    /// Defines the contract for a salary calculator.
    /// </summary>
    public interface ISalaryCalculator
    {
        /// <summary>
        /// Calculates the detailed salary breakdown based on the provided gross package and pay frequency.
        /// </summary>
        /// <param name="grossPackage">The total gross salary package.</param>
        /// <param name="payFrequency">The frequency of the salary payments (e.g., weekly, fortnightly, monthly).</param>
        /// <returns>A <see cref="SalaryBreakdown"/> object containing the detailed breakdown of the salary.</returns>
        SalaryBreakdown CalculateSalaryBreakdown(decimal grossPackage, PayFrequency payFrequency);
    }
}
