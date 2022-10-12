namespace CustomServer.Exceptions;

public class HttpException : Exception
{
    public int StatusCode { get; }
    public string ReasonPhrase { get; }

    protected HttpException(int statusCode, string reasonPhrase)
    {
        StatusCode = statusCode;
        ReasonPhrase = reasonPhrase;
    }
}