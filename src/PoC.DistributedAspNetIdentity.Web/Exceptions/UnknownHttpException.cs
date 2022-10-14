public class UnknownHttpException : Exception
{
    public UnknownHttpException(HttpResponseMessage response) : base(BuildMessage(response))
    {
    }

    public UnknownHttpException(HttpResponseMessage response, string? message) : base($"{BuildMessage(response)}{message}")
    {
    }

    public UnknownHttpException(HttpResponseMessage response, string? message, Exception? innerException) : base($"{BuildMessage(response)}{message}", innerException)
    {
    }

    public static string BuildMessage(HttpResponseMessage response) =>
        $"Request failed with status {response.StatusCode:d}. {response.RequestMessage.Method} {response.RequestMessage.RequestUri}. ";
}