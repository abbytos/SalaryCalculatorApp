using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Models;
using SalaryCalculatorApp.Strategies;

public class IncomeTaxStrategyTests
{
    private readonly IncomeTaxConfig _config;

    public IncomeTaxStrategyTests()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json")
            .Build();

        _config = configuration.GetSection("IncomeTax").Get<IncomeTaxConfig>();
    }

    [Theory]
    [InlineData(50000, 7797)]
    public void CalculateDeduction_ValidIncome_ReturnsExpectedTax(decimal taxableIncome, decimal expectedTax)
    {
        // Act
        var options = Options.Create(_config);
        var incomeTaxStrategy = new IncomeTaxStrategy(options);
        var actualTax = incomeTaxStrategy.CalculateDeduction(taxableIncome);

        // Assert
        Assert.Equal(expectedTax, actualTax);
    }
}
