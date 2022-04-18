using CustomAPIProject.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomAPIProject.Repository
{
    public class LoginRepository<T> : _IRepository<Login> where T : class
    {
        private readonly DBContext _dBContext;

        public LoginRepository(DBContext dBContext)
        {
            _dBContext = dBContext;
        }

        //public IQueryable<T> FindAll() => _dBContext.Set<T>().AsNoTracking();
        //public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
        //    _dBContext.Set<T>().Where(expression).AsNoTracking();
        //public void Create(T entity) => _dBContext.Set<T>().Add(entity);
        //public void Update(T entity) => _dBContext.Set<T>().Update(entity);
        //public void Delete(T entity) => _dBContext.Set<T>().Remove(entity);


        #region MyCode
        public int Add(Login t)
        {
            _dBContext.Login.Add(t);
            _dBContext.SaveChanges();

            return t.LoginId;
        }

        public bool Delete(int id)
        {
            var login = _dBContext.Login.Find(id);
            if (login != null)
            {
                _dBContext.Remove(login);
                _dBContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Login> GetAll()
        {
            return _dBContext.Login.ToList();
        }

        public Login GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public int Update(Login t)
        {
            _dBContext.Update(t);
            _dBContext.SaveChanges();

            return t.LoginId;
        }

        #endregion
    }
}
