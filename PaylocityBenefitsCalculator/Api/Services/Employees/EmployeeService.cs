using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Extensions;
using Api.Models;
using Api.Repositories;

namespace Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetEmployeeDto?> GetByIdAsync(int id)
        {
            Employee? employee = await _repository.GetByIdAsync(id);
            
            if (employee == null)
            {
                return null;
            };

            GetEmployeeDto dto = employee.ToGetEmployeeDto();
            return dto;
        }

        public async Task<List<GetEmployeeDto>> GetAllAsync()
        {
            List<Employee> employees = await _repository.GetAllAsync();
            return employees.Select(e => e.ToGetEmployeeDto()).ToList();
        }


        public async Task<GetEmployeeDto?> AddEmployeeAsync(CreateNewEmployeeDto employeeDto)
        {
            var employee = employeeDto.ToEntity();
            try
            {
                var addedEmployee = await _repository.AddEmployeeAsync(employee);

                if (addedEmployee == null)
                {
                    // add logging here
                    Console.WriteLine("Failed to add Employee");
                    return null;
                }

                return addedEmployee.ToGetEmployeeDto();
            }
            catch (Exception ex)
            {
                // add logging here
                Console.WriteLine($"Add new employee exception: {ex}");
                throw;
            }
        }

        public async Task<GetDependentDto> AddDependentAsync(CreateNewDependentDto dependentDto)
        {
            var employee = await _repository.GetByIdAsync(dependentDto.EmployeeId);
            if (employee == null)
            {
                // Add logging here
                Console.WriteLine("Employee ID for dependent not found");
                return new GetDependentDto();
            }

            // Employee can only have 1 spouse or domestic partner
            if (dependentDto.Relationship == Relationship.Spouse || dependentDto.Relationship == Relationship.DomesticPartner)
            {
                if (employee.Dependents.Any(d => 
                d.Relationship == Relationship.DomesticPartner || 
                d.Relationship == Relationship.Spouse))
                {
                    // Adding logging here
                    Console.WriteLine("Employee already has spouse or domestic partner");
                    return new GetDependentDto();
                }
            }

            var dependent = dependentDto.ToDependent();
            try
            {
                var addedDependent = await _repository.AddDependentAsync(dependent);
                if (addedDependent == null)
                {
                    // add logging here
                    Console.WriteLine("Failed to add Employee");
                    return null;
                }

                return addedDependent.ToGetDependentDto();
            }
            catch (Exception ex)
            {
                // add logging here
                Console.WriteLine($"Add new dependent exception: {ex}");
                throw;
            }
        }
    }
}
