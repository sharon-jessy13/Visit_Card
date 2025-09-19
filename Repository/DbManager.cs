using System.Data;
using Microsoft.Data.SqlClient;
using visit_card.Models;

namespace visit_card.Repository;

public class DbManager
{
    private readonly string _connectionString;

    public DbManager(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("WFAppConnection")!;
    }

    public async Task<bool> CheckIsEligibleAsync(int employeeId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("VisitingCardRequest_CheckIsEligible", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add(new SqlParameter("@EID", employeeId));
        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return result != null && Convert.ToBoolean(result);
    }

    public async Task<EmployeeDetailsDto?> GetEmployeeDetailsAsync(int employeeId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("VisitingCardRequest_GetEmployeeDetails", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add(new SqlParameter("@EID", employeeId));
        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new EmployeeDetailsDto
            {
                EmployeeId = reader.GetInt64(reader.GetOrdinal("EID")),
                EmployeeName = reader.GetString(reader.GetOrdinal("FullName")),
                Designation = reader.GetString(reader.GetOrdinal("Designation")),
                MworkLocationID = reader.GetInt32(reader.GetOrdinal("MWorkLocationID")),
                MySingleID = reader.GetString(reader.GetOrdinal("MySingleID")),
                GroupName = reader.GetString(reader.GetOrdinal("GroupName")),
                Labname = reader.IsDBNull(reader.GetOrdinal("Labname")) ? null : reader.GetString(reader.GetOrdinal("Labname")),
                ResourceType = reader.GetString(reader.GetOrdinal("ResourceType"))
            };
        }
        return null;
    }

    public async Task<IEnumerable<LocationDto>> GetLocationsAsync(bool isActive)
    {
        var locations = new List<LocationDto>();
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("VisitingCardRequest_GetLocations", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add(new SqlParameter("@IsActive", isActive));
        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            locations.Add(new LocationDto
            {
                LocationId = reader.GetInt32(reader.GetOrdinal("MWorklocationID")),
                Name = reader.GetString(reader.GetOrdinal("Location"))
            });
        }
        return locations;
    }

    public async Task<VisitingCardRequest?> GetRequestDetailsByIdAsync(int vcrId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("VisitingCardRequest_GetDeatilsByMasterID", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add(new SqlParameter("@VCRID", vcrId));
        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new VisitingCardRequest
            {
                VCRID = reader.GetInt32(reader.GetOrdinal("VCRID")),
                EmployeeName = reader.GetString(reader.GetOrdinal("EmployeeName")),
                Designation = reader.GetString(reader.GetOrdinal("Designation")),
                Group = reader.GetString(reader.GetOrdinal("Group")),
                NumberOfCards = reader.GetInt32(reader.GetOrdinal("NumberOfCards")),
                MobileNumber = reader.IsDBNull(reader.GetOrdinal("MobileNumber")) ? null : reader.GetString(reader.GetOrdinal("MobileNumber")),
                WorkLocation = reader.GetString(reader.GetOrdinal("WorkLocation")),
                IsDesignationDisplayed = reader.GetBoolean(reader.GetOrdinal("IsDesignationDisplayed")),
                IsGroupDisplayed = reader.GetBoolean(reader.GetOrdinal("IsGroupDisplayed")),
                IsKannadaAddressIncluded = reader.GetBoolean(reader.GetOrdinal("IsKannadaAddressIncluded"))
            };
        }
        return null;
    }

    public async Task ExecuteNonQueryAsync(string storedProcedureName, params SqlParameter[] parameters)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(storedProcedureName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddRange(parameters);
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<object> ExecuteNonQueryWithOutputAsync(string storedProcedureName, string outputParamName, params SqlParameter[] parameters)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(storedProcedureName, connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddRange(parameters);
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        return command.Parameters[outputParamName].Value;
    }
}