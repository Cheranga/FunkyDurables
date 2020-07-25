using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Funky.Durables.Activities;
using Funky.Durables.Core;
using Funky.Durables.DataAccess;
using Funky.Durables.Orchestrators;
using Funky.Durables.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Patterns.FunctionChaining
{
    public class ProcessCustomerClientFunction
    {
        [FunctionName(nameof(ProcessCustomerClientFunction))]
        public async Task ProcessAsync([BlobTrigger("%InputContainer%/{fileName}")]Stream stream, string fileName,
            [DurableClient]IDurableOrchestrationClient client)
        {
            await client.StartNewAsync(nameof(ProcessCustomerOrchestratorFunction), Guid.NewGuid().ToString("N"), fileName);
        }
    }

    public class ProcessCustomerOrchestratorFunction
    {   

        [FunctionName(nameof(ProcessCustomerOrchestratorFunction))]
        public  async Task<Result> ProcessAsync([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var fileName = context.GetInput<string>();

            var operation = await context.CallActivityAsync<Result<InsertCustomersRequest>>(nameof(ReadCustomerRecordsActivityFunction), fileName);

            await context.CallSubOrchestratorAsync(nameof(CustomerRecordOrchestratorFunction), operation.Data);

            return Result.Success();
        }
    }
}
