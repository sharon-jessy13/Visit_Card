namespace visit_card.Models
{
    public class VisitingCardRequest
    {
        public int VCRID { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Group { get; set; }
        public int NumberOfCards { get; set; }
        public string SingleId { get; set; }
        public bool IsDesignationDisplayed { get; set; }
        public bool IsGroupDisplayed { get; set; }
        public bool IsKannadaAddressIncluded { get; set; }
        public string MobileNumber { get; set; }
        public string WorkLocation { get; set; }
    }
}