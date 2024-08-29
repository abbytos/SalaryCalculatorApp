
namespace SalaryCalculatorApp.Models
{
    /// <summary>
    /// Represents a detailed breakdown of salary components and deductions.
    /// </summary>
    public class SalaryBreakdown
    {
        /// <summary>
        /// Gets or sets the total gross salary or income before any deductions.
        /// </summary>
        public decimal GrossPackage { get; set; }

        /// <summary>
        /// Gets or sets the amount of superannuation contribution.
        /// </summary>
        public decimal SuperContribution { get; set; }

        /// <summary>
        /// Gets or sets the income that is subject to tax after all applicable deductions.
        /// </summary>
        public decimal TaxableIncome { get; set; }

        /// <summary>
        /// Gets or sets the amount of Medicare levy applied to the taxable income.
        /// </summary>
        public decimal MedicareLevy { get; set; }

        /// <summary>
        /// Gets or sets the amount of budget repair levy applied to the taxable income.
        /// </summary>
        public decimal BudgetRepairLevy { get; set; }

        /// <summary>
        /// Gets or sets the total income tax calculated based on the taxable income.
        /// </summary>
        public decimal IncomeTax { get; set; }

        /// <summary>
        /// Gets or sets the income remaining after all deductions and taxes.
        /// </summary>
        public decimal NetIncome { get; set; }

        /// <summary>
        /// Gets or sets the amount of pay received in each pay period, considering all deductions and taxes.
        /// </summary>
        public decimal PayPacketAmount { get; set; }

        /// <summary>
        /// Returns a string representation of the salary breakdown, including all key financial metrics.
        /// </summary>
        /// <returns>A formatted string displaying the salary breakdown.</returns>
        public override string ToString()
        {
            return $"Gross Package: {GrossPackage:C}\n" +
                   $"Super Contribution: {SuperContribution:C}\n" +
                   $"Taxable Income: {TaxableIncome:C}\n" +
                   $"Medicare Levy: {MedicareLevy:C}\n" +
                   $"Budget Repair Levy: {BudgetRepairLevy:C}\n" +
                   $"Income Tax: {IncomeTax:C}\n" +
                   $"Net Income: {NetIncome:C}\n" +
                   $"Pay Packet Amount: {PayPacketAmount:C}";
        }
    }

}
