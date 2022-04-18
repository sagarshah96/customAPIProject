using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomAPIProject.Repository
{
    public interface _IRepository<T>
    {
        //IQueryable<T> FindAll();
        //IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        //void Create(T entity);
        //void Update(T entity);
        //void Delete(T entity);

        #region MyCode
        int Add(T t);

        int Update(T t);

        bool Delete(int id);

        List<T> GetAll();

        T GetByID(int id);

        #endregion
    }
}
