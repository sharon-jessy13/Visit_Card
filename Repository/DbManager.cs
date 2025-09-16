using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
public class DbManager
{
    private readonly string _connectionString;

    public DbManager(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("WFAppConnection");
    }

    public void ExecuteNonQuery(string storedProcedureName, SqlParameter[] parameters)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    public DataTable GetDataTable(string storedProcedureName, SqlParameter[] parameters)
    {
        DataTable dt = new DataTable();
        using (var connection = new SqlConnection(_connectionString))
        {
            using (var command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }
        }
        return dt;
    }
}