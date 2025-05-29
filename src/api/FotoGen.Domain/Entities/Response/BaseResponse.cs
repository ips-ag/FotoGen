namespace FotoGen.Domain.Entities.Response;

public class BaseResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ErrorCode { get; set; }
    public T? Data { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }

    public static BaseResponse<T> Success(T? data = default, string? message = "")
    {
        return new BaseResponse<T>
        {
            IsSuccess = true, Data = data, Message = message ?? string.Empty, ErrorCode = null
        };
    }

    public static BaseResponse<T> Fail(ErrorCode errorCode)
    {
        string errorInfo = ErrorMessage.Get(errorCode);
        return new BaseResponse<T> { IsSuccess = false, ErrorCode = errorCode.ToString(), Message = errorInfo };
    }

    public static BaseResponse<T> Fail(IDictionary<string, string[]>? errors)
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            ErrorCode = Response.ErrorCode.Validation.ToString("G"),
            Message = "Validation failed",
            Errors = errors
        };
    }
}
