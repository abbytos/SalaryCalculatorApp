using Microsoft.Extensions.DependencyInjection;
using SalaryCalculatorApp.Interfaces;
using SalaryCalculatorApp.Models;
using SalaryCalculatorApp.Utils;

namespace SalaryCalculatorApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var host = HostBuilderExtensions.CreateHostBuilder(args).Build();
            var serviceProvider = host.Services;

            try
            {
                var grossPackage = UserInput.GetDecimalInput("Enter your gross salary package: ");
                var payFrequency = UserInput.GetPayFrequency();

                CalculateAndDisplaySalaryBreakdown(serviceProvider, grossPackage, payFrequency);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        private static void CalculateAndDisplaySalaryBreakdown(IServiceProvider serviceProvider, decimal grossPackage, PayFrequency payFrequency)
        {
            var salaryCalculator = serviceProvider.GetRequiredService<ISalaryCalculator>();
            var breakdown = salaryCalculator.CalculateSalaryBreakdown(grossPackage, payFrequency);
            Display.ShowBreakdown(breakdown);
        }
    }
}
