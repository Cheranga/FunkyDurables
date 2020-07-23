using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funky.Durables.Activities;
using Funky.Durables.Core;
using Funky.Durables.DataAccess.Commands;
using Funky.Durables.DataAccess.Models;
using Funky.Durables.Extensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Orchestrators
{
    public class InsertDataOrchestrator
    {
        [FunctionName(nameof(InsertDataOrchestrator))]
        public async Task<Result> InsertAsync([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var insertCommand = context.GetInput<InsertCustomerDataCommand>();
            if (insertCommand.Records == null || !insertCommand.Records.Any())
            {
                return Result.Success();
            }

            var insertCommands = await context.CallActivityAsync<List<CustomerDataWriteModel>>(nameof(GetCustomerDataRecordsFunction), insertCommand);

            var groups = insertCommands.SplitList(1000).ToList();

            //var tasks = new List<Task<Result>>();

            foreach (var @group in groups)
            {
                await context.CallActivityAsync<Result>(nameof(InsertFileRecordActivityFunction), @group);
                //var task = context.CallActivityAsync<Result>(nameof(InsertFileRecordActivityFunction), @group);
                //tasks.Add(task);
            }

            //await Task.WhenAll(tasks);

            return Result.Success();
        }
    }
}