using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Models;
using SalaryCalculatorApp.Strategies;
using SalaryCalculatorApp.Services;
using SalaryCalculatorTests.Utils;

namespace SalaryCalculatorApp.Tests
{
    public class SalaryCalculatorIntegrationTests
    {
        private readonly IncomeTaxConfig _incomeTaxConfig;
        private readonly MedicareLevyConfig _medicareLevyConfig;
        private readonly BudgetRepairLevyConfig _budgetRepairLevyConfig;
        private IOptions<SalarySettingsConfig> _salarySettingsOptions;

        public SalaryCalculatorIntegrationTests()
        {
            // Load configuration from a JSON file.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

            // Bind configuration sections to config models using ConfigHelper.
            _incomeTaxConfig = ConfigHelper.LoadConfig<IncomeTaxConfig>(configuration, "IncomeTax");
            _medicareLevyConfig = ConfigHelper.LoadConfig<MedicareLevyConfig>(configuration, "MedicareLevy");
            _budgetRepairLevyConfig = ConfigHelper.LoadConfig<BudgetRepairLevyConfig>(configuration, "BudgetRepairLevy");
            _salarySettingsOptions = Options.Create(ConfigHelper.LoadConfig<SalarySettingsConfig>(configuration, "SalarySettings"));
        }

        public static IEnumerable<object[]> SalaryBreakdownTestData =>
            new List<object[]>
            {
                new object[] { 65000m, PayFrequency.Monthly, 10839.00m, 1188.00m, 0.00m, 5639.27m, 47333.73m, 3944.48m},
                new object[] { 200000m, PayFrequency.Fortnightly, 55476.00m, 3653.00m, 52.00m, 17351.60m, 123467.40m, 4748.75m},

                // Minimum non-zero salary test case
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
            // Create Options instances for deduction strategies
            var optionsIncomeTax = Options.Create(_incomeTaxConfig);
            var optionsMedicareLevy = Options.Create(_medicareLevyConfig);
            var optionsBudgetRepairLevy = Options.Create(_budgetRepairLevyConfig);

            // Instantiate DeductionCalculator with the strategies
            var deductionCalculator = new DeductionCalculator(
                new IncomeTaxStrategy(optionsIncomeTax),
                new MedicareLevyStrategy(optionsMedicareLevy),
                new BudgetRepairLevyStrategy(optionsBudgetRepairLevy)
            );

            // Instantiate SalaryCalculator with DeductionCalculator and SalarySettings
            var salaryCalculator = new SalaryCalculator(
                deductionCalculator,
                _salarySettingsOptions
            );

            // Act
            var breakdown = salaryCalculator.CalculateSalaryBreakdown(grossPackage, payFrequency);

            // Assert
            // Validate results with expected values, allowing for precision up to 2 decimal places
            Assert.Equal(expectedIncomeTax, breakdown.IncomeTax, 2);
            Assert.Equal(expectedMedicareLevy, breakdown.MedicareLevy, 2);
            Assert.Equal(expectedBudgetRepairLevy, breakdown.BudgetRepairLevy, 2);
            Assert.Equal(expectedSuperannuation, breakdown.SuperContribution, 2);
            Assert.Equal(expectedNetIncome, breakdown.NetIncome, 2);
            Assert.Equal(expectedPayPacketAmount, breakdown.PayPacketAmount, 2);
        }
    }
}
