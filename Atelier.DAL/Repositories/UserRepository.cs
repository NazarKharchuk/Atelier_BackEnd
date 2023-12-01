using Atelier.DAL.Context;
using Atelier.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Atelier.DAL.Repositories
{
    public class UserRepository : IUserRepository<User>
    {
        private readonly AtelierContext db;

        public UserRepository(AtelierContext context)
        {
            db = context;
        }

        public IQueryable<User> GetAll()
        {
            return db.Users;
        }

        public async Task<User> Get(int id)
        {
            return await db.Users.Include(x => x.Employee).Where(x => x.UserId == id).FirstOrDefaultAsync();
        }

        public async Task Create(User item)
        {
            await db.Users.AddAsync(item);
        }

        public void Update(User item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public List<User> Find(Func<User, Boolean> predicate)
        {
            return db.Users.Where(predicate).ToList();
        }

        public async Task Delete(int id)
        {
            User item = await db.Users.FindAsync(id);
            if (item != null)
                db.Users.Remove(item);
        }
    }
}
