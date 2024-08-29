using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Models;
using SalaryCalculatorApp.Strategies;

namespace SalaryCalculatorApp.Tests
{
    public class MedicareLevyStrategyTests
    {
        private readonly MedicareLevyConfig _config;

        public MedicareLevyStrategyTests()
        {
            // Set up the configuration builder to read from the config.json file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the current directory
                .AddJsonFile("config.json") // Add the JSON configuration file
                .Build(); // Build the configuration

            // Load the configuration specifically for Medicare Levy
            // Load the configuration specifically for Medicare Levy
            _config = configuration.GetSection("MedicareLevy").Get<MedicareLevyConfig>()
                ?? throw new InvalidOperationException("MedicareLevy configuration is missing");
        }

        [Theory]
        [InlineData(20000, 0)] // Test case with taxable income of 20,000 expecting Medicare Levy of 0
        [InlineData(22000, 67)] // Test case with taxable income of 22,000 expecting Medicare Levy of 67
        public void CalculateMedicareLevy_WithoutConfiguration_CorrectlyCalculates(decimal taxableIncome, decimal expectedLevy)
        {
            // Arrange
            var options = Options.Create(_config); // Create an Options instance with the loaded configuration
            var strategy = new MedicareLevyStrategy(options); // Instantiate the MedicareLevyStrategy with the options

            // Act
            var result = strategy.CalculateDeduction(taxableIncome); // Calculate the Medicare Levy based on the taxable income

            // Assert
            Assert.Equal(expectedLevy, result, 3); // Verify that the calculated levy matches the expected levy with a precision of 3 decimal places
        }

        [Theory]
        [InlineData(-5000)] // Test case with negative taxable income expecting an exception
        [InlineData(0)]     // Test case with zero taxable income expecting an exception
        public void CalculateDeduction_InvalidInputs_ThrowsArgumentOutOfRangeException(decimal taxableIncome)
        {
            // Arrange
            var options = Options.Create(_config); // Create an Options instance with a new, empty configuration
            var strategy = new MedicareLevyStrategy(options); // Instantiate the MedicareLevyStrategy with the options

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                strategy.CalculateDeduction(taxableIncome) // Call the method with invalid input
            );

            // Verify that the correct exception is thrown with the expected message
            Assert.Equal("Taxable income must be greater than zero. (Parameter 'taxableIncome')", exception.Message);
        }
    }
}
