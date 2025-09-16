using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using visit_card.Models;
using System.Collections.Generic;

namespace visit_card.Repository
{

    public class DbManager
    {
        private readonly string _connectionString;

        public DbManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WFAppConnection")!;
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

                public DataTable GetEmployeeDetails(int employeeId)
        {
            DataTable dt = new DataTable();
            string storedProcedureName = "VisitingCardRequest_GetEmployeeDetails";
            var parameters = new[] { new SqlParameter("@EID", employeeId) };

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

         public DataTable GetVisitingCardDetailsByMasterId(int masterId)
        {
            DataTable dt = new DataTable();
            string storedProcedureName = "VisitingCardRequest_GetDeatilsByMasterID";
            var parameters = new[] { new SqlParameter("@VCRID", masterId) };

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        
        public bool CheckIsEligible(int employeeId)
        {
            string storedProcedureName = "VisitingCardRequest_CheckIsEligible";
            var parameters = new[] { new SqlParameter("@EID", employeeId) };

            
            object result = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    connection.Open();
                    result = command.ExecuteScalar();
                }
            }

            
            return result != null && Convert.ToBoolean(result);
        }

                public DataTable GetLocations(bool isActive)
        {
            DataTable dt = new DataTable();
            string storedProcedureName = "VisitingCardRequest.getLocations";
            var parameters = new[] { new SqlParameter("@IsActive", isActive) };

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}