namespace SportsPro.Models
{
    public class SportsProSession
    {
        private const string TechKey = "technicianid";
        private const string CustomerKey = "customerid";

        private ISession session { get; set; }

        public SportsProSession(ISession session)
        {
            this.session = session;
        }

        public void SetTechnicianId(int techId)
        {
            session.SetInt32(TechKey, techId);
        }

        public int? GetTechnicianId()
        {
            return session.GetInt32(TechKey);
        }

        public void RemoveTechnician()
        {
            session.Remove(TechKey);
        }


        // Customer session management methods
        public void SetCustomerId(int customerId)
        {
            session.SetInt32(CustomerKey, customerId);
        }

        public int? GetCustomerId()
        {
            return session.GetInt32(CustomerKey);
        }

        public void RemoveCustomer()
        {
            session.Remove(CustomerKey);
        }
    }
}
