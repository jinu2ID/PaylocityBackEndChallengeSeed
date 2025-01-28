using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Exceptions;
using Api.Extensions;
using Api.Models;
using Api.Repositories;

namespace Api.Services.Employees;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        Employee? employee = await _repository.GetByIdAsync(id);
        
        if (employee == null)
        {
            throw new EmployeeNotFoundException(id);
        };

        return employee;
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        List<Employee> employees = await _repository.GetAllAsync();
        return employees;
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

    public async Task<Dependent> AddDependentAsync(CreateNewDependentDto dependentDto)
    {
        var employee = await _repository.GetByIdAsync(dependentDto.EmployeeId);
        if (employee == null)
        {
            // Add logging here
            Console.WriteLine("Employee ID for dependent not found");
            return new Dependent();
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
                return new Dependent();
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

            return addedDependent;
        }
        catch (Exception ex)
        {
            // add logging here
            Console.WriteLine($"Add new dependent exception: {ex}");
            throw;
        }
    }
}
