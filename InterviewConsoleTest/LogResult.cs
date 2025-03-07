public class LogResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public LogResult(bool success, string errorMessage)
    {
        Success = success;
        ErrorMessage = errorMessage;
    }
}
