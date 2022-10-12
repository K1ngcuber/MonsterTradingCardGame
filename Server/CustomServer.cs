using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using CustomServer.Attributes;
using CustomServer.Attributes.RouteAttributes;
using CustomServer.Exceptions;
using CustomServer.Helpers;
using CustomServer.Response;

namespace CustomServer;

public static class CustomServer
{
    private const int Port = 9999;
    private static volatile bool _isRunning = true;
    private static Task? _serverTask;
    private static readonly HttpListener Listener = new() {Prefixes = {$"http://localhost:{Port}/"}};
    private static HttpListenerResponse? _response;

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

            _response = context.Response;
            await using var body = context.Request.InputStream;
            using var reader = new StreamReader(body, context.Request.ContentEncoding);

            var json = await reader.ReadToEndAsync();

            //log request
            Console.WriteLine($"Request: {method} {route} body: {json}");

            //TODO: implement auth middleware

            await HandleRequest(route[1..], method, json);

            _response.Close();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
        }
    }

    private static async Task HandleRequest(string route, string httpMethod, string body)
    {
        try
        {
            //find all controllers
            var controllers = Assembly.GetEntryAssembly()
                ?.GetTypes()
                .Where(x => x.IsDefined(typeof(ApiControllerAttribute)));

            if (controllers is null)
            {
                throw new NotFoundException("Controller not found");
            }

            //find controller with route
            var controller = controllers.FirstOrDefault(x =>
                x.IsDefined(typeof(RouteAttribute)) &&
                route.Contains(x.GetCustomAttribute<RouteAttribute>()?.Route ?? ""));


            //if not found -> 404
            if (controller is null)
            {
                throw new NotFoundException("Route not found");
            }

            var methodUrl = route.Replace((controller.GetCustomAttribute<RouteAttribute>()?.Route ?? "") + "/", "");

            //create instance of controller
            var controllerObj = Activator.CreateInstance(controller);

            //find all method
            var method = controller.GetMethods().FirstOrDefault(x =>
                x.IsDefined(typeof(HttpAttribute)) &&
                x.GetCustomAttribute<HttpAttribute>()?.Method == httpMethod &&
                methodUrl.Contains(x.GetCustomAttribute<HttpAttribute>()?.Route ?? ""));

            //if not found -> 404
            if (method is null)
            {
                throw new NotFoundException("Method not found");
            }

            var parameterUrl = methodUrl.Replace(method.GetCustomAttribute<HttpAttribute>()?.Route + "/", "");

            var parameters = method.GetParameters();
            var args = MethodUrlHelper.GetArgs(body, parameterUrl, parameters);

            ActionResult? result;
            //check if method is async
            var isAwaitable = method.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;
            if (isAwaitable)
            {
                //!!!!!!!!!!! possible source for error xD
                result = (ActionResult) await (dynamic) method.Invoke(controllerObj, args)!;
            }
            else
            {
                result = (ActionResult) method.Invoke(controllerObj, args)!;
            }

            HandleResponse(result);
        }
        catch (HttpException e)
        {
            HandleResponse(new ErrorCode(e.StatusCode, e.ReasonPhrase));
        }
        catch (Exception e)
        {
            //something very bad happened
            HandleResponse(new ErrorCode(500, "Internal server error"));
            Console.WriteLine("A very bad Internal Server Error");
            Console.Error.WriteLine(e);
        }
    }

    private static void HandleResponse(ActionResult result)
    {
        if (_response is null) return;

        _response.StatusCode = result.StatusCode;
        _response.ContentType = "application/json";
        var buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result.Value ?? result.StatusCode));
        _response.ContentLength64 = buffer.Length;
        _response.OutputStream.Write(buffer, 0, buffer.Length);
    }
}