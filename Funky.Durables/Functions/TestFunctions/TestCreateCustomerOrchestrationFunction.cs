using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Functions.TestFunctions
{
    public class TestCreateCustomerOrchestrationFunction
    {
        [FunctionName(nameof(TestCreateCustomerOrchestrationFunction))]
        public async Task CreateCustomerAsync([OrchestrationTrigger] IDurableOrchestrationContext context,
            [DurableClient]IDurableOrchestrationClient client)
        {
            var customer = context.GetInput<Customer>();

            using (var cts = new CancellationTokenSource())
            {
                var timeoutTask = context.CreateTimer(context.CurrentUtcDateTime.AddMinutes(1), cts.Token);
                var customerCreatedTask = context.WaitForExternalEvent<CustomerCreatedOperation>("customercreated");

                await context.CallActivityAsync(nameof(TestCreateCustomerActivityFunction), customer);

                var winner = await Task.WhenAny(timeoutTask, customerCreatedTask);

                if (winner == customerCreatedTask)
                {
                    cts.Cancel();
                    //
                    // Customer created, do something
                    //
                    var operation = customerCreatedTask.Result;
                }
                else
                {
                    //
                    // Timeout occured, do something
                    //
                }
            }

        }
    }
}