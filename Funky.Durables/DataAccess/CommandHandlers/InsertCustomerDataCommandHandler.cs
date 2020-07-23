using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funky.Durables.Core;
using Funky.Durables.DataAccess.Models;
using Funky.Durables.Extensions;
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

        public async Task<Result> ExecuteAsync(List<CustomerDataWriteModel> records)
        {
            try
            {
                var groups = records.SplitList().ToList();

                foreach (var @group in groups)
                {
                    var batchOperation = new TableBatchOperation();

                    foreach (var record in @group)
                    {
                        var tableOperation = TableOperation.InsertOrReplace(record);
                        batchOperation.Add(tableOperation);
                    }

                    await _cloudTable.ExecuteBatchAsync(batchOperation);
                }

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