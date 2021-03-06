﻿using System.Collections.Generic;
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
    public class CustomerRecordOrchestratorFunction
    {
        [FunctionName(nameof(CustomerRecordOrchestratorFunction))]
        public async Task OrchestrateAsync([OrchestrationTrigger]IDurableOrchestrationContext context)
        {
            var fileRecordsRequest = context.GetInput<InsertCustomersRequest>();

            var fileInformation = await context.CallActivityAsync<CustomerFileInformation>(nameof(GetClassifyFileRecordsActivityFunction), fileRecordsRequest);

            var validRecordsCommand = GetCommand("valid", fileInformation.ValidRecords);
            var invalidRecordsCommand = GetCommand("invalid", fileInformation.InvalidRecords);
            var invalidRowRecordsCommand = GetCommand("invalidrows", fileInformation.InvalidRowRecords);

            var taskValidRecords = context.CallSubOrchestratorAsync<Result>(nameof(InsertDataOrchestrator), validRecordsCommand);
            var taskInvalidRecords = context.CallSubOrchestratorAsync<Result>(nameof(InsertDataOrchestrator), invalidRecordsCommand);
            var taskInvalidRowRecords = context.CallSubOrchestratorAsync<Result>(nameof(InsertDataOrchestrator), invalidRowRecordsCommand);

            await Task.WhenAll(taskValidRecords, taskInvalidRecords, taskInvalidRowRecords);


            //var insertValidRecordsOperation = await context.CallActivityAsync<Result>(nameof(InsertFileRecordActivityFunction), fileInformation.ValidRecords);
        }

        
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