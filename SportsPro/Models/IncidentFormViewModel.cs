namespace SportsPro.Models
{

    // ViewModel for Add/Edit views to allow for dropdown lists and cleaner code.
    public class IncidentFormViewModel
    {
        public string OperationMode { get; set; } = "Add";
        public Incident CurrentIncident { get; set; } = new Incident();

        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Technician> Technicians { get; set; } = new List<Technician>();


    }
}
