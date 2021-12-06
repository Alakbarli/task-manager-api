using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApi.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly TaskManagerDB _dbContext;
        private readonly DbSet<TEntity> _entity;

        public BaseRepository(TaskManagerDB context)
        {
            _dbContext = context;
            _entity = _dbContext.Set<TEntity>();
        }
        public IEnumerable<TEntity> All
        {
            get { return _entity; }
        }
        public IQueryable<TEntity> AllQuery
        {
            get { return _entity; }
        }
        public async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await _entity.ToListAsync();
        }

        public void Insert(TEntity entity)
        {
            _entity.Add(entity);
        }

        public void Remove(TEntity entity)
        {
            _entity.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _entity.Update(entity);
        }
        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
