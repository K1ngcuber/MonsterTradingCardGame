using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using CustomServer.Attributes;

namespace CustomServer;

public static class CustomServer
{
    private const int Port = 9999;
    private static volatile bool _isRunning = true;
    private static Task? _serverTask;
    private static readonly HttpListener Listener = new HttpListener {Prefixes = {$"http://localhost:{Port}/"}};

    public static void StartWebServer()
    {
        if (_serverTask is {IsCompleted: false}) return; //Already started
        _serverTask = ServerLoop();
    }

    public static void Stop()
    {
        _isRunning = false;
        lock (Listener)
        {
            //Use a lock so we don't kill a request that's currently being processed
            Listener.Stop();
        }

        try
        {
            _serverTask?.Wait();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
        }
    }

    private static async Task? ServerLoop()
    {
        Listener.Start();
        while (_isRunning)
        {
            try
            {
                //GetContextAsync() returns when a new request come in
                var context = await Listener.GetContextAsync();
                lock (Listener)
                {
                    if (_isRunning) ProcessRequest(context).Wait();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }

    private static async Task ProcessRequest(HttpListenerContext context)
    {
        try
        {
            var route = context.Request.Url?.AbsolutePath ?? "/";
            var method = context.Request.HttpMethod;

            using var response = context.Response;
            await using var body = context.Request.InputStream;
            using var reader = new StreamReader(body, context.Request.ContentEncoding);

            var json = await reader.ReadToEndAsync();
            
            //log request
            Console.WriteLine($"Request: {method} {route} body: {json}");

            await HandleRequest(route[1..], method, json, context.Response);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
        }
    }

    private static async Task HandleRequest(string route, string httpMethod, string body, HttpListenerResponse response)
    {
        //find all controllers
        var controllers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.IsDefined(typeof(ApiControllerAttribute)));

        //find controller with route
        var controller = controllers.FirstOrDefault(x =>
            x.IsDefined(typeof(RouteAttribute)) && x.GetCustomAttribute<RouteAttribute>()?.Route == route);

        //if not found -> 404
        if (controller is null)
        {
            HandleResponse(404, "Route not Found!", response);
            return;
        }

        //create instance of controller
        var controllerObj = Activator.CreateInstance(controller);

        //find all method
        var method = controller.GetMethods().FirstOrDefault(x =>
            x.IsDefined(typeof(HttpAttribute)) && x.GetCustomAttribute<HttpAttribute>()?.Method == httpMethod);

        //if not found -> 404
        if (method is null)
        {
            HandleResponse(404, "Method not found!", response);
            return;
        }

        //todo body and parameters from route
        var args = new object?[]
        {
            body
        };
        try
        {
            object? result;
            var isAwaitable = method.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;
            if (isAwaitable)
            {
                //!!!!!!!!!!! possible source for error xD
                result = (object)await (dynamic)method.Invoke(controllerObj, args)!;
            }
            else
            {
                result = method.Invoke(controllerObj, args);
            }
            HandleResponse(200, result?.ToString() ?? "", response);
        }
        //Todo HttpException
        catch
        {
            HandleResponse(500, "Internal Server Error", response);
        }
    }

    private static void HandleResponse(int statusCode, string message, HttpListenerResponse response)
    {
        response.StatusCode = statusCode;
        response.ContentType = "application/json";
        var buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
    }
}