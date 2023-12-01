using Atelier.DAL.Context;
using Atelier.DAL.Entities;
using Atelier.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Atelier.DAL.Repositories
{
    public class ClientRepository : IClientRepository<Client>
    {
        private readonly AtelierContext db;

        public ClientRepository(AtelierContext context)
        {
            db = context;
        }

        public IQueryable<Client> GetAll()
        {
            return db.Clients;
        }

        public async Task<Client> Get(int id)
        {
            return await db.Clients.FindAsync(id);
        }

        public async Task Create(Client item)
        {
            await db.Clients.AddAsync(item);
        }

        public void Update(Client item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public List<Client> Find(Func<Client, Boolean> predicate)
        {
            return db.Clients.Where(predicate).ToList();
        }

        public async Task Delete(int id)
        {
            Client item = await db.Clients.FindAsync(id);
            if (item != null)
                db.Clients.Remove(item);
        }
    }
}
