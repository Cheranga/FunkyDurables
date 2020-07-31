using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Functions.TestFunctions
{
    public class TestCreateCustomerActivityFunction
    {
        [FunctionName(nameof(TestCreateCustomerActivityFunction))]
        public async Task CreateCustomerAsync([ActivityTrigger] IDurableActivityContext context,
            [DurableClient]IDurableOrchestrationClient client)
        {
            var customer = context.GetInput<Customer>();
            //
            // TODO: Create customer in data storage
            //
            await Task.Delay(TimeSpan.FromSeconds(10));

            await client.RaiseEventAsync(context.InstanceId, "customercreated", new CustomerCreatedOperation
            {
                InstanceId = context.InstanceId,
                Status = true,
                Customer = customer
            });
        }
    }
}