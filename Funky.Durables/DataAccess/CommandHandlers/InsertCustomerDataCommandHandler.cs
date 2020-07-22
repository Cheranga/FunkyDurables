using System;
using System.Threading.Tasks;
using Funky.Durables.Core;
using Funky.Durables.DataAccess.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;

namespace Funky.Durables.DataAccess.CommandHandlers
{
    public class InsertCustomerDataCommandHandler
    {
        private readonly CloudTable _cloudTable;
        private readonly ILogger<InsertCustomerDataCommandHandler> _logger;

        public InsertCustomerDataCommandHandler(IDataStorageFactory dataStorageFactory, ILogger<InsertCustomerDataCommandHandler> logger)
        {
            _cloudTable = dataStorageFactory.GetTable();
            _logger = logger;
        }

        public async Task<Result> ExecuteAsync(CustomerDataWriteModel model)
        {
            try
            {
                var tableOperation = TableOperation.InsertOrReplace(model);
                await _cloudTable.ExecuteAsync(tableOperation);

                return Result.Success();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error when inserting record.");
            }

            return Result.Failure("Error when inserting record.");
        }
    }
}