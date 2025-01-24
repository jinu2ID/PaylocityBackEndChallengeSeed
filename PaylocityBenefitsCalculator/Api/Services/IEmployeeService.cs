﻿using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services
{
    public interface IEmployeeService
    { 
       Task<GetEmployeeDto?> GetByIdAsync(int id);
       Task<List<GetEmployeeDto>> GetAllAsync();
       Task<GetEmployeeDto?> AddEmployeeAsync(CreateNewEmployeeDto employeeDto);
    }
}
