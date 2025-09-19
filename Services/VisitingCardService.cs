using Microsoft.Data.SqlClient;
using System.Data;
using visit_card.Models;
using visit_card.Repository;

namespace visit_card.Services;

public class VisitingCardService
{
    private readonly DbManager _dbManager;

    public VisitingCardService(DbManager dbManager)
    {
        _dbManager = dbManager;
    }

    public async Task<EligibilityCheckResponseDto> CheckEligibilityAndGetDetailsAsync(int employeeId)
    {
        bool isEligible = await _dbManager.CheckIsEligibleAsync(employeeId);
        if (!isEligible)
        {
            return new EligibilityCheckResponseDto { IsEligible = false, Message = "Employee is not eligible." };
        }
        var employeeDetails = await _dbManager.GetEmployeeDetailsAsync(employeeId);
        var locations = await _dbManager.GetLocationsAsync(true);
        return new EligibilityCheckResponseDto
        {
            IsEligible = true,
            Message = "Employee is eligible to apply for a visiting card.",
            EmployeeDetails = employeeDetails,
            Locations = locations
        };
    }

    public async Task<VisitingCardRequest?> GetRequestDetailsAsync(int vcrId)
    {
        return await _dbManager.GetRequestDetailsByIdAsync(vcrId);
    }

    public async Task<int> SaveVisitingCardRequestAsync(VisitingCardInsertDto request)
    {
        var parameters = new List<SqlParameter>
        {
            new("@MEmpID", request.MempID),
            new("@EmployeeName", request.EmployeeName),
            new("@Designation", request.Designation),
            new("@EmpGroup", (object)request.Group ?? DBNull.Value),
            new("@NofCards", request.NumberOfCards),
            new("@MySingleID", request.MySingleID),
            new("@MworkLocationID", request.MworkLocationID),
            new("@MobileNo", (object)request.MobileNumber ?? DBNull.Value),
            new("@IsDesignationDisplayed", request.IsDesignationDisplayed ? 1 : 0),
            new("@IsGroupDisplayed", request.IsGroupDisplayed ? 1 : 0),
            new("@IsKannadaAddressIncluded", request.IsKannadaAddressIncluded ? 1 : 0),
            new("@VCRID", SqlDbType.BigInt) { Direction = ParameterDirection.Output }
        };

        var newId = await _dbManager.ExecuteNonQueryWithOutputAsync(
            "VisitingCardRequest_InsertCardDetails",
            "@VCRID",
            parameters.ToArray()
        );

        return Convert.ToInt32(newId);
    }

    public async Task UpdateVisitingCardRequestAsync(int vcrId, VisitingCardRequest request)
    {
        var parameters = new List<SqlParameter>
        {
            new("@VCRID", vcrId),
            new("@EmployeeName", request.EmployeeName),
            new("@Designation", request.Designation),
            new("@EmpGroup", request.Group),
            new("@NoOfCards", request.NumberOfCards),
            new("@MySingleID", request.MySingleID),
            new("@MobileNo", string.IsNullOrEmpty(request.MobileNumber) ? DBNull.Value : request.MobileNumber),
            new("@MworkLocationID", request.MworkLocationID),
            new("@IsKannadaAddressIncluded", request.IsKannadaAddressIncluded)
        };
        await _dbManager.ExecuteNonQueryAsync("VCR_UpdateCardDetails", parameters.ToArray());
    }
}