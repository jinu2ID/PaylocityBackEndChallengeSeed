﻿using System.ComponentModel.DataAnnotations;
using Api.Dtos.Dependent;

namespace Api.Dtos.Employee;

public class CreateNewEmployeeDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [StringLength(100)]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(100)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Salary is required")]
    [Range(0, 9999999.99)]
    public decimal Salary { get; set; }

    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    public ICollection<GetDependentDto> Dependents { get; set; } = new List<GetDependentDto>();
}
