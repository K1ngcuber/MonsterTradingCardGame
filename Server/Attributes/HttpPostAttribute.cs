namespace CustomServer.Attributes;

public class HttpPostAttribute : HttpAttribute
{
    public HttpPostAttribute(string route = "/") : base("POST", route)
    {
    }
}