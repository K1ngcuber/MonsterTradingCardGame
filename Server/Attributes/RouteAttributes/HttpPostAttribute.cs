namespace CustomServer.Attributes.RouteAttributes;

public class HttpPostAttribute : HttpAttribute
{
    public HttpPostAttribute(string route = "/") : base("POST", route)
    {
    }
}