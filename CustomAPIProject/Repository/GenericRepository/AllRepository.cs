using CustomAPIProject.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.Repository.GenericRepository
{
    public class AllRepository<T> : IAllRepository<T> where T : class
    {

        private readonly DBContext _dataContext;

        private readonly DbSet<T> DbEntity;
        public AllRepository(DBContext dbContext)
        {
            _dataContext = dbContext;
            DbEntity = _dataContext.Set<T>();
        }

        public void DeleteModel(T ModelId)
        {
            T model = DbEntity.Find(ModelId);
            DbEntity.Remove(model);
        }

        public IEnumerable<T> GetModel()
        {
            return DbEntity.ToList();
        }

        public T GetModelById(int ModelId)
        {
            return DbEntity.Find(ModelId);
        }

        public void InsertModel(T model)
        {
            DbEntity.Add(model);
        }

        public void Save()
        {
            _dataContext.SaveChanges();
        }

        public void UpdateModel(T model)
        {
            _dataContext.Entry(model).State = EntityState.Modified;
        }
    }
}
