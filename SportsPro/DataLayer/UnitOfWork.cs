using SportsPro.Models;

namespace SportsPro.DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private SportsProContext context { get; set; }

        public Repository<Product> Products { get; private set; }
        public Repository<Technician> Technicians { get; private set; }
        public Repository<Country> Countries { get; private set; }
        public Repository<Customer> Customers { get; private set; }
        public Repository<Incident> Incidents { get; private set; }
        public Repository<Registration> Registrations { get; private set; }

        public UnitOfWork(SportsProContext ctx)
        {
            context = ctx;
            Products = new Repository<Product>(ctx);
            Technicians = new Repository<Technician>(ctx);
            Countries = new Repository<Country>(ctx);
            Customers = new Repository<Customer>(ctx);
            Incidents = new Repository<Incident>(ctx);
            Registrations = new Repository<Registration>(ctx);
        }

        public void Save() => context.SaveChanges();
    }
}
