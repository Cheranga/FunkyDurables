using System;
using System.IO;
using System.Threading.Tasks;
using Funky.Durables.Functions.Orchestrators;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Functions.Clients
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
}
