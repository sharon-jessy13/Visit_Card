using Microsoft.AspNetCore.Mvc;
using visit_card.Models;
using visit_card.Services;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class VisitingCardController : ControllerBase
{
    private readonly VisitingCardService _visitingCardService;

    public VisitingCardController(VisitingCardService visitingCardService)
    {
        _visitingCardService = visitingCardService;
    }

    [HttpGet("check-eligibility/{employeeId}")]
    public IActionResult CheckEligibility(int employeeId)
    {
        
        bool isEligible = _visitingCardService.CheckIsEligible(employeeId);
        if (isEligible)
        {
           
            DataTable employeeDetails = _visitingCardService.GetEmployeeDetails(employeeId);
            DataTable locations = _visitingCardService.GetLocations(true);

            
            return Ok(new { IsEligible = true, Message = "Employee is eligible to apply for a visiting card." });
        }
        return Unauthorized(new { IsEligible = false, Message = "Employee is not eligible." });
    }

    [HttpPost("submit-request")]
    public IActionResult SubmitRequest([FromBody] VisitingCardRequest request)
    {
        if (string.IsNullOrEmpty(request.EmployeeName) || string.IsNullOrEmpty(request.Designation))
        {
            return BadRequest("Employee name and designation are mandatory.");
        }

        try
        {
            _visitingCardService.SaveVisitingCardRequest(request);
            return Ok("Visiting card request submitted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPut("update-request")]
    public IActionResult UpdateRequest([FromBody] VisitingCardRequest request)
    {
        if (request.VCRID <= 0)
        {
            return BadRequest("VCRID is required to update a request.");
        }

        try
        {
            _visitingCardService.UpdateVisitingCardRequest(request);
            return Ok("Visiting card request updated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet("get-request/{vcrId}")]
    public IActionResult GetRequestDetails(int vcrId)
    {
        
        DataTable details = _visitingCardService.GetVisitingCardDetails(vcrId);
        

        if (details.Rows.Count == 0)
        {
            return NotFound($"No details found for VCRID: {vcrId}.");
        }
        
        return Ok(details);
    }
}