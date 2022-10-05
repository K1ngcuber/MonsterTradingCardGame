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
}