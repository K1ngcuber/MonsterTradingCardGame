namespace CustomServer.Exceptions;

public class UnauthorizedException : HttpException
{
    public UnauthorizedException(string reasonPhrase) : base(401, reasonPhrase)
    {
    }
}