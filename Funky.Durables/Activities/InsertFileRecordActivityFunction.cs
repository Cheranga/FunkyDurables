using System;
using System.Threading.Tasks;
using Funky.Durables.Core;
using Funky.Durables.DataAccess;
using Funky.Durables.DataAccess.CommandHandlers;
using Funky.Durables.DataAccess.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Funky.Durables.Activities
{
    public class InsertFileRecordActivityFunction
    {
        private readonly InsertCustomerDataCommandHandler _commandHandler;
        private readonly ILogger<InsertFileRecordActivityFunction> _logger;
        private readonly CloudTable _table;

        public InsertFileRecordActivityFunction(InsertCustomerDataCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        [FunctionName(nameof(InsertFileRecordActivityFunction))]
        public async Task<Result> InsertAsync([ActivityTrigger] IDurableActivityContext context)
        {
            var model = context.GetInput<CustomerDataWriteModel>();
            var operation = await _commandHandler.ExecuteAsync(model);
            return operation;
        }
    }
}