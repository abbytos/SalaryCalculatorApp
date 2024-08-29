using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Models;
using SalaryCalculatorApp.Strategies;

public class IncomeTaxStrategyTests
{
    private readonly IncomeTaxConfig _config;

    public IncomeTaxStrategyTests()
    {
        // Set up the configuration builder to read from the config.json file
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the current directory
            .AddJsonFile("config.json") // Add the JSON configuration file
            .Build(); // Build the configuration

        // Bind the "IncomeTax" section of the configuration to the IncomeTaxConfig class
        _config = configuration.GetSection("IncomeTax").Get<IncomeTaxConfig>() 
            ?? throw new InvalidOperationException("IncomeTax configuration is missing");

    }

    [Theory]
    [InlineData(50000, 7797)] // Test case with taxable income of 50,000 expecting tax of 7,797
    public void CalculateDeduction_ValidIncome_ReturnsExpectedTax(decimal taxableIncome, decimal expectedTax)
    {
        // Arrange
        var options = Options.Create(_config); // Create an Options instance with the loaded configuration
        var incomeTaxStrategy = new IncomeTaxStrategy(options); // Instantiate the IncomeTaxStrategy with the options

        // Act
        var actualTax = incomeTaxStrategy.CalculateDeduction(taxableIncome); // Calculate the income tax based on the taxable income

        // Assert
        Assert.Equal(expectedTax, actualTax); // Verify that the calculated tax matches the expected tax
    }
}
