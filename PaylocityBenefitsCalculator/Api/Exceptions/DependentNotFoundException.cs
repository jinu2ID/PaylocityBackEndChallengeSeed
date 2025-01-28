namespace Api.Exceptions;

public class DependentNotFoundException : Exception
{
    public int DependentId { get; }

    public DependentNotFoundException()
    {
    }

    public DependentNotFoundException(int dependentId)
        : base($"Dependent with ID {dependentId} was not found.")
    {
        DependentId = dependentId;
    }

    public DependentNotFoundException(string message)
        : base(message)
    {
    }

    public DependentNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
