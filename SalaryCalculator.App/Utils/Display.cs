using SalaryCalculatorApp.Models;

namespace SalaryCalculatorApp.Utils
{
    /// <summary>
    /// Provides methods for displaying salary breakdown information.
    /// </summary>
    public static class Display
    {
        /// <summary>
        /// Shows the detailed salary breakdown on the console.
        /// </summary>
        /// <param name="breakdown">An instance of <see cref="SalaryBreakdown"/> containing the salary details.</param>
        public static void ShowBreakdown(SalaryBreakdown breakdown)
        {
            Console.WriteLine(); 
            Console.WriteLine($"Gross Package: {breakdown.GrossPackage:C}"); 
            Console.WriteLine($"Superannuation: {breakdown.SuperContribution:C}"); 
            Console.WriteLine(); 
            Console.WriteLine($"Taxable Income: {breakdown.TaxableIncome:C}"); 
            Console.WriteLine(); 
            Console.WriteLine("Deductions:"); 
            Console.WriteLine($"Medicare Levy: {breakdown.MedicareLevy:C}"); 
            Console.WriteLine($"Budget Repair Levy: {breakdown.BudgetRepairLevy:C}"); 
            Console.WriteLine($"Income Tax: {breakdown.IncomeTax:C}");
            Console.WriteLine(); 
            Console.WriteLine($"Net Income: {breakdown.NetIncome:C}"); 
            Console.WriteLine($"Pay Packet Amount: {breakdown.PayPacketAmount:C}"); 
        }
    }
}
