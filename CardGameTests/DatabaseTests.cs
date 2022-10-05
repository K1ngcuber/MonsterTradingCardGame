using System.Threading.Tasks;
using Infrastructure;
using Models;
using NUnit.Framework;

namespace CardGameTests;

public class Tests
{
    private Repository repo;
    
    [SetUp]
    public void Setup()
    {
        repo = new Repository();
    }
    
    [Test]
    public async Task Test1()
    {
        //arrange

        //act
        var result = await repo.ListAsync<Card>();

        //assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.GreaterThan(0));
    }
}