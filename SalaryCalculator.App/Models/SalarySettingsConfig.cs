namespace SalaryCalculatorApp.Models
{
    /// <summary>
    /// Configuration settings for salary-related constants such as superannuation.
    /// This class holds configuration values used in salary calculations, such as the superannuation rate and denominator.
    /// </summary>
    public class SalarySettingsConfig
    {
        /// <summary>
        /// Gets or sets the superannuation rate as a percentage.
        /// This value represents the rate at which superannuation contributions are calculated from the gross salary.
        /// </summary>
        public decimal SuperannuationRate { get; set; }

        /// <summary>
        /// Gets or sets the denominator used for superannuation calculations.
        /// This value is used in conjunction with the superannuation rate to compute the superannuation contribution.
        /// </summary>
        public decimal SuperannuationDenominator { get; set; }

        /// <summary>
        /// Validates the configuration settings to ensure they are correctly set.
        /// Throws an exception if any of the configuration values are invalid (e.g., non-positive values).
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown when the superannuation rate or denominator is less than or equal to zero.
        /// </exception>
        public void Validate()
        {
            // Validate that the superannuation rate is greater than zero
            if (SuperannuationRate <= 0)
                throw new ArgumentException("Superannuation rate must be greater than zero.");

            // Validate that the superannuation denominator is greater than zero
            if (SuperannuationDenominator <= 0)
                throw new ArgumentException("Superannuation denominator must be greater than zero.");
        }
    }
}
