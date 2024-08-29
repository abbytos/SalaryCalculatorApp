namespace SalaryCalculatorApp.Models
{
    /// <summary>
    /// Configuration settings for income tax calculations.
    /// </summary>
    public class IncomeTaxConfig
    {
        /// <summary>
        /// Gets or sets the first income threshold for tax calculation.
        /// Income up to this amount is taxed at <see cref="Rate1"/>.
        /// </summary>
        public decimal Threshold1 { get; set; }

        /// <summary>
        /// Gets or sets the second income threshold for tax calculation.
        /// Income between <see cref="Threshold1"/> and this amount is taxed at <see cref="Rate2"/>.
        /// </summary>
        public decimal Threshold2 { get; set; }

        /// <summary>
        /// Gets or sets the third income threshold for tax calculation.
        /// Income between <see cref="Threshold2"/> and this amount is taxed at <see cref="Rate3"/>.
        /// </summary>
        public decimal Threshold3 { get; set; }

        /// <summary>
        /// Gets or sets the fourth income threshold for tax calculation.
        /// Income above this amount is taxed at <see cref="Rate4"/>.
        /// </summary>
        public decimal Threshold4 { get; set; }

        /// <summary>
        /// Gets or sets the tax rate applied to income within the first threshold range.
        /// </summary>
        public decimal Rate1 { get; set; }

        /// <summary>
        /// Gets or sets the tax rate applied to income within the second threshold range.
        /// </summary>
        public decimal Rate2 { get; set; }

        /// <summary>
        /// Gets or sets the tax rate applied to income within the third threshold range.
        /// </summary>
        public decimal Rate3 { get; set; }

        /// <summary>
        /// Gets or sets the tax rate applied to income above the fourth threshold.
        /// </summary>
        public decimal Rate4 { get; set; }

        /// <summary>
        /// Gets or sets the base tax amount for income within the first threshold range.
        /// </summary>
        public decimal BaseTax1 { get; set; }

        /// <summary>
        /// Gets or sets the base tax amount for income within the second threshold range.
        /// </summary>
        public decimal BaseTax2 { get; set; }

        /// <summary>
        /// Gets or sets the base tax amount for income within the third threshold range.
        /// </summary>
        public decimal BaseTax3 { get; set; }

        /// <summary>
        /// Gets or sets the base tax amount for income above the fourth threshold.
        /// </summary>
        public decimal BaseTax4 { get; set; }
    }
}
