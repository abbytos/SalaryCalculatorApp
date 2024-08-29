using SalaryCalculatorApp.Models;
using SalaryCalculatorApp.Services;
using SalaryCalculatorApp.Strategies;

namespace SalaryCalculatorApp.Tests;

public class SalaryCalculatorTests
{
    public static IEnumerable<object[]> SalaryBreakdownTestData =>
       new List<object[]>
       {
                new object[] { 65000m, PayFrequency.Monthly, 10839.00m, 1188.00m, 0.00m, 5639.27m, 47333.73m, 3944.48m},
                new object[] { 65000m, PayFrequency.Fortnightly, 10839.00m, 1188.00m, 0.00m, 5639.27m, 47333.73m, 1820.53m },
                new object[] { 65000m, PayFrequency.Weekly, 10839.00m, 1188.00m, 0.00m, 5639.27m, 47333.73m, 910.26m },
                new object[] { 200000m, PayFrequency.Monthly, 55476.00m, 3653.00m, 53.00m, 17351.60m, 123466.40m, 10288.87m },
                new object[] { 200000m, PayFrequency.Fortnightly, 55476.00m, 3653.00m, 53.00m, 17351.60m, 123466.40m, 4748.71m },
                new object[] { 200000m, PayFrequency.Weekly, 55476.00m, 3653.00m, 53.00m, 17351.60m, 123466.40m, 2374.35m },

                // Minimum non-zero salary
                new object[] { 1m, PayFrequency.Monthly, 0.00m, 0.00m, 0.00m, 0.09m, 0.91m, 0.08m }, 
       };
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


    //[Theory]
    //[MemberData(nameof(SalaryBreakdownTestData))]
    //public void CalculateSalaryBreakdown_CorrectlyCalculatesBreakdown(
    //    decimal grossPackage,
    //    PayFrequency payFrequency,
    //    decimal expectedIncomeTax,
    //    decimal expectedMedicareLevy,
    //    decimal expectedBudgetRepairLevy,
    //    decimal expectedSuperannuation,
    //    decimal expectedNetIncome,
    //    decimal expectedPayPacketAmount)
    //{
    //    // Arrange
    //    var salaryCalculator = new SalaryCalculator(
    //        new IncomeTaxStrategy(),
    //        new MedicareLevyStrategy(),
    //        new BudgetRepairLevyStrategy()
    //    );

    //    // Act
    //    var breakdown = salaryCalculator.CalculateSalaryBreakdown(grossPackage, payFrequency);

    //    // Assert
    //    Assert.Equal(expectedIncomeTax, breakdown.IncomeTax, 2);
    //    Assert.Equal(expectedMedicareLevy, breakdown.MedicareLevy, 2);
    //    Assert.Equal(expectedBudgetRepairLevy, breakdown.BudgetRepairLevy, 2);
    //    Assert.Equal(expectedSuperannuation, breakdown.SuperContribution, 2);
    //    Assert.Equal(expectedNetIncome, breakdown.NetIncome, 2);
    //    Assert.Equal(expectedPayPacketAmount, breakdown.PayPacketAmount, 2);
    //}

    //[Theory]
    //[MemberData(nameof(InvalidInputTestData))]
    //public void CalculateSalaryBreakdown_HandlesErrorsCorrectly(
    //    decimal grossPackage,
    //    PayFrequency payFrequency,
    //    Type expectedExceptionType)
    //{
    //    // Arrange
    //    var salaryCalculator = new SalaryCalculator(
    //        new IncomeTaxStrategy(),
    //        new MedicareLevyStrategy(),
    //        new BudgetRepairLevyStrategy()
    //    );

    //    // Act & Assert
    //    var exception = Assert.Throws(expectedExceptionType, () =>
    //        salaryCalculator.CalculateSalaryBreakdown(grossPackage, payFrequency)
    //    );
    //}
}