using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Funky.Durables.Core;
using Funky.Durables.Extensions;
using Funky.Durables.Patterns.Monitor;
using Funky.Durables.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Funky.Durables.Patterns.FunctionChaining
{
    public class ClassifyOrdersFunction
    {
        [FunctionName(nameof(ClassifyOrdersFunction))]
        public async Task<IActionResult> ClassifyAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders")]HttpRequest request,
            [DurableClient]IDurableOrchestrationClient client)
        {
            var fileRecordsRequest = await request.GetModel<FileRecordsRequest>();

            await client.StartNewAsync(nameof(ClassifyOrdersOrchestratorFunction), fileRecordsRequest);

            return new OkResult();
        }

    }

    public class ClassifyOrdersOrchestratorFunction
    {
        [FunctionName(nameof(ClassifyOrdersOrchestratorFunction))]
        public async Task OrchestrateAsync([OrchestrationTrigger]IDurableOrchestrationContext context)
        {
            var fileRecordsRequest = context.GetInput<FileRecordsRequest>();

            var fileInformation = await context.CallActivityAsync<FileInformation>(nameof(ClassifyFileRecordsActivityFunction), fileRecordsRequest);

            var insertValidRecordsOperation = await context.CallActivityAsync<Result>(nameof(InsertFileRecordActivityFunction), fileInformation.ValidRecords);
        }
    }

    public class ClassifyFileRecordsActivityFunction
    {
        [FunctionName(nameof(ClassifyFileRecordsActivityFunction))]
        public async Task<FileInformation> ClassifyAsync([ActivityTrigger]IDurableActivityContext context)
        {
            var fileRecordsRequest = context.GetInput<FileRecordsRequest>();
            await Task.Delay(TimeSpan.FromSeconds(3));
            //
            // TODO: Classify the records
            //
            return new FileInformation
            {
                ValidRecords = new List<FileRecord>(),
                InvalidRecords = new List<FileRecord>(),
                InvalidRowRecords = new List<FileRecord>()
            };
        }
    }
}
