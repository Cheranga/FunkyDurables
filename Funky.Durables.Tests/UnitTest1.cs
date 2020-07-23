using System;
using System.Linq;
using AutoFixture;
using Funky.Durables.Models;
using Funky.Durables.Requests;
using Newtonsoft.Json;
using Xunit;

namespace Funky.Durables.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var fixture = new Fixture();
            var fileRecords = fixture.CreateMany<CustomerFileRecord>(50000).ToList();
            var request = new InsertCustomersRequest
            {
                Records = fileRecords
            };

            var json = JsonConvert.SerializeObject(request);

        }
    }
}
