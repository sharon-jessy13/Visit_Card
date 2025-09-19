using Microsoft.AspNetCore.Mvc;
using visit_card.Models;
using visit_card.Services;

namespace visit_card.Controllers;

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
    public async Task<IActionResult> CheckEligibility(int employeeId)
    {
        var result = await _visitingCardService.CheckEligibilityAndGetDetailsAsync(employeeId);
        if (!result.IsEligible)
        {
            return StatusCode(StatusCodes.Status403Forbidden, result);
        }
        return Ok(result);
    }

    [HttpGet("get-request/{vcrId}")]
    public async Task<IActionResult> GetRequestDetails(int vcrId)
    {
        var details = await _visitingCardService.GetRequestDetailsAsync(vcrId);
        if (details == null)
        {
            return NotFound(new { Message = $"No details found for VCRID: {vcrId}." });
        }
        return Ok(details);
    }

    [HttpPost("submit-request")]
    public async Task<IActionResult> SubmitRequest([FromBody] VisitingCardInsertDto request)
    {
        if (string.IsNullOrEmpty(request.EmployeeName) || string.IsNullOrEmpty(request.Designation))
        {
            return BadRequest(new { Message = "Employee name and designation are mandatory." });
        }

        var newVcrId = await _visitingCardService.SaveVisitingCardRequestAsync(request);
        return Ok(new { vcrId = newVcrId });
    }

    [HttpPut("update-request/{vcrId}")]
    public async Task<IActionResult> UpdateRequest(int vcrId, [FromBody] VisitingCardRequest request)
    {
        if (vcrId <= 0)
        {
            return BadRequest(new { Message = "A valid VCRID is required." });
        }
        await _visitingCardService.UpdateVisitingCardRequestAsync(vcrId, request);
        return Ok(new { Message = $"Visiting card request {vcrId} updated successfully." });
    }
}