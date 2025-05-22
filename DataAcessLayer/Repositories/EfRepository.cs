using DataAcessLayer.Interfaces;
using DataAcessLayer.ModelsCalculator;
using DataAcessLayer.ModelsRPS;
using DataAcessLayer.ModelsShapes;
using DataAcessLayer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAcessLayer.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly AllDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EfRepository(AllDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // AsNoTracking om du bara läser och inte kommer ändra entiteterna
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            // Hittar via primärnyckel
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            // Lägger till men sparar inte förrän SaveChangesAsync kallas
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            // Markerar entity som ändrad
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            // Markerar entity som borttagen
            _dbSet.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            // Sparar alla påbörjade ändringar i context
            await _context.SaveChangesAsync();
        }
    }
}
