using System.Net;
using CardGame.Services;
using FluentAssertions;
using Xunit;

namespace CardGameTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var service = new AuthService();

        service.Login("","");

        "".Should().Be("");
    }
}