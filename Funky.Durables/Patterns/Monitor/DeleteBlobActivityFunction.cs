using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Funky.Durables.Constants;
using Funky.Durables.Core;
using Funky.Durables.Requests;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Funky.Durables.Patterns.Monitor
{
    public class FileInformation
    {
        public List<FileRecord> ValidRecords { get; set; }
        public List<FileRecord> InvalidRecords { get; set; }
        public List<FileRecord> InvalidRowRecords { get; set; }
    }

    public class FileRecord
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class FileProcessorFunction
    {
        [FunctionName(nameof(FileProcessorFunction))]
        public async Task ProcessAsync([BlobTrigger("%InputContainer%/{fileName}")]
            Stream stream, string fileName,
            [DurableClient]IDurableOrchestrationClient client)
        {
            var orchestrationStatus = await client.GetStatusAsync(fileName);
            //if (orchestrationStatus != null && orchestrationStatus.RuntimeStatus != OrchestrationRuntimeStatus.Completed)
            //{
            //    await client.TerminateAsync(fileName, "Terminating the process");
            //}

            await client.StartNewAsync(nameof(FileProcessorOrchestratorFunction), fileName);
        }
    }

    public class FileProcessorOrchestratorFunction
    {
        [FunctionName(nameof(FileProcessorOrchestratorFunction))]
        public async Task OrchestrateAsync([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var fileName = context.GetInput<string>();
            var fileInformation = await context.CallActivityAsync<FileInformation>(nameof(ReadFileActivityFunction), fileName);

            await context.CallSubOrchestratorAsync(nameof(DeleteBlobMonitorFunction), $"{context.InstanceId}-monitor", null);

            //context.CallActivityAsync(nameof(InsertValidRecordsActivityFunction), fileInformation.ValidRecords);
            //context.CallActivityAsync(nameof(InsertInvalidRecordsActivityFunction), fileInformation.InvalidRecords);
            //context.CallActivityAsync(nameof(InsertInvalidDataRowRecordsActivityFunction), fileInformation.InvalidRowRecords);

        }
    }

    public class ReadFileActivityFunction
    {
        [FunctionName(nameof(ReadFileActivityFunction))]
        public async Task<FileInformation> ReadAsync([ActivityTrigger]IDurableActivityContext context)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));

            var fileName = context.GetInput<string>();
            //
            // TODO: Read the file
            //
            return new FileInformation
            {
                ValidRecords = new List<FileRecord>(),
                InvalidRecords = new List<FileRecord>(),
                InvalidRowRecords = new List<FileRecord>()
            };
        }
    }

    public class InsertFileRecordActivityFunction
    {
        [FunctionName(nameof(InsertFileRecordActivityFunction))]
        public async Task<Result> InsertAsync([ActivityTrigger] IDurableActivityContext context,
            [DurableClient]IDurableClient client)
        {
            var records = context.GetInput<List<FileRecord>>();
            //
            // TODO: Insert records
            //
            return Result.Success();
        }
    }

    public class DeleteBlobMonitorFunction
    {
        [FunctionName(nameof(DeleteBlobMonitorFunction))]
        public async Task DeleteAsync([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var fileName = context.GetInput<string>();

            var validRecordsTask = context.WaitForExternalEvent(DataInsertionConstants.ValidRecords);
            var invalidRecordsTask = context.WaitForExternalEvent(DataInsertionConstants.InvalidRecords);
            var invalidRowRecordsTask = context.WaitForExternalEvent(DataInsertionConstants.InvalidRowRecords);

            await Task.WhenAll(validRecordsTask, invalidRecordsTask, invalidRowRecordsTask);

            await context.CallActivityAsync(nameof(DeleteBlobActivityFunction), fileName);
        }
    }

    public class DeleteBlobActivityFunction
    {
        private readonly ILogger<DeleteBlobActivityFunction> _logger;

        public DeleteBlobActivityFunction(ILogger<DeleteBlobActivityFunction> logger)
        {
            _logger = logger;
        }

        [FunctionName(nameof(DeleteBlobActivityFunction))]
        public async Task DeleteAsync([ActivityTrigger]IDurableActivityContext context)
        {
            var blobName = context.GetInput<string>();
            await Task.Delay(TimeSpan.FromSeconds(3));
            //
            // TODO: Delete the blob from the container
            //
        }
    }
}
