namespace SalaryCalculatorApp.Models
{
    /// <summary>
    /// Configuration settings for Medicare levy calculations.
    /// </summary>
    public class MedicareLevyConfig
    {
        /// <summary>
        /// Gets or sets the first income threshold for Medicare levy calculation.
        /// Income up to this amount is taxed at <see cref="Rate1"/>.
        /// </summary>
        public decimal Threshold1 { get; set; }

        /// <summary>
        /// Gets or sets the second income threshold for Medicare levy calculation.
        /// Income between <see cref="Threshold1"/> and this amount is taxed at <see cref="Rate2"/>.
        /// </summary>
        public decimal Threshold2 { get; set; }

        /// <summary>
        /// Gets or sets the Medicare levy rate applied to income within the first threshold range.
        /// </summary>
        public decimal Rate1 { get; set; }

        /// <summary>
        /// Gets or sets the Medicare levy rate applied to income above the second threshold.
        /// </summary>
        public decimal Rate2 { get; set; }
    }
}
