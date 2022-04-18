using CustomAPIProject.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomAPIProject.Repository
{
    public class CustomerRepository<T> : _IRepository<Customer> where T : class
    {
        private readonly DBContext _dBContext;

        public CustomerRepository(DBContext dBContext)
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
        public int Add(Customer t)
        {
            _dBContext.Customer.Add(t);
            _dBContext.SaveChanges();

            return t.CustomerId;
        }

        public bool Delete(int id)
        {
            var customer = _dBContext.Customer.Find(id);
            if (customer != null)
            {
                _dBContext.Remove(customer);
                _dBContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Customer> GetAll()
        {
            return _dBContext.Customer.ToList();
        }

        public Customer GetByID(int id)
        {
            return _dBContext.Customer.Where(x => x.CustomerId == id).FirstOrDefault();
        }

        public int Update(Customer t)
        {
            _dBContext.Update(t);
            _dBContext.SaveChanges();

            return t.CustomerId;
        }

        #endregion
    }
}
