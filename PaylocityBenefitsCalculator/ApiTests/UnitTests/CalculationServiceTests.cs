using System;
using System.Threading.Tasks;
using Api.Exceptions;
using Api.Models;
using Api.Services.Calculation;
using Api.Services.Calculation.Strategy;
using Api.Services.Employees;
using Moq;
using Xunit;

namespace Api.Tests.Services.Calculation
{
    public class CalculationServiceTests
    {
        private readonly Mock<IEmployeeService> _mockEmployeeService;
        private readonly Mock<IBenefitCostStrategyFactory> _mockBenefitCostStrategyFactory;
        private readonly Mock<IBenefitCostStrategy> _mockBenefitCostStrategy;
        private readonly CalculationService _calculationService;

        public CalculationServiceTests()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _mockBenefitCostStrategyFactory = new Mock<IBenefitCostStrategyFactory>();
            _mockBenefitCostStrategy = new Mock<IBenefitCostStrategy>();

            // System under test
            _calculationService = new CalculationService(
                _mockEmployeeService.Object,
                _mockBenefitCostStrategyFactory.Object
            );
        }

        [Fact]
        public async Task GetPaycheckAsync_EmployeeNotFound_ThrowsException()
        {
            // Arrange
            int employeeId = 123;
            _mockEmployeeService
                .Setup(s => s.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync((Api.Models.Employee)null); // simulate no employee found

            // Act & Assert
            await Assert.ThrowsAsync<EmployeeNotFoundException>(() =>
                _calculationService.GetPaycheckAsync(employeeId));
        }

        [Fact]
        public async Task GetPaycheckAsync_ValidEmployee_ReturnsPaycheckFromStrategy()
        {
            // Arrange
            int employeeId = 123;
            var employee = new Employee
            {
                Id = employeeId,
                Salary = 100000m
            };
            var expectedPaycheck = new Paycheck
            {
                NetPaycheckAmount = 2000m
            };

            _mockEmployeeService
                .Setup(s => s.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync(employee);

            _mockBenefitCostStrategyFactory
                .Setup(f => f.GetStrategy(employee))
                .Returns(_mockBenefitCostStrategy.Object);

            _mockBenefitCostStrategy
                .Setup(st => st.GetPaycheck(employee))
                .Returns(expectedPaycheck);

            // Act
            var actualPaycheck = await _calculationService.GetPaycheckAsync(employeeId);

            // Assert
            Assert.NotNull(actualPaycheck);
            Assert.Equal(expectedPaycheck.NetPaycheckAmount, actualPaycheck.NetPaycheckAmount);

            // Verify that the service calls the strategy as expected
            _mockEmployeeService.Verify(s => s.GetEmployeeByIdAsync(employeeId), Times.Once);
            _mockBenefitCostStrategyFactory.Verify(f => f.GetStrategy(employee), Times.Once);
            _mockBenefitCostStrategy.Verify(st => st.GetPaycheck(employee), Times.Once);
        }

        [Fact]
        public async Task GetPaycheckNetAmountAsync_ReturnsNetPaycheckAmount()
        {
            // Arrange
            int employeeId = 456;
            var employee = new Employee { Id = employeeId, Salary = 50000m };
            var paycheck = new Paycheck
            {
                NetPaycheckAmount = 1500m
            };

            _mockEmployeeService
                .Setup(s => s.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync(employee);

            _mockBenefitCostStrategyFactory
                .Setup(f => f.GetStrategy(employee))
                .Returns(_mockBenefitCostStrategy.Object);

            _mockBenefitCostStrategy
                .Setup(st => st.GetPaycheck(employee))
                .Returns(paycheck);

            // Act
            decimal netAmount = await _calculationService.GetPaycheckNetAmountAsync(employeeId);

            // Assert
            Assert.Equal(1500m, netAmount);
        }
    }
}
