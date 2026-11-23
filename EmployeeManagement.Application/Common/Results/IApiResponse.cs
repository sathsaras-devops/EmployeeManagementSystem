namespace EmployeeManagement.Application.Common.Results;

public interface IApiResponse
{
    bool Success { get; }

    string Message { get; }

    IEnumerable<string> Errors { get; }
}
