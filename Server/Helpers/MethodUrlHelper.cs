using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using CustomServer.Attributes.ParameterAttributes;

namespace CustomServer.Helpers;

public static class MethodUrlHelper
{
    //parse the url and get the parameters
    public static object?[] GetArgs(string body, string methodUrl, ParameterInfo[] parameters)
    {
        var args = new object?[parameters.Length];
        var parts = methodUrl.Split('/');
        
        foreach (var parameterInfo in parameters)
        {
            if (parameterInfo.GetCustomAttribute<FromRouteAttribute>() is not null)
            {
                args[parameterInfo.Position] = parts[parameterInfo.Position].ParseArgFromRoute(parameterInfo);
            }
            else if(parameterInfo.GetCustomAttribute<FromBodyAttribute>() is not null)
            {
                args[parameterInfo.Position] = body.ParseArgFromBody(parameterInfo);
            }
        }

        return args;
    }

    //parse json body to object
    private static object? ParseArgFromBody(this string body, ParameterInfo parameterInfo)
    {
        var type = parameterInfo.ParameterType;
        
        return JsonSerializer.Deserialize(body, type);
    }
    
    //parse arguments from route to parameters
    private static object? ParseArgFromRoute(this string part, ParameterInfo parameterInfo)
    {
        var type = parameterInfo.ParameterType;
        var converter = TypeDescriptor.GetConverter(type);
        return converter.ConvertFrom(part);
    }
}