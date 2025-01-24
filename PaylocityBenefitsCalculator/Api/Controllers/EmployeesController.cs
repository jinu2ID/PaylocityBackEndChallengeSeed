using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Extensions;
using Api.Models;
using Api.Repositories;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class EmployeesController : ControllerBase
{
    private IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService service)
    {
        _employeeService = service;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        GetEmployeeDto? employee = await _employeeService.GetByIdAsync(id);
        // map employee to GetEmployeeDto and return
        if (employee == null)
        {
            return NotFound(new ApiResponse<GetEmployeeDto>()
            {
                Data = null,
                Error = $"Employee not found",
                Success = false
            });
        }

        return Ok(new ApiResponse<GetEmployeeDto>()
        {
            Data = employee,
            Success = true
        });
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        //task: use a more realistic production approach
        var employees = await _employeeService.GetAllAsync();

        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employees,
            Success = true
        };

        return result;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeDto"></param>
    /// <returns></returns>
    /// <remarks>
    /// </remarks>
    [SwaggerOperation(Summary = "Create new employees")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Add(CreateNewEmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var addedEmployeeDto = await _employeeService.AddEmployeeAsync(employeeDto);
            return new ApiResponse<GetEmployeeDto>()
            {
                Data = addedEmployeeDto,
                Success = true
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // return error
            throw;
        }

    }
}
