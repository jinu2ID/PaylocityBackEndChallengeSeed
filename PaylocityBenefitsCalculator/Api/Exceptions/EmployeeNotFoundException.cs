namespace Api.Exceptions;

public class EmployeeNotFoundException : Exception
{
    public int EmployeeId { get; }

    public EmployeeNotFoundException()
    {
    }

    public EmployeeNotFoundException(int employeeId)
        : base($"Employee with ID {employeeId} was not found.")
    {
        EmployeeId = employeeId;
    }

    public EmployeeNotFoundException(string message)
        : base(message)
    {
    }

    public EmployeeNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
