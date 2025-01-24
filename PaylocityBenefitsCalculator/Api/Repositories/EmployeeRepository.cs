﻿using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Dependents)
                .OrderBy(e => e.Id)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Employee>> GetPagedAsync(int pageIndex, int pageSize)
        {
            return await _context.Employees
                .Include(e => e.Dependents)
                .AsNoTracking()
                .OrderBy(e => e.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Dependents)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee?> AddEmployeeAsync(Employee? employee)
        {
            try
            {
                if (employee == null)
                {
                    return null;
                }

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                return employee;
            }
            catch (DbUpdateException ex)
            {
                // add logging here
                Console.WriteLine($"Failed to add new employee to Database: {ex}");
                throw;
            }
        }

        public async Task<Dependent?> AddDependentAsync(Dependent dependent)
        {
            try
            {
                if (dependent == null)
                {
                    return null;
                }

                await _context.Dependents.AddAsync(dependent);
                await _context.SaveChangesAsync();

                return dependent;
            }
            catch (DbUpdateException ex)
            {
                // add logging here
                Console.WriteLine($"Failed to add new employee to Database: {ex}");
                throw;
            }
        }
    }
}
