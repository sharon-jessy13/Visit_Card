namespace visit_card.Models;

// Main request model, matching your original class
public class VisitingCardRequest
{
    public int VCRID { get; set; }
    public int MempID { get; set; } 
    public int EID { get; set; } 
    public string EmployeeName { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty; // Maps to @EmpGroup
    public int NumberOfCards { get; set; } // Maps to @NoOfCards
    public string? MySingleID { get; set; }
    public bool IsDesignationDisplayed { get; set; }
    public bool IsGroupDisplayed { get; set; }
    public bool IsKannadaAddressIncluded { get; set; }
    public string? MobileNumber { get; set; } 
    public int MworkLocationID { get; set; }
    public string WorkLocation { get; set; } = string.Empty; 
    public string? VCStatus { get; set; }
}


// DTO for returning employee details
public class EmployeeDetailsDto
{
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; } 
    public string? Designation { get; set; }
    public int MworkLocationID { get; set; }
    public string? MySingleID { get; set; }
    public string? GroupName { get; set; }
    public string? Labname { get; set; }
    public string? ResourceType { get; set; }
}

// DTO for returning location details
public class LocationDto
{
    public int LocationId { get; set; }
    public string? Name { get; set; }
}

// DTO for the combined response of the eligibility check
public class EligibilityCheckResponseDto
{
    public bool IsEligible { get; set; }
    public string Message { get; set; } = string.Empty;
    public EmployeeDetailsDto? EmployeeDetails { get; set; }
    public IEnumerable<LocationDto>? Locations { get; set; }
}