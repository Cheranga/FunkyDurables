using System.Collections.Generic;
using System.Threading.Tasks;
using Dynamitey.DynamicObjects;
using Funky.Durables.Activities;
using Funky.Durables.Core;
using Funky.Durables.DataAccess;
using Funky.Durables.DataAccess.Commands;
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

            var fileInformation = await context.CallActivityAsync<CustomerFileInformation>(nameof(ClassifyFileRecordsActivityFunction), fileRecordsRequest);

            var validRecordsCommand = GetCommand("valid", fileInformation.ValidRecords);

            await context.CallSubOrchestratorAsync<Result>(nameof(InsertDataOrchestrator), validRecordsCommand);

            //var insertValidRecordsOperation = await context.CallActivityAsync<Result>(nameof(InsertFileRecordActivityFunction), fileInformation.ValidRecords);
        }

        [Deterministic]
        public InsertCustomerDataCommand GetCommand(string category, List<CustomerFileRecord> records)
        {
            return new InsertCustomerDataCommand
            {
                Category = category,
                Records = records
            };
        }
    }
}