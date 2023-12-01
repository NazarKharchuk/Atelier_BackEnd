using Atelier.DAL.Context;
using Atelier.DAL.Entities;
using Atelier.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Atelier.DAL.Repositories
{
    public class MaterialRepository : IMaterialRepository<Material>
    {
        private readonly AtelierContext db;

        public MaterialRepository(AtelierContext context)
        {
            db = context;
        }

        public IQueryable<Material> GetAll()
        {
            return db.Materials;
        }

        public async Task<Material> Get(int id)
        {
            return await db.Materials.FindAsync(id);
        }

        public async Task Create(Material item)
        {
            await db.Materials.AddAsync(item);
        }

        public void Update(Material item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public List<Material> Find(Func<Material, Boolean> predicate)
        {
            return db.Materials.Where(predicate).ToList();
        }

        public async Task Delete(int id)
        {
            Material item = await db.Materials.FindAsync(id);
            if (item != null)
                db.Materials.Remove(item);
        }
    }
}
