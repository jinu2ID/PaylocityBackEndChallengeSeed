using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Exceptions;
using Api.Extensions;
using Api.Models;
using Api.Services.Calculation;
using Api.Services.Employees;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class EmployeesController : ControllerBase
{
    private IEmployeeService _employeeService;
    private ICalculationService _calculationService;

    public EmployeesController(IEmployeeService service, ICalculationService calculationService)
    {
        _employeeService = service;
        _calculationService = calculationService;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        try
        {
            Employee? employee = await _employeeService.GetByIdAsync(id);
            if (employee != null)
            {
                GetEmployeeDto employeeDto = employee.ToGetEmployeeDto();
            
                return Ok(new ApiResponse<GetEmployeeDto>()
                {
                    Data = employeeDto,
                    Success = true
                });
            }
        }
        catch(EmployeeNotFoundException ex)
        {
            // Adding logging here
            Console.WriteLine($"Employee ID: {id} not found.", ex.Message, ex.InnerException);
        }

        return NotFound(new ApiResponse<GetEmployeeDto>()
        {
            Data = null,
            Error = $"Employee not found",
            Success = false
        });
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        //task: use a more realistic production approach
        var employees = await _employeeService.GetAllAsync();
        List<GetEmployeeDto> employeeDtos = employees.Select(e => e.ToGetEmployeeDto()).ToList();

        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = employeeDtos,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Create new employees")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Add(CreateNewEmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var addedEmployeeDto = await _employeeService.AddEmployeeAsync(employeeDto);

            if (addedEmployeeDto != null)
            {
                return new ApiResponse<GetEmployeeDto>()
                {
                    Data = addedEmployeeDto,
                    Success = true
                };
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return new ApiResponse<GetEmployeeDto>()
        {
            Message = "Unable to add new Employee",
            Success = false
        };
    }

    [SwaggerOperation(Summary = "Get employee paycheck")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Paycheck>>> GetPaycheck(int employeeId)
    {
        try
        {
            var paycheck = await _calculationService.GetPaycheckAsync(employeeId);
            return new ApiResponse<Paycheck>()
            {
                Data = paycheck,
                Success = true
            };
        }
        catch (EmployeeNotFoundException ex)
        {
            Console.WriteLine($"Employee ID: {employeeId} not found.", ex.Message, ex.InnerException);
            return NotFound(new ApiResponse<GetEmployeeDto>()
            {
                Data = null,
                Error = $"Employee not found",
                Success = false
            });
        }
       
    }

    [SwaggerOperation(Summary = "Get employee paycheck net amount")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Decimal>>> GetPaycheckNetAmount(int employeeId)
    {
        try
        {
            var netAmount = await _calculationService.GetPaycheckNetAmountAsync(employeeId);
            return new ApiResponse<Decimal>()
            {
                Data = netAmount,
                Success = true
            };
        }
        catch (EmployeeNotFoundException ex)
        {
            Console.WriteLine($"Employee ID: {employeeId} not found.", ex.Message, ex.InnerException);
            return NotFound(new ApiResponse<GetEmployeeDto>()
            {
                Data = null,
                Error = $"Employee not found",
                Success = false
            });
        }


    }
}
