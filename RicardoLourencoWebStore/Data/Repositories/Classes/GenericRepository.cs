using Microsoft.EntityFrameworkCore;
using RicardoLourencoWebStore.Data.Entities;
using RicardoLourencoWebStore.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Data.Repositories.Classes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public DataContext Context { get; }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await SaveAllAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await SaveAllAsync();
        }

        public async Task<bool> ExistsAsync(int id) => await _context.Set<T>().AnyAsync(e => e.Id == id);

        public IQueryable<T> GetAll() => _context.Set<T>().AsNoTracking();

        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await SaveAllAsync();
        }

        private async Task<bool> SaveAllAsync() => await _context.SaveChangesAsync() > 0;
    }
}
