using Microsoft.EntityFrameworkCore;
using POC_TaskBoard.API.Context;
using POC_TaskBoard.API.Entities;
using POC_TaskBoard.API.Repos.Interface;
using System.Linq.Expressions;

namespace POC_TaskBoard.API.Repos
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.Where(e => EF.Property<int>(e, "Id") == id).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetListWhereAsync(Expression<Func<T, bool>>? predicate)
        {

            var entities = await _context.Set<T>()
                .AsNoTracking()
                .Where(predicate).ToListAsync();

            return entities;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity, int id)
        {
            var existingEntity = await _context.Set<T>().FindAsync(id);
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> BoardExistAsync(Expression<Func<T, bool>> predicate)
        {
            var exists = await _context.Set<T>().AnyAsync(predicate);
            return exists;
        }

        
    }
}
