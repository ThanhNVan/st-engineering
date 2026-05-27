namespace Napoleon.Shared.Common.CustomException;

public class BadRequestException(string message, Exception? exception = default) 
    : Exception(message, exception);

