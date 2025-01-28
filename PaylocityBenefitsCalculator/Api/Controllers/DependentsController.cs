using Api.Dtos.Dependent;
using Api.Extensions;
using Api.Exceptions;
using Api.Models;
using Api.Services.Employees;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Api.Dtos.Employee;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private IEmployeeService _employeeService;

    public DependentsController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        try
        {
            Dependent? dependent = await _employeeService.GetDependentByIdAsync(id);
            if (dependent != null)
            {
                GetDependentDto dependentDto = dependent.ToGetDependentDto();

                return Ok(new ApiResponse<GetDependentDto>()
                {
                    Data = dependentDto,
                    Success = true
                });
            }
        }
        catch (DependentNotFoundException ex)
        {
            // Add logging here
            Console.WriteLine($"Dependent ID: {id} not found.", ex.Message, ex.InnerException);
        }

        return NotFound(new ApiResponse<GetEmployeeDto>()
        {
            Data = null,
            Error = $"Dependent not found",
            Success = false
        });
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _employeeService.GetAllDependentsAsync();
        List<GetDependentDto> dependentDtos = dependents.Select(d => d.ToGetDependentDto()).ToList();

        var result = new ApiResponse<List<GetDependentDto>>()
        {
            Data = dependentDtos,
            Success = true
        };

        return result;
    }
}
