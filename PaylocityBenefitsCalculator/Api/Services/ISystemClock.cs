namespace Api.Services;

public interface ISystemClock
{
    DateTime UtcNow { get; }
}
