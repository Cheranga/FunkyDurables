using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funky.Durables.DataAccess.Commands;
using Funky.Durables.DataAccess.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Activities
{
    public class GetInsertRecordCommandsActivityFunction
    {
        [FunctionName(nameof(GetInsertRecordCommandsActivityFunction))]
        public Task<List<CustomerDataWriteModel>> GetWriteModels([ActivityTrigger] IDurableActivityContext context)
        {
            var command = context.GetInput<InsertCustomerDataCommand>();
            var category = command.Category.ToUpper();

            var models = command.Records.Select(x => new CustomerDataWriteModel
            {
                PartitionKey = category,
                RowKey = Guid.NewGuid().ToString("N"),
                Name = x.Name,
                Address = x.Address
            }).ToList();

            return Task.FromResult(models);
        }
    }
}