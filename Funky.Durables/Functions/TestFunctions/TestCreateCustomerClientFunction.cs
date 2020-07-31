using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Funky.Durables.Functions.TestFunctions
{
    public class TestCreateCustomerClientFunction
    {
        [FunctionName(nameof(TestCreateCustomerClientFunction))]
        public async Task<IActionResult> CreateCustomerAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "testcustomers")]
            HttpRequest request,
            [DurableClient]IDurableOrchestrationClient client)
        {
            var content = await new StreamReader(request.Body).ReadToEndAsync();
            var customer = JsonConvert.DeserializeObject<Customer>(content);

            var instanceId = Guid.NewGuid().ToString("N");
            await client.StartNewAsync(nameof(TestCreateCustomerOrchestrationFunction),instanceId, customer);

            return new AcceptedResult();
        }

    }
}
