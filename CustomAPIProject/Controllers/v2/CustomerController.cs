using CustomAPIProject.ApplicationContext;
using CustomAPIProject.Filters;
using CustomAPIProject.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
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

        [MapToApiVersion("2.0")]
        [Cachable(100000)]
        [Authorize(Roles.Admin)]
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customer = _CustomerRepo.GetAll().Where(x => x.IsActive == true);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }
    }
}
