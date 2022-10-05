namespace CustomServer.Attributes.RouteAttributes;

public class HttpGetAttribute : HttpAttribute
{
    public HttpGetAttribute(string route = "/") : base("GET", route)
    {
    }
}