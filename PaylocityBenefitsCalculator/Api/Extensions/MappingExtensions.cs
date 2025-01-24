using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Extensions
{
    public static class MappingExtensions
    {
        // Maps Employee to GetEmployeeDto
        public static GetEmployeeDto ToGetEmployeeDto(this Employee employee)
        {
            return new GetEmployeeDto()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Salary = employee.Salary,
                DateOfBirth = employee.DateOfBirth,
                Dependents = employee.Dependents?.Select(d => d.ToGetDependentDto()).ToList()
            };
        }

        // Maps CreateNewEmployeeDto to Employee
        public static Employee ToEntity(this CreateNewEmployeeDto dto)
        {
            return new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Salary = dto.Salary,
                DateOfBirth = dto.DateOfBirth
            };
        }

        public static GetDependentDto ToGetDependentDto(this Dependent dependent)
        {
            return new GetDependentDto()
            {
                Id = dependent.Id,
                FirstName = dependent.FirstName,
                LastName = dependent.LastName,
                DateOfBirth = dependent.DateOfBirth,
                Relationship = dependent.Relationship
            };
        }

        public static Dependent ToDependent(this CreateNewDependentDto dto)
        {
            return new Dependent()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Relationship = dto.Relationship,
                EmployeeId = dto.EmployeeId
            };
        }
    }
}
