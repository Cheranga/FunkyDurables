using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funky.Durables.Core;
using Funky.Durables.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Activities
{
    public class InsertFileRecordActivityFunction
    {
        [FunctionName(nameof(InsertFileRecordActivityFunction))]
        public async Task<Result> InsertAsync([ActivityTrigger] IDurableActivityContext context,
            [DurableClient]IDurableClient client)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            var records = context.GetInput<List<FileRecord>>();
            
            //
            // TODO: Insert records
            //
            return Result.Success();
        }
    }
}