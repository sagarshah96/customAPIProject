using CustomAPIProject.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class DemoTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSum()
        {
            var testRepo = new TestRepo();
            int result = testRepo.sum(3, 5);
            Assert.IsTrue(result == 8);
        }
    }
}
