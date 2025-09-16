using System.Data;
using Microsoft.Data.SqlClient;
using visit_card.Models;
using visit_card.Repository;

namespace visit_card.Services
{
    public class VisitingCardService
    {
        private readonly DbManager _dbManager;

        public VisitingCardService(DbManager dbManager)
        {
            _dbManager = dbManager;
        }

        public bool CheckIsEligible(int employeeId)
        {
           
            return _dbManager.CheckIsEligible(employeeId);
        }

        public DataTable GetEmployeeDetails(int employeeId)
        {
            
            return _dbManager.GetEmployeeDetails(employeeId);
        }

        public DataTable GetLocations(bool isActive)
        {
            
            return _dbManager.GetLocations(isActive);
        }

        public void SaveVisitingCardRequest(VisitingCardRequest request)
        {
            
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeName", request.EmployeeName),
                new SqlParameter("@Designation", request.Designation),
                new SqlParameter("@Group", request.Group),
                new SqlParameter("@NoOfCards", request.NumberOfCards),
                new SqlParameter("@WorkLocation", request.WorkLocation),
                new SqlParameter("@IsDesignationDisplayed", request.IsDesignationDisplayed),
                new SqlParameter("@IsGroupDisplayed", request.IsGroupDisplayed),
                new SqlParameter("@IsKannadaAddressIncluded", request.IsKannadaAddressIncluded)
            };

            if (!string.IsNullOrEmpty(request.MobileNumber))
            {
                parameters.Add(new SqlParameter("@MobileNo", request.MobileNumber));
            }
            else
            {
                parameters.Add(new SqlParameter("@MobileNo", DBNull.Value));
            }

            var vcrIdParam = new SqlParameter("@VCRID", SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Output
            };
            parameters.Add(vcrIdParam);

            _dbManager.ExecuteNonQuery("VisitingCardRequest_InsertCardDetails", parameters.ToArray());
        }

        public void UpdateVisitingCardRequest(VisitingCardRequest request)
        {
            
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@VCRID", request.VCRID),
                new SqlParameter("@EmployeeName", request.EmployeeName),
                new SqlParameter("@Designation", request.Designation),
                new SqlParameter("@NoOfCards", request.NumberOfCards),
                new SqlParameter("@WorkLocation", request.WorkLocation),
                new SqlParameter("@IsKannadaAddressIncluded", request.IsKannadaAddressIncluded)
            };
            if (!string.IsNullOrEmpty(request.MobileNumber))
            {
                parameters.Add(new SqlParameter("@MobileNo", request.MobileNumber));
            }
            else
            {
                parameters.Add(new SqlParameter("@MobileNo", DBNull.Value));
            }

            _dbManager.ExecuteNonQuery("VCR_UpdateCardDetails", parameters.ToArray());
        }
        
        public DataTable GetVisitingCardDetails(int masterId)
        {
            
            return _dbManager.GetVisitingCardDetailsByMasterId(masterId);
        }
    }
}