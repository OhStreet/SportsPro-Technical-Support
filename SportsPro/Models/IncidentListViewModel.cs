namespace SportsPro.Models
{   
    // ViewModel for the main list view to allow for future filtering by status, etc. and cleaner code in the view.
    public class IncidentListViewModel
    {
        public string IncidentStatus { get; set; } = "all";

        public Technician? Technician { get; set; } = new Technician();

        public List<Incident> Incidents { get; set; } = new List<Incident>();

    }
}
