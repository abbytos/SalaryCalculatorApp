namespace SalaryCalculatorApp.Models
{
    /// <summary>
    /// Configuration settings for salary-related constants such as Superannuation.
    /// </summary>
    public class SalarySettingsConfig
    {
        /// <summary>
        /// Gets or sets the superannuation rate as a percentage.
        /// </summary>
        public decimal SuperannuationRate { get; set; }

        /// <summary>
        /// Gets or sets the denominator used for superannuation calculations.
        /// </summary>
        public decimal SuperannuationDenominator { get; set; }
    }
}
