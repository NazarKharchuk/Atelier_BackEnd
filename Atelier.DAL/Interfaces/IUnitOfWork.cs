using Atelier.DAL.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Atelier.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClientRepository<Client> Clients { get; }
        IEmployeeRepository<Employee> Employees { get; }
        IMaterialRepository<Material> Materials { get; }
        IOrderRepository<Order> Orders { get; }
        IWorksTypeRepository<WorksType> WorksTypes { get; }
        IUserRepository<User> Users { get; }

        Task SaveAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
