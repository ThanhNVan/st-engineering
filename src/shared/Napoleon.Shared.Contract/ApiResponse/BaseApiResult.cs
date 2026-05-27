using System.Net;

namespace Napoleon.Shared.Contract.ApiResponse;

public record BaseApiResult(HttpStatusCode HttpStatusCode = HttpStatusCode.OK, string Message = "")
{
    public bool IsSuccess => ((int)HttpStatusCode).ToString().StartsWith('2');

    public string Message { get; init; } = !string.IsNullOrWhiteSpace(Message) ? Message : Enum.GetName(typeof(HttpStatusCode), (int)HttpStatusCode)!;
}

