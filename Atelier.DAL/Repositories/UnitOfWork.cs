using Atelier.DAL.Context;
using Atelier.DAL.Entities;
using Atelier.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Atelier.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AtelierContext db;
        private ClientRepository Client_repository;
        private EmployeeRepository Employee_repository;
        private MaterialRepository Material_repository;
        private OrderRepository Order_repository;
        private WorksTypeRepository WorksType_repository;
        private UserRepository User_repository;

        public UnitOfWork(DbContextOptions<AtelierContext> options)
        {
            db = new AtelierContext(options);
        }

        public IClientRepository<Client> Clients
        {
            get
            {
                if (Client_repository == null)
                    Client_repository = new ClientRepository(db);
                return Client_repository;
            }
        }

        public IEmployeeRepository<Employee> Employees
        {
            get
            {
                if (Employee_repository == null)
                    Employee_repository = new EmployeeRepository(db);
                return Employee_repository;
            }
        }

        public IMaterialRepository<Material> Materials
        {
            get
            {
                if (Material_repository == null)
                    Material_repository = new MaterialRepository(db);
                return Material_repository;
            }
        }

        public IOrderRepository<Order> Orders
        {
            get
            {
                if (Order_repository == null)
                    Order_repository = new OrderRepository(db);
                return Order_repository;
            }
        }

        public IWorksTypeRepository<WorksType> WorksTypes
        {
            get
            {
                if (WorksType_repository == null)
                    WorksType_repository = new WorksTypeRepository(db);
                return WorksType_repository;
            }
        }

        public IUserRepository<User> Users
        {
            get
            {
                if (User_repository == null)
                    User_repository = new UserRepository(db);
                return User_repository;
            }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await db.Database.BeginTransactionAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
