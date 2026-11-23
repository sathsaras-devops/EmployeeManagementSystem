namespace EmployeeManagement.Application.Common.Results;

public class ApiResponse<T> : IApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public IEnumerable<string> Errors { get; set; }
        = Enumerable.Empty<string>();

    public ApiResponse(
        bool success,
        string message,
        T? data,
        IEnumerable<string>? errors = null)
    {
        Success = success;
        Message = message;
        Data = data;
        Errors = errors ?? Enumerable.Empty<string>();
    }

    public static ApiResponse<T> SuccessResponse(
        T data,
        string message = "Request Successful")
    {
        return new ApiResponse<T>(
            true,
            message,
            data);
    }

    public static ApiResponse<T> ErrorResponse(
        string message = "Request Failed",
        IEnumerable<string>? errors = null)
    {
        return new ApiResponse<T>(
            false,
            message,
            default,
            errors);
    }
}


