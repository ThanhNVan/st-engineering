namespace Napoleon.Shared.Common.CustomException;

public class UnauthorizedException(string message, Exception? exception = default)
    : Exception(message, exception);