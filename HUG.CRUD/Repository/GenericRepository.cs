
using HUG.CRUD.Base;
using HUG.CRUD.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HUG.CRUD.Repository
{ 
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly DbContext _context;
        private DbSet<T> _entities;

        protected GenericRepository(DbContext context)
        {
            _context = context;
        }

        //Original Func
        public ICollection<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public bool Create(T entity)
        {
            _context.Add(entity);
            return Save();
        }
        public bool Update(T entity)
        {
            _context.Update(entity);
            return Save();
        }
        public bool Delete(T entity)
        {
            _context.Remove(entity);
            return Save();
        }
        public bool SoftDelete(T entity)
        {
            throw new NotImplementedException();
        }
        public bool IsExists(int id)
        {
            return _context.Set<T>().Any(e => e.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }


        //ASYNC FUNCTION
        public Task<ICollection<T>> GetAll_Async()
        {
            throw new NotImplementedException();
        }
        public async Task<T> GetById_Async(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public Task<bool> Create_Async(T entity)
        {
            _context.AddAsync(entity);
            return Save_Async();
        }
        public Task<bool> Update_Async(T entity)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Delete_Async(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExists_Async(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save_Async()
        {
            int save = await _context.SaveChangesAsync();
            return save > 0;
        }


    }
}
