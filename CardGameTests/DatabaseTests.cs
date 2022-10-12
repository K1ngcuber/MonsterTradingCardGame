using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Models;
using NUnit.Framework;

namespace CardGameTests;

public class Tests
{
    Repository<User> repo;
    
    [SetUp]
    public void Setup()
    {
        repo = new Repository<User>();
    }

    [Test]
    public async Task InsertAndCheckIfNotEmptyAndDelete()
    {
        //arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "MAXI",
            Password = "123456",
        };
        
        //act
        await repo.AddAsync(user);

        //assert
        var result = await repo.ListAsync();
        Assert.That(result, Is.Not.Null);

        var u = await repo.GetByIdAsync(result.First().Id);
        Assert.That(u?.Id, Is.EqualTo(user.Id)); 

        foreach (var entry in result)
        {
            await repo.DeleteAsync(entry.Id);
        }
        
        result = await repo.ListAsync();
        Assert.That(result.Count, Is.EqualTo(0));
    }
}