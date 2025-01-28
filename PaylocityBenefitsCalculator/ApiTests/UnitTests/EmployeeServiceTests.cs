using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Exceptions;
using Api.Extensions;
using Api.Models;
using Api.Repositories;
using Api.Services.Employees;
using Moq;
using Xunit;

namespace Api.Tests.Services.Employees
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepository;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _mockRepository = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_mockRepository.Object);
        }

        #region GetEmployeeByIdAsync

        [Fact]
        public async Task GetEmployeeByIdAsync_EmployeeExists_ReturnsEmployee()
        {
            // Arrange
            int employeeId = 123;
            var employee = new Employee { Id = employeeId, Salary = 50000m };

            _mockRepository.Setup(r => r.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync(employee);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(employeeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employeeId, result.Id);
            _mockRepository.Verify(r => r.GetEmployeeByIdAsync(employeeId), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_EmployeeDoesNotExist_ThrowsEmployeeNotFoundException()
        {
            // Arrange
            int employeeId = 456;
            _mockRepository.Setup(r => r.GetEmployeeByIdAsync(employeeId))
                .ReturnsAsync((Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<EmployeeNotFoundException>(() =>
                _employeeService.GetEmployeeByIdAsync(employeeId));
        }

        #endregion

        #region GetAllEmployeesAsync

        [Fact]
        public async Task GetAllEmployeesAsync_ReturnsListOfEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Salary = 40000m },
                new Employee { Id = 2, Salary = 50000m }
            };

            _mockRepository.Setup(r => r.GetAllEmployeesAsync())
                .ReturnsAsync(employees);

            // Act
            var result = await _employeeService.GetAllEmployeesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mockRepository.Verify(r => r.GetAllEmployeesAsync(), Times.Once);
        }

        #endregion

        #region AddEmployeeAsync

        [Fact]
        public async Task AddEmployeeAsync_Success_ReturnsGetEmployeeDto()
        {
            // Arrange
            var createDto = new CreateNewEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Salary = 60000m
            };
            var employeeEntity = createDto.ToEntity(); // from the extension method
            var addedEmployee = new Employee
            {
                Id = 101,
                FirstName = "John",
                LastName = "Doe",
                Salary = 60000m
            };

            _mockRepository.Setup(r => r.AddEmployeeAsync(It.IsAny<Employee>()))
                .ReturnsAsync(addedEmployee);

            // Act
            var result = await _employeeService.AddEmployeeAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            _mockRepository.Verify(r => r.AddEmployeeAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public async Task AddEmployeeAsync_RepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            var createDto = new CreateNewEmployeeDto
            {
                FirstName = "Jane",
                LastName = "Smith"
            };

            _mockRepository.Setup(r => r.AddEmployeeAsync(It.IsAny<Employee>()))
                .ReturnsAsync((Employee)null);

            // Act
            var result = await _employeeService.AddEmployeeAsync(createDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddEmployeeAsync_RepositoryThrowsException_RethrowsException()
        {
            // Arrange
            var createDto = new CreateNewEmployeeDto { FirstName = "Bob", LastName = "Marley" };

            _mockRepository.Setup(r => r.AddEmployeeAsync(It.IsAny<Employee>()))
                .ThrowsAsync(new Exception("DB Error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _employeeService.AddEmployeeAsync(createDto));
        }

        #endregion

        #region AddDependentAsync

        [Fact]
        public async Task AddDependentAsync_EmployeeNotFound_ReturnsEmptyDependent()
        {
            // Arrange
            var depDto = new CreateNewDependentDto
            {
                EmployeeId = 999,
                FirstName = "SpouseName",
                Relationship = Relationship.Spouse
            };

            _mockRepository.Setup(r => r.GetEmployeeByIdAsync(depDto.EmployeeId))
                .ReturnsAsync((Employee)null);

            // Act
            var result = await _employeeService.AddDependentAsync(depDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            // We expect an "empty" Dependent if employee is null
            _mockRepository.Verify(r => r.AddDependentAsync(It.IsAny<Dependent>()), Times.Never);
        }

        [Fact]
        public async Task AddDependentAsync_EmployeeAlreadyHasSpouse_ReturnsEmptyDependent()
        {
            // Arrange
            var existingDependent = new Dependent { Relationship = Relationship.Spouse };
            var employee = new Employee
            {
                Id = 123,
                Dependents = new List<Dependent> { existingDependent }
            };

            var newDepDto = new CreateNewDependentDto
            {
                EmployeeId = 123,
                Relationship = Relationship.DomesticPartner
            };

            _mockRepository.Setup(r => r.GetEmployeeByIdAsync(123))
                .ReturnsAsync(employee);

            // Act
            var result = await _employeeService.AddDependentAsync(newDepDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
            // We do not call AddDependentAsync because we short-circuit
            _mockRepository.Verify(r => r.AddDependentAsync(It.IsAny<Dependent>()), Times.Never);
        }

        [Fact]
        public async Task AddDependentAsync_SuccessfulAdd_ReturnsDependent()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 555,
                Dependents = new List<Dependent>()
            };
            var depDto = new CreateNewDependentDto
            {
                EmployeeId = 555,
                FirstName = "ChildName",
                Relationship = Relationship.Child
            };

            var dependentToAdd = depDto.ToDependent();
            var addedDependent = new Dependent
            {
                Id = 1000,
                FirstName = "ChildName",
                Relationship = Relationship.Child,
                EmployeeId = 555
            };

            _mockRepository.Setup(r => r.GetEmployeeByIdAsync(employee.Id))
                .ReturnsAsync(employee);

            _mockRepository.Setup(r => r.AddDependentAsync(It.IsAny<Dependent>()))
                .ReturnsAsync(addedDependent);

            // Act
            var result = await _employeeService.AddDependentAsync(depDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1000, result.Id);
            _mockRepository.Verify(r => r.GetEmployeeByIdAsync(employee.Id), Times.Once);
            _mockRepository.Verify(r => r.AddDependentAsync(It.IsAny<Dependent>()), Times.Once);
        }

        [Fact]
        public async Task AddDependentAsync_AddReturnsNull_LogsAndReturnsNull()
        {
            // Arrange
            var employee = new Employee { Id = 777, Dependents = new List<Dependent>() };
            var depDto = new CreateNewDependentDto
            {
                EmployeeId = 777,
                FirstName = "Other",
                Relationship = Relationship.Child
            };

            _mockRepository.Setup(r => r.GetEmployeeByIdAsync(777))
                .ReturnsAsync(employee);

            _mockRepository.Setup(r => r.AddDependentAsync(It.IsAny<Dependent>()))
                .ReturnsAsync((Dependent)null);

            // Act
            var result = await _employeeService.AddDependentAsync(depDto);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(r => r.AddDependentAsync(It.IsAny<Dependent>()), Times.Once);
        }

        [Fact]
        public async Task AddDependentAsync_RepositoryThrowsException_RethrowsException()
        {
            // Arrange
            var employee = new Employee { Id = 1, Dependents = new List<Dependent>() };
            var depDto = new CreateNewDependentDto
            {
                EmployeeId = 1,
                FirstName = "ErrorChild",
                Relationship = Relationship.Child
            };

            _mockRepository.Setup(r => r.GetEmployeeByIdAsync(1))
                .ReturnsAsync(employee);

            _mockRepository.Setup(r => r.AddDependentAsync(It.IsAny<Dependent>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _employeeService.AddDependentAsync(depDto));
        }

        #endregion

        #region GetDependentByIdAsync

        [Fact]
        public async Task GetDependentByIdAsync_DependentExists_ReturnsDependent()
        {
            // Arrange
            int depId = 999;
            var dep = new Dependent { Id = depId, FirstName = "Dependency", Relationship = Relationship.Child };

            _mockRepository.Setup(r => r.GetDependentByIdAsync(depId))
                .ReturnsAsync(dep);

            // Act
            var result = await _employeeService.GetDependentByIdAsync(depId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(depId, result.Id);
            _mockRepository.Verify(r => r.GetDependentByIdAsync(depId), Times.Once);
        }

        [Fact]
        public async Task GetDependentByIdAsync_DependentNotFound_ThrowsDependentNotFoundException()
        {
            // Arrange
            int depId = 111;
            _mockRepository.Setup(r => r.GetDependentByIdAsync(depId))
                .ReturnsAsync((Dependent)null);

            // Act & Assert
            await Assert.ThrowsAsync<DependentNotFoundException>(() =>
                _employeeService.GetDependentByIdAsync(depId));
        }

        #endregion

        #region GetAllDependentsAsync

        [Fact]
        public async Task GetAllDependentsAsync_ReturnsListOfDependents()
        {
            // Arrange
            var dependents = new List<Dependent>
            {
                new Dependent { Id = 1, FirstName = "Dep1" },
                new Dependent { Id = 2, FirstName = "Dep2" }
            };

            _mockRepository.Setup(r => r.GetAllDependentsAsync())
                .ReturnsAsync(dependents);

            // Act
            var result = await _employeeService.GetAllDependentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mockRepository.Verify(r => r.GetAllDependentsAsync(), Times.Once);
        }

        #endregion
    }
}
