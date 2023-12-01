using Atelier.DAL.Context;
using Atelier.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Atelier.DAL.Repositories
{
    public class WorksTypeRepository : IWorksTypeRepository<WorksType>
    {
        private readonly AtelierContext db;

        public WorksTypeRepository(AtelierContext context)
        {
            db = context;
        }

        public IQueryable<WorksType> GetAll()
        {
            return db.WorksTypes;
        }

        public async Task<WorksType> Get(int id)
        {
            return await db.WorksTypes.FindAsync(id);
        }

        public async Task Create(WorksType item)
        {
            await db.WorksTypes.AddAsync(item);
        }

        public void Update(WorksType item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public List<WorksType> Find(Func<WorksType, Boolean> predicate)
        {
            return db.WorksTypes.Where(predicate).ToList();
        }

        public async Task Delete(int id)
        {
            WorksType item = await db.WorksTypes.FindAsync(id);
            if (item != null)
                db.WorksTypes.Remove(item);
        }
    }
}
