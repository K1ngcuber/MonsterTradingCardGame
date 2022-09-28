namespace CustomServer.Attributes;

public class HttpGetAttribute : HttpAttribute
{
    public HttpGetAttribute(string route = "/") : base("GET", route)
    {
    }
}