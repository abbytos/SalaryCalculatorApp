using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Models;
using SalaryCalculatorApp.Strategies;

namespace SalaryCalculatorApp.Tests
{
    public class BudgetRepairLevyStrategyTests
    {
        private readonly BudgetRepairLevyConfig _config;

        public BudgetRepairLevyStrategyTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

            // Load the configuration specifically for Budget Repair Levy
            _config = configuration.GetSection("BudgetRepairLevy").Get<BudgetRepairLevyConfig>();
        }

        [Theory]
        [InlineData(170000, 0)]    // Income below threshold
        [InlineData(180000, 0)]    // Income at threshold
        [InlineData(200000, 400)] // Income above threshold
        [InlineData(250000, 1400)] // Higher income
        public void CalculateDeduction_ValidInputs_CalculatesCorrectly(decimal taxableIncome, decimal expectedDeduction)
        {
            // Arrange
            var options = Options.Create(_config);
            var strategy = new BudgetRepairLevyStrategy(options);

            // Act
            var deduction = strategy.CalculateDeduction(taxableIncome);

            // Assert
            Assert.Equal(expectedDeduction, deduction, 2); // Check with a precision of 2 decimal places
        }

        [Theory]
        [InlineData(-10)] // Negative income should be invalid
        public void CalculateDeduction_InvalidInputs_ThrowsArgumentOutOfRangeException(decimal taxableIncome)
        {
            // Arrange
            var options = Options.Create(_config);
            var strategy = new BudgetRepairLevyStrategy(options);

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                strategy.CalculateDeduction(taxableIncome)
            );

            Assert.Equal("Income must be greater than zero. (Parameter 'taxableIncome')", exception.Message);
        }
    }
}
