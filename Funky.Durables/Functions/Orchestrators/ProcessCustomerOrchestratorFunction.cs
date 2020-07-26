using System.Threading.Tasks;
using Funky.Durables.Core;
using Funky.Durables.Functions.Activities;
using Funky.Durables.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Functions.Orchestrators
{
    public class ProcessCustomerOrchestratorFunction
    {
        [FunctionName(nameof(ProcessCustomerOrchestratorFunction))]
        public async Task<Result> ProcessAsync([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var fileName = context.GetInput<string>();

            var operation = await context.CallActivityAsync<Result<InsertCustomersRequest>>(nameof(ReadCustomerRecordsActivityFunction), fileName);

            if (!operation.Status)
            {
                return Result.Failure(operation.ErrorMessage);
            }

            await context.CallSubOrchestratorAsync(nameof(CustomerRecordOrchestratorFunction), operation.Data);

            return Result.Success();
        }
    }
}