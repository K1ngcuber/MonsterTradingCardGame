using System.Data;
using System.Reflection;
using Npgsql;

namespace Infrastructure;

public class DatabaseConnection
{
    private NpgsqlConnection Connection { get; }

    public DatabaseConnection()
    {
        Connection = new NpgsqlConnection(Constants.ConnectionString);
    }

    //create database tables
    public void Init()
    {
        var file = new FileInfo("install.sql");
        
        var script = file.OpenText().ReadToEnd();
        var command = new NpgsqlCommand(script, Connection);
        Connection.Open();
        command.ExecuteNonQuery();
        Connection.Close();
        Console.WriteLine("Database initialized");
    }

    //create a new command object
    public NpgsqlCommand CreateCommand(string sql)
    {
        return new NpgsqlCommand(sql, Connection);
    }

    //read data from the database and return a list
    public async Task<List<T>> SelectAsync<T>(NpgsqlCommand command)
    {
        var result = new List<T>();
        await Connection.OpenAsync();

        var reader = command.ExecuteReader();
        while (await reader.ReadAsync())
        {
            //get all columns of read table
            var columns = await reader.GetColumnSchemaAsync();

            //create a new instance of the generic type
            var obj = Activator.CreateInstance<T>();
            foreach (var column in columns)
            {
                //get value from reader by column name
                var value = reader[column.ColumnName];
                
                //get property info by column name case insensitive
                var property = obj?.GetType().GetProperty(column.ColumnName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                //set property value
                property?.SetValue(obj, value);
            }

            result.Add(obj);
        }

        await Connection.CloseAsync();
        return result;
    }
    
    //insert, update or delete data
    public async Task ExecuteAsync(NpgsqlCommand command)
    {
        await Connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        await Connection.CloseAsync();
    }
    
    //count rows in a table
    public async Task<int> CountAsync(NpgsqlCommand command)
    {
        await Connection.OpenAsync();
        var result = (int) (await command.ExecuteScalarAsync() ?? 0);
        await Connection.CloseAsync();
        return result;
    }
}