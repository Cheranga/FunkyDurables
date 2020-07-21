using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funky.Durables.Models;
using Funky.Durables.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Activities
{
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