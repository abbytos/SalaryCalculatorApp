using SalaryCalculatorApp.Models;

namespace SalaryCalculatorApp.Utils
{
    /// <summary>
    /// Provides utility methods for obtaining user input.
    /// </summary>
    public static class UserInput
    {
        /// <summary>
        /// Prompts the user for a decimal input and parses it.
        /// </summary>
        /// <param name="prompt">The prompt message to display to the user.</param>
        /// <returns>The parsed decimal value.</returns>
        /// <exception cref="FormatException">Thrown when the input is not a valid decimal.</exception>
        public static decimal GetDecimalInput(string prompt)
        {
            Console.Write(prompt); // Display the prompt message to the user.
            if (decimal.TryParse(Console.ReadLine(), out var result))
            {
                return result; // Return the parsed decimal value.
            }
            throw new FormatException("Invalid decimal format."); // Throw an exception if parsing fails.
        }

        /// <summary>
        /// Prompts the user to enter their pay frequency and parses the input.
        /// </summary>
        /// <returns>The parsed pay frequency as a <see cref="PayFrequency"/> enumeration value.</returns>
        /// <exception cref="ArgumentException">Thrown when the input is not a valid pay frequency.</exception>
        public static PayFrequency GetPayFrequency()
        {
            Console.Write("Enter your pay frequency (W for weekly, F for fortnightly, M for monthly): ");
            return Console.ReadLine()?.ToUpper() switch
            {
                "W" => PayFrequency.Weekly,       // Map 'W' to PayFrequency.Weekly.
                "F" => PayFrequency.Fortnightly,   // Map 'F' to PayFrequency.Fortnightly.
                "M" => PayFrequency.Monthly,       // Map 'M' to PayFrequency.Monthly.
                _ => throw new ArgumentException("Invalid pay frequency. Please enter W, F, or M.") // Throw an exception for invalid inputs.
            };
        }
    }
}
