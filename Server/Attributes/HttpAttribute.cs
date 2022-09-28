namespace CustomServer.Attributes;

public class HttpAttribute : Attribute
{
    public string Method { get; }
    public string Route { get; }

    protected HttpAttribute(string method, string route = "/")
    {
        Method = method;
        Route = route;
    }
}