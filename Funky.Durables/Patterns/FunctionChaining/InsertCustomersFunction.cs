using System;
using System.Text;
using System.Threading.Tasks;
using Funky.Durables.Extensions;
using Funky.Durables.Orchestrators;
using Funky.Durables.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Funky.Durables.Patterns.FunctionChaining
{
    public class InsertCustomersFunction
    {
        [FunctionName(nameof(InsertCustomersFunction))]
        public async Task<IActionResult> ClassifyAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "customers")]HttpRequest request,
            [DurableClient]IDurableOrchestrationClient client)
        {
            var fileRecordsRequest = await request.GetModel<InsertCustomersRequest>();

            var instanceId = Guid.NewGuid().ToString("N");

            await client.StartNewAsync(nameof(CustomerRecordOrchestratorFunction), instanceId, fileRecordsRequest);

            var actionResult = await client.WaitForCompletionOrCreateCheckStatusResponseAsync(request, instanceId, TimeSpan.FromSeconds(2));

            return actionResult;
        }

    }
}
