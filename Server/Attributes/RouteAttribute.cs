namespace CustomServer.Attributes;

public class RouteAttribute : Attribute
{
    public string Route { get; }

    public RouteAttribute(string route)
    {
        Route = route;
    }
}