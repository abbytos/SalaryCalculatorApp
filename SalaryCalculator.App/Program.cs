using Microsoft.Extensions.DependencyInjection;
using SalaryCalculatorApp.Interfaces;
using SalaryCalculatorApp.Models;
using SalaryCalculatorApp.Utils;

namespace SalaryCalculatorApp
{
    /// <summary>
    /// The Program class serves as the entry point for the Salary Calculator application.
    /// It sets up the dependency injection framework and handles user interactions.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The Main method is the entry point of the application. It initializes the necessary services,
        /// prompts the user for input, and triggers the salary calculation process.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        static void Main(string[] args)
        {
            // Build the host to set up dependency injection and configuration.
            var host = HostBuilderExtensions.CreateHostBuilder(args).Build();
            var serviceProvider = host.Services;

            try
            {
                // Prompt the user to enter the gross salary package.
                var grossPackage = UserInput.GetDecimalInput("Enter your gross salary package: ");

                // Prompt the user to select the pay frequency (weekly, fortnightly, or monthly).
                var payFrequency = UserInput.GetPayFrequency();

                // Calculate and display the salary breakdown based on user input.
                CalculateAndDisplaySalaryBreakdown(serviceProvider, grossPackage, payFrequency);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during input or calculation.
                ExceptionHandler.Handle(ex);
            }
        }

        /// <summary>
        /// Calculates the salary breakdown and displays it to the user.
        /// </summary>
        /// <param name="serviceProvider">Service provider used to retrieve necessary services via dependency injection.</param>
        /// <param name="grossPackage">The gross salary package entered by the user.</param>
        /// <param name="payFrequency">The pay frequency selected by the user (weekly, fortnightly, or monthly).</param>
        private static void CalculateAndDisplaySalaryBreakdown(IServiceProvider serviceProvider, decimal grossPackage, PayFrequency payFrequency)
        {
            // Retrieve the salary calculator service from the service provider (dependency injection).
            var salaryCalculator = serviceProvider.GetRequiredService<ISalaryCalculator>();

            // Calculate the salary breakdown based on the gross package and pay frequency.
            var breakdown = salaryCalculator.CalculateSalaryBreakdown(grossPackage, payFrequency);

            // Display the calculated salary breakdown to the user.
            Display.ShowBreakdown(breakdown);
        }
    }
}
