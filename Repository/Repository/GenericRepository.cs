using Core.Entities;
using Core.Interfaces;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly BankingSystemDbContext _dbContext;
        public GenericRepository(BankingSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<T> GetAll()
        => _dbContext.Set<T>().ToList();

        public IEnumerable<T> GetAllFiltered(Func<T, bool> Filter)
        {
            return _dbContext.Set<T>()
                   .Where(Filter)
                   .ToList();
        }

        public IEnumerable<T> GetAllPaged(int pageNumber, int pageSize)
        {
            return _dbContext.Set<T>()
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();
        }

        public T GetByID(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }


        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
