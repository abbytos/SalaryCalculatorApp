using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SalaryCalculatorApp.Models;
using SalaryCalculatorApp.Services; 
using SalaryCalculatorApp.Strategies;
using Xunit;
using SalaryCalculatorTests.Utils;

namespace SalaryCalculatorApp.Tests
{
    public class SalaryCalculatorTests
    {
        private IConfiguration _configuration;
        private IOptions<IncomeTaxConfig> _incomeTaxOptions;
        private IOptions<MedicareLevyConfig> _medicareLevyOptions;
        private IOptions<BudgetRepairLevyConfig> _budgetRepairLevyOptions;

        public SalaryCalculatorTests()
        {
            // Load configuration
            _configuration = ConfigHelper.LoadConfiguration();

            // Initialize options from the configuration
            _incomeTaxOptions = Options.Create(ConfigHelper.LoadConfig<IncomeTaxConfig>(_configuration, "IncomeTax"));
            _medicareLevyOptions = Options.Create(ConfigHelper.LoadConfig<MedicareLevyConfig>(_configuration, "MedicareLevy"));
            _budgetRepairLevyOptions = Options.Create(ConfigHelper.LoadConfig<BudgetRepairLevyConfig>(_configuration, "BudgetRepairLevy"));
        }

        // Test data for valid salary breakdown calculations.
        public static IEnumerable<object[]> SalaryBreakdownTestData =>
           new List<object[]>
           {
                new object[] { 65000m, PayFrequency.Monthly, 10839.00m, 1188.00m, 0.00m, 5639.27m, 47333.73m, 3944.48m },
                new object[] { 65000m, PayFrequency.Fortnightly, 10839.00m, 1188.00m, 0.00m, 5639.27m, 47333.73m, 1820.53m },
                new object[] { 65000m, PayFrequency.Weekly, 10839.00m, 1188.00m, 0.00m, 5639.27m, 47333.73m, 910.26m },
                new object[] { 200000m, PayFrequency.Monthly, 55476.00m, 3653.00m, 52.00m, 17351.60m, 123467.40m, 10288.95m },
                new object[] { 200000m, PayFrequency.Fortnightly, 55476.00m, 3653.00m, 52.00m, 17351.60m, 123467.40m, 4748.75m },
                new object[] { 200000m, PayFrequency.Weekly, 55476.00m, 3653.00m, 52.00m, 17351.60m, 123467.40m, 2374.37m },
                new object[] { 1m, PayFrequency.Monthly, 0.00m, 0.00m, 0.00m, 0.09m, 0.91m, 0.08m },
           };

        // Test data for invalid input scenarios to ensure error handling
        public static IEnumerable<object[]> InvalidInputTestData =>
            new List<object[]>
            {
                new object[] { -50000m, PayFrequency.Monthly, typeof(ArgumentOutOfRangeException) },
                new object[] { -50000m, PayFrequency.Fortnightly, typeof(ArgumentOutOfRangeException) },
                new object[] { -50000m, PayFrequency.Weekly, typeof(ArgumentOutOfRangeException) },
                new object[] { 0m, PayFrequency.Monthly, typeof(ArgumentOutOfRangeException) },
                new object[] { 0m, PayFrequency.Fortnightly, typeof(ArgumentOutOfRangeException) },
                new object[] { 0m, PayFrequency.Weekly, typeof(ArgumentOutOfRangeException) },
                new object[] { 50000m, (PayFrequency)999, typeof(ArgumentException) }
            };

        [Theory]
        [MemberData(nameof(SalaryBreakdownTestData))]
        public void CalculateSalaryBreakdown_CorrectlyCalculatesBreakdown(
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
            var salaryCalculator = new SalaryCalculator(
                new IncomeTaxStrategy(_incomeTaxOptions),
                new MedicareLevyStrategy(_medicareLevyOptions),
                new BudgetRepairLevyStrategy(_budgetRepairLevyOptions)
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

        [Theory]
        [MemberData(nameof(InvalidInputTestData))]
        public void CalculateSalaryBreakdown_HandlesErrorsCorrectly(
            decimal grossPackage,
            PayFrequency payFrequency,
            Type expectedExceptionType)
        {
           // Arrange
           var salaryCalculator = new SalaryCalculator(
               new IncomeTaxStrategy(_incomeTaxOptions),
               new MedicareLevyStrategy(_medicareLevyOptions),
               new BudgetRepairLevyStrategy(_budgetRepairLevyOptions)
           );

            // Act & Assert
            var exception = Assert.Throws(expectedExceptionType, () =>
                salaryCalculator.CalculateSalaryBreakdown(grossPackage, payFrequency)
            );

            // Verify that the correct exception is thrown
            Assert.Equal(expectedExceptionType, exception.GetType());
        }
    }
}
