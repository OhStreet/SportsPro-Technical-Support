using SportsPro.Models;

namespace SportsPro.DataLayer
{
    public interface IUnitOfWork
    {
        Repository<Product> Products { get; }
        Repository<Technician> Technicians { get; }
        Repository<Country> Countries { get; }
        Repository<Customer> Customers { get; }
        Repository<Incident> Incidents { get; }
        Repository<Registration> Registrations { get; }
        void Save();
    }
}
