using CustomAPIProject.ApplicationContext;
using CustomAPIProject.Filters;
using CustomAPIProject.Models;
using CustomAPIProject.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly _IRepository<Customer> _CustomerRepo;
        private readonly _IRepository<Login> _LoginRepo;
        //private readonly IMemoryCache _cache;
        public CustomerController(_IRepository<Customer> crepository, _IRepository<Login> lrepository)/*, IMemoryCache memoryCache)*/
        {
            _CustomerRepo = crepository;
            _LoginRepo = lrepository;
          //  _cache = memoryCache;

        }

        /// Custom Cachable
        [Cachable(100000)]
        [Authorize(Roles.Admin)]
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customer = _CustomerRepo.GetAll().Where(x => x.IsActive == true);
            if (customer == null)
                return NotFound();
            return Ok(customer);

            #region Custom Cache Using IMemoryCache
            //if (!_cache.TryGetValue("_CustomerCache", out List<Customer> customer))
            //{
            //    customer = _CustomerRepo.GetAll().Where(x => x.IsActive == true).ToList();
            //    var cacheEntryOptions = new MemoryCacheEntryOptions
            //    {
            //        AbsoluteExpiration = DateTime.Now.AddMinutes(5),
            //        SlidingExpiration = TimeSpan.FromMinutes(2),
            //        Size = 1024,
            //    };
            //    _cache.Set("_CustomerCache", customer, cacheEntryOptions);
            //}
            //return Ok(customer);
            #endregion
        }


        /// <summary>
        /// Rewrite URL
        /// </summary>
        /// <returns></returns>

        //[HttpGet]
        //public IActionResult GetAllCustomersv2()
        //{
        //    var customer = _CustomerRepo.GetAll().Where(x => x.IsActive == true);
        //    return Ok(customer);
        //}

        [Authorize(Roles.Admin, Roles.Customer)]
        [HttpGet("{id}")]
        public IActionResult GetCustomerByID(int id)
        {
            var customer = _CustomerRepo.GetByID(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public IActionResult SaveCustomer(CustomerModel objCustomer)
        {
            Customer obj = new Customer();
            obj.FirstName = objCustomer.FirstName;
            obj.LastName = objCustomer.LastName;
            obj.MiddleName = objCustomer.MiddleName;
            obj.MobileNo = objCustomer.MobileNo;
            obj.IsActive = true;
            int CustomerID = _CustomerRepo.Add(obj);

            Login objLogin = new Login();
            objLogin.CustomerId = CustomerID;
            objLogin.Email = objCustomer.Email;
            objLogin.Password = Common.Encrypt(objCustomer.Password);
            objLogin.RoleId = (int)Roles.Customer;
            int LoginID = _LoginRepo.Add(objLogin);
            return Ok("Customer Saved Successfully.!!");
        }

        [Authorize(Roles.Admin, Roles.Customer)]
        [HttpPut]
        public IActionResult UpdateCustomer(CustomerModel objCustomer)
        {
            Customer obj = _CustomerRepo.GetByID(objCustomer.CustomerId);
            obj.FirstName = objCustomer.FirstName;
            obj.LastName = objCustomer.LastName;
            obj.MiddleName = objCustomer.MiddleName;
            obj.MobileNo = objCustomer.MobileNo;
            int CustomerID = _CustomerRepo.Update(obj);

            Login objLogin = _LoginRepo.GetByID(objCustomer.CustomerId);
            objLogin.CustomerId = CustomerID;
            objLogin.Email = objCustomer.Email;
            objLogin.Password = Common.Encrypt(objCustomer.Password);
            int LoginID = _LoginRepo.Update(objLogin);
            return Ok("Customer Update Successfully.!!");
        }

        [Authorize(Roles.Admin)]
        [HttpDelete("{id}")]
        public IActionResult DeleteCusomer(int id)
        {
            Customer obj = _CustomerRepo.GetByID(id);
            obj.IsActive = false;
            int CustomerID = _CustomerRepo.Update(obj);
            return Ok("Deleted Successfully.!!");
        }
    }
}
