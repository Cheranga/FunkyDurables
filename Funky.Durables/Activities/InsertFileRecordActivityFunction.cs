using System.Collections.Generic;
using System.Threading.Tasks;
using Funky.Durables.Core;
using Funky.Durables.DataAccess.CommandHandlers;
using Funky.Durables.DataAccess.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Activities
{
    public class InsertFileRecordActivityFunction
    {
        private readonly InsertCustomerDataCommandHandler _commandHandler;

        public InsertFileRecordActivityFunction(InsertCustomerDataCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        [FunctionName(nameof(InsertFileRecordActivityFunction))]
        public async Task<Result> InsertAsync([ActivityTrigger] IDurableActivityContext context)
        {
            var records = context.GetInput<List<CustomerDataWriteModel>>();

            var operation = await _commandHandler.ExecuteAsync(records);
            return operation;
        }
    }
}