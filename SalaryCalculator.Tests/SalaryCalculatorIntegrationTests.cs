using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Models;
using SalaryCalculatorApp.Services;
using SalaryCalculatorApp.Strategies;
using SalaryCalculatorTests.Utils;

namespace SalaryCalculatorApp.Tests
{
    public class SalaryCalculatorIntegrationTests
    {
        private readonly IncomeTaxConfig _incomeTaxConfig;
        private readonly MedicareLevyConfig _medicareLevyConfig;
        private readonly BudgetRepairLevyConfig _budgetRepairLevyConfig;

        public SalaryCalculatorIntegrationTests()
        {
            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

            // Use ConfigHelper to bind configuration sections to config models
            _incomeTaxConfig = ConfigHelper.LoadConfig<IncomeTaxConfig>(configuration, "IncomeTax");
            _medicareLevyConfig = ConfigHelper.LoadConfig<MedicareLevyConfig>(configuration, "MedicareLevy");
            _budgetRepairLevyConfig = ConfigHelper.LoadConfig<BudgetRepairLevyConfig>(configuration, "BudgetRepairLevy");
        }

        public static IEnumerable<object[]> SalaryBreakdownTestData =>
            new List<object[]>
            {
                new object[] { 65000m, PayFrequency.Monthly, 10839.00m, 1188.00m, 0.00m, 5639.27m, 47333.73m, 3944.48m},
                new object[] { 200000m, PayFrequency.Fortnightly, 55476.00m, 3653.00m, 52.00m, 17351.60m, 123467.40m, 4748.75m},

                // Minimum non-zero salary
                new object[] { 1m, PayFrequency.Monthly, 0.00m, 0.00m, 0.00m, 0.09m, 0.91m, 0.08m },
            };

        [Theory]
        [MemberData(nameof(SalaryBreakdownTestData))]
        public void CalculateSalaryBreakdown_IntegrationTests_ValidInputs_CorrectlyCalculates(
            decimal grossPackage,
            PayFrequency payFrequency,
            decimal expectedIncomeTax,
            decimal expectedMedicareLevy,
            decimal expectedBudgetRepairLevy,
            decimal expectedSuperannuation,
            decimal expectedNetIncome,
            decimal expectedPayPacketAmount)
        {
            // Arrange
            var optionsIncomeTax = Options.Create(_incomeTaxConfig);
            var optionsMedicareLevy = Options.Create(_medicareLevyConfig);
            var optionsBudgetRepairLevy = Options.Create(_budgetRepairLevyConfig);

            var salaryCalculator = new SalaryCalculator(
                new IncomeTaxStrategy(optionsIncomeTax),
                new MedicareLevyStrategy(optionsMedicareLevy),
                new BudgetRepairLevyStrategy(optionsBudgetRepairLevy)
            );

            // Act
            var breakdown = salaryCalculator.CalculateSalaryBreakdown(grossPackage, payFrequency);

            // Assert
            Assert.Equal(expectedIncomeTax, breakdown.IncomeTax, 2);
            Assert.Equal(expectedMedicareLevy, breakdown.MedicareLevy, 2);
            Assert.Equal(expectedBudgetRepairLevy, breakdown.BudgetRepairLevy, 2);
            Assert.Equal(expectedSuperannuation, breakdown.SuperContribution, 2);
            Assert.Equal(expectedNetIncome, breakdown.NetIncome, 2);
            Assert.Equal(expectedPayPacketAmount, breakdown.PayPacketAmount, 2);
        }
    }
}
