using System;
using System.Collections.Generic;
using Api.Models;
using Api.Services;
using Api.Services.Calculation;
using Moq;
using Xunit;

namespace Api.Tests.Services
{
    public class DefaultBenefitCostStrategyTests
    {
        // Common fields for all tests
        private readonly Mock<ISystemClock> _mockSystemClock;
        private readonly DefaultBenefitCostStrategy _strategy;

        public DefaultBenefitCostStrategyTests()
        {
            _mockSystemClock = new Mock<ISystemClock>();
            // Set the “current time” to a fixed date (for consistent age calculations)
            _mockSystemClock.SetupGet(x => x.UtcNow).Returns(new DateTime(2025, 1, 24, 0, 0, 0, DateTimeKind.Utc));

            _strategy = new DefaultBenefitCostStrategy(_mockSystemClock.Object);
        }

        [Fact]
        public void CalculatePaycheck_BelowThreshold_NoDependents_ReturnsExpectedNetPay()
        {
            // Arrange
            // Salary below the 80k threshold, no dependents
            var employee = new Employee
            {
                Salary = 52000m, // Example: $52,000/year
                Dependents = new List<Dependent>()
            };

            // Act
            decimal netPaycheck = _strategy.GetPaycheck(employee).NetPaycheckAmount;

            // Assert
            // - 26 paychecks per year => each paycheck is 52000 / 26 = 2000 gross
            // - Base monthly cost for benefits: $1000
            // - No dependents => no extra cost
            // - Yearly cost = $1000 * 12 = $12,000
            // - Per paycheck cost = $12,000 / 26 ≈ 461.54
            // - Net = 2000 - 461.54 = 1538.46
            // We'll verify to 2 decimal places
            Assert.Equal(1538.46m, Math.Round(netPaycheck, 2));
        }

        [Fact]
        public void CalculatePaycheck_AboveThreshold_NoDependents_IncludesHighSalaryAdditionalCost()
        {
            // Arrange
            var employee = new Employee
            {
                Salary = 100000m, // Above 80k
                Dependents = new List<Dependent>()
            };

            // Act
            decimal netPaycheck = _strategy.GetPaycheck(employee).NetPaycheckAmount;

            // Assert
            // - Base monthly cost = $1000
            // - Additional 2% on 100k => $2,000/year => $166.67/month
            // - So monthly cost = 1000 + 166.67 = 1166.67
            // - Yearly = 1166.67 * 12 ≈ 14000.04
            // - / 26 => per paycheck cost ≈ 538.46
            // - Gross per paycheck = 100000 / 26 ≈ 3846.15
            // - Net per paycheck ≈ 3846.15 - 538.46 = 3307.69
            Assert.Equal(3307.69m, Math.Round(netPaycheck, 2));
        }

        [Fact]
        public void CalculatePaycheck_WithOlderDependent_AddsAdditionalCost()
        {
            // Arrange
            var employee = new Employee
            {
                Salary = 60000m, // Below threshold
                Dependents = new List<Dependent>
                {
                    // Dependent born in 1960 => 65 years old in 2025
                    new Dependent { DateOfBirth = new DateTime(1960, 1, 1) },
                }
            };

            // Act
            decimal netPaycheck = _strategy.GetPaycheck(employee).NetPaycheckAmount;

            // Assert
            // Calculation steps:
            // 1) Base monthly cost = $1000
            // 2) Dependent monthly cost = $600
            // 3) Dependent is over 50 => + $200 => total dependent monthly = $800
            // 4) Total monthly cost = $1000 + $800 = $1800
            // 5) Yearly cost = 1800 * 12 = $21600
            // 6) Per paycheck cost = 21600 / 26 ≈ 830.77
            // 7) Gross paycheck = 60000 / 26 ≈ 2307.69
            // 8) Net = 2307.69 - 830.77 = 1476.92
            Assert.Equal(1476.92m, Math.Round(netPaycheck, 2));
        }

        [Fact]
        public void CalculatePaycheck_WithMultipleDependents_MixedAges()
        {
            // Arrange
            var employee = new Employee
            {
                Salary = 60000m,
                Dependents = new List<Dependent>
                {
                    // Over 50 (1950 => 75 y/o in 2025)
                    new Dependent { DateOfBirth = new DateTime(1950, 6, 15) },
                    // Under 50 (1980 => 45 y/o in 2025)
                    new Dependent { DateOfBirth = new DateTime(1980, 10, 1) }
                }
            };

            // Act
            decimal netPaycheck = _strategy.GetPaycheck(employee).NetPaycheckAmount;

            // Assert
            // Steps:
            // 1) Base monthly = $1000
            // 2) 2 dependents => 2 * 600 = $1200
            // 3) First dependent over 50 => +$200 => total = $1400 for dependents
            // 4) monthlyCost = 1000 + 1400 = $2400
            // 5) yearlyCost = 2400 * 12 = $28800
            // 6) perPaycheckCost = 28800 / 26 ≈ 1107.69
            // 7) grossPerCheck = 60000 / 26 ≈ 2307.69
            // 8) net = 2307.69 - 1107.69 = 1200.00
            Assert.Equal(1200.00m, Math.Round(netPaycheck, 2));
        }

        [Fact]
        public void GetAge_WhenBirthdayHasNotOccurredYetThisYear_SubtractsOne()
        {
            // Arrange
            // Let's say today is 2025-01-24 from our mock clock.
            // Person's birth date is 2025-03-01 => not had birthday in 2025 yet => should subtract one
            var birthday = new DateTime(1975, 3, 1);
            var privateMethod = typeof(DefaultBenefitCostStrategy)
                .GetMethod("GetAge", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            int age = (int)privateMethod.Invoke(_strategy, new object[] { birthday, _mockSystemClock.Object.UtcNow });

            // Assert
            // If they were born in 1975, in 2025 they'd normally be 50,
            // but their birthday hasn't happened yet (Mar 1), so it should return 49.
            Assert.Equal(49, age);
        }

        [Fact]
        public void GetAge_WhenBirthdayOccurredEarlierThisYear_ReturnsExpectedAge()
        {
            // Arrange
            // Mock clock says it's 2025-01-24. Let’s pretend the birthday was 1975-01-01 => already had it this year.
            var birthday = new DateTime(1975, 1, 1);
            var privateMethod = typeof(DefaultBenefitCostStrategy)
                .GetMethod("GetAge", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            int age = (int)privateMethod.Invoke(_strategy, new object[] { birthday, _mockSystemClock.Object.UtcNow });

            // Assert
            // They turn 50 on 2025-01-01, so now it's 2025-01-24 => they are indeed 50
            Assert.Equal(50, age);
        }
    }
}
