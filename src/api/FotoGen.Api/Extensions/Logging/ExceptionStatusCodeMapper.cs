using System.ComponentModel.DataAnnotations;

namespace FotoGen.Extensions.Logging;

public class ExceptionStatusCodeMapper
{
    public int GetStatusCode(Exception e)
    {
        return e switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            OperationCanceledException => StatusCodes.Status400BadRequest,
            FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
