namespace SalaryCalculatorApp.Models
{
    /// <summary>
    /// Represents the different payment frequencies for salary calculations.
    /// </summary>
    public enum PayFrequency
    {
        /// <summary>
        /// Indicates a weekly payment frequency, where salary is paid every week.
        /// </summary>
        Weekly,

        /// <summary>
        /// Indicates a fortnightly payment frequency, where salary is paid every two weeks.
        /// </summary>
        Fortnightly,

        /// <summary>
        /// Indicates a monthly payment frequency, where salary is paid once a month.
        /// </summary>
        Monthly
    }
}
