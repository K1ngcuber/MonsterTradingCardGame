using Npgsql;

namespace Infrastructure;

public class Repository
{
    private readonly DatabaseConnection _connection;
    
    public Repository()
    {
        _connection = new DatabaseConnection();
    }
    
    public async Task<List<T>> ListAsync<T>()
    {
        var command = _connection.CreateCommand("select * from card");

        var result = await _connection.SelectAsync<T>(command);
        return result;
    }
}