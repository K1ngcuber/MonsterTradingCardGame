using Npgsql;

namespace Infrastructure;

public class Repository<T> where T : class
{
    private readonly DatabaseConnection _connection;

    public Repository()
    {
        _connection = new DatabaseConnection();
    }

    public async Task<List<T>> ListAsync()
    {
        var command = _connection.CreateCommand($"select * from \"{typeof(T).Name}\"");

        var result = await _connection.SelectAsync<T>(command);
        return result;
    }

    public async Task AddAsync(T entity)
    {
        var command = _connection.CreateCommand(
            $"insert into \"{typeof(T).Name}\" ({string.Join(", ", typeof(T).GetProperties().Select(p => p.Name))}) values ({string.Join(", ", typeof(T).GetProperties().Select(p => "@" + p.Name))})");

        foreach (var property in typeof(T).GetProperties())
        {
            command.Parameters.AddWithValue(property.Name, property.GetValue(entity) ?? "");
        }

        await _connection.ExecuteAsync(command);
    }

    public async Task DeleteAsync(Guid id)
    {
        var command = _connection.CreateCommand($"delete from \"{typeof(T).Name}\" where id = @id");
        command.Parameters.AddWithValue("id", id);

        await _connection.ExecuteAsync(command);
    }
    
    public async Task<T?> GetByIdAsync(Guid id)
    {
        var command = _connection.CreateCommand($"select * from \"{typeof(T).Name}\" where id = @id");
        command.Parameters.AddWithValue("id", id);

        //TODO: change so that only one object is returned
        var result = await _connection.SelectAsync<T>(command);
        return result.FirstOrDefault();
    }

    public async Task<T?> GetAsync(string filter, object[] parameters)
    {
        var command = _connection.CreateCommand($"select * from \"{typeof(T).Name}\" " + filter);
        for (var i = 0; i < parameters.Length; i++)
        {
            command.Parameters.AddWithValue($"{i}", parameters[i]);
        }

        var result = await _connection.SelectAsync<T>(command);
        return result.FirstOrDefault();
    }

    public async Task<List<T>> FindAsync(string filter, object[] parameters)
    {
        var command = _connection.CreateCommand($"select * from \"{typeof(T).Name}\" " + filter);
        for (var i = 0; i < parameters.Length; i++)
        {
            command.Parameters.AddWithValue($"{i}", parameters[i]);
        }

        var result = await _connection.SelectAsync<T>(command);
        return result.ToList();
    }
}