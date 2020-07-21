using System.Threading.Tasks;
using Funky.Durables.Activities;
using Funky.Durables.Core;
using Funky.Durables.Models;
using Funky.Durables.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Orchestrators
{
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
}