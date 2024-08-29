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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

            // Load the configuration specifically for Medicare Levy
            _config = configuration.GetSection("MedicareLevy").Get<MedicareLevyConfig>();
        }


        [Theory]
        [InlineData(20000, 0)]
        [InlineData(22000, 67)]
        public void CalculateMedicareLevy_WithoutConfiguration_CorrectlyCalculates(decimal taxableIncome, decimal expectedLevy)
        {
            // Arrange
            var options = Options.Create(_config);
            var strategy = new MedicareLevyStrategy(options);

            // Act
            var result = strategy.CalculateDeduction(taxableIncome);

            // Assert
            Assert.Equal(expectedLevy, result, 3);
        }


        [Theory]
        [InlineData(-5000)]
        [InlineData(0)]
        public void CalculateDeduction_InvalidInputs_ThrowsArgumentOutOfRangeException(decimal taxableIncome)
        {
            // Arrange
            var options = Options.Create(new MedicareLevyConfig());
            var strategy = new MedicareLevyStrategy(options);

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                strategy.CalculateDeduction(taxableIncome)
            );

            Assert.Equal("Taxable income must be greater than zero. (Parameter 'taxableIncome')", exception.Message);
        }
    }
}
