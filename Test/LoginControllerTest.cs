using CustomAPIProject.ApplicationContext;
using CustomAPIProject.Repository;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{

    [TestFixture]
    public class LoginControllerTest
    {

        [SetUp]
        public void Setup()
        {
           
        }


        [Test]
        public void TestLogin()
        {
            //var repository = new MockRepository(MockBehavior.Strict) { DefaultValue = DefaultValue.Mock };
            //var mockCustomer = repository.Create<_IRepository<Customer>>();
            //mockCustomer.Setup(x => x.GetAll());
            //repository.Verify();

            //List<Customer> lst = new List<Customer>();
            //Mock<_IRepository<Customer>> chk = new Mock<_IRepository<Customer>>();
            //chk.Setup(x => x.GetAll()).Returns(lst);
        }

       
    }
}
