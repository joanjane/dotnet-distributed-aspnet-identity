using Microsoft.AspNetCore.Mvc;

public class ValidationProblemDetailsException : Exception
{
    public ValidationProblemDetails ProblemDetails { get; }

    public ValidationProblemDetailsException(ValidationProblemDetails problemDetails)
        : base(BuildMessage(problemDetails))
    {
        ProblemDetails = problemDetails;
    }

    public ValidationProblemDetailsException(ValidationProblemDetails problemDetails, string? message)
        : base($"{BuildMessage(problemDetails)}{message}")
    {
    }

    public ValidationProblemDetailsException(ValidationProblemDetails problemDetails, string? message, Exception? innerException = null) 
        : base($"{BuildMessage(problemDetails)}{message}", innerException)
    {
    }

    public static string BuildMessage(ValidationProblemDetails problemDetails) =>
        $"Validation problems found: {string.Join(',', problemDetails.Errors.Keys)}";
}
