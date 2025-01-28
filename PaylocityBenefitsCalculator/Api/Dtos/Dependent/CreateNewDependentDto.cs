using Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Dependent;

public class CreateNewDependentDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [StringLength(100)]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(100)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Dependent-Employee Relationship required")]
    [Range(0,3)]
    public Relationship Relationship { get; set; }

    [Required(ErrorMessage = "Employee ID required")]
    public int EmployeeId { get; set; }
}
