namespace FotoGen.Application.Common
{
    public class BaseResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
        public T? Data { get; set; }
        public IDictionary<string, string[]>? Errors { get; set; }
        public static BaseResponse<T> Success(T? data = default, string? message = "") =>
        new()
        {
            IsSuccess = true,
            Data = data,
            Message = message ?? string.Empty,
            ErrorCode = null
        };
        public static BaseResponse<T> Fail(ApplicationErrorCode errorCode)
        {
            var errorInfo = ApplicationErrorMessage.Get(errorCode);
            return new()
            {
                IsSuccess = false,
                ErrorCode = errorCode.ToString(),
                Message = errorInfo
            };
        }
        public static BaseResponse<T> Fail(IDictionary<string, string[]>? errors) =>
            new()
            {
                IsSuccess = false,
                ErrorCode = ApplicationErrorCode.Validation.ToString(),
                Message = "Validation failed",
                Errors = errors
            };
    }
}
