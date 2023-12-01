using Atelier.DAL.Context;
using Atelier.DAL.Entities;
using Atelier.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Atelier.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository<Employee>
    {
        private readonly AtelierContext db;

        public EmployeeRepository(AtelierContext context)
        {
            db = context;
        }

        public IQueryable<Employee> GetAll()
        {
            return db.Employees;
        }

        public async Task<Employee> Get(int id)
        {
            return await db.Employees.FindAsync(id);
        }

        public async Task Create(Employee item)
        {
            await db.Employees.AddAsync(item);
        }

        public void Update(Employee item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public List<Employee> Find(Func<Employee, Boolean> predicate)
        {
            return db.Employees.Where(predicate).ToList();
        }

        public async Task Delete(int id)
        {
            Employee item = await db.Employees.FindAsync(id);
            if (item != null)
                db.Employees.Remove(item);
        }
    }
}
