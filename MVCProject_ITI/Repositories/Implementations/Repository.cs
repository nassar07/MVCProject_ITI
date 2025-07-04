﻿using Microsoft.EntityFrameworkCore;
using MVCProject_ITI.Models;
using MVCProject_ITI.Repositories.Interfaces;

namespace MVCProject_ITI.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly Context _context;
        private readonly DbSet<T> _dbSet;

        public Repository(Context context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll() => await _dbSet.ToListAsync();
        public async Task<IQueryable<T>> GetAllWithInclude(params string[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await Task.FromResult(query);
        }

        public async Task<T?> GetById(int id) => await _dbSet.FindAsync(id);

        public async Task Add(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task SaveChanges() => await _context.SaveChangesAsync();


        public void Add(TaskItem task)
        {
            _context.TaskItems.Add(task);
        }

    }
}
