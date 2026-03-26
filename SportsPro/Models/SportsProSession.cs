namespace SportsPro.Models
{
    public class SportsProSession
    {
        private const string TechKey = "technicianid";

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
    }
}