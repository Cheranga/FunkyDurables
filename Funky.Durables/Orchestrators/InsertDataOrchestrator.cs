using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funky.Durables.Activities;
using Funky.Durables.Core;
using Funky.Durables.DataAccess.Commands;
using Funky.Durables.DataAccess.Models;
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

            var insertCommands = await context.CallActivityAsync<List<CustomerDataWriteModel>>(nameof(GetInsertRecordCommandsActivityFunction), insertCommand);

            var tasks = insertCommands.Select(x => context.CallActivityAsync<Result>(nameof(InsertFileRecordActivityFunction), x)).ToList();

            await Task.WhenAll(tasks);

            return Result.Success();
        }
    }
}