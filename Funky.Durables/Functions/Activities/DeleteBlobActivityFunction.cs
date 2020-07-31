using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funky.Durables.DataAccess;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Funky.Durables.Functions.Activities
{
    public class DeleteBlobActivityFunction
    {
        private readonly ILogger<DeleteBlobActivityFunction> _logger;
        private readonly CloudBlobContainer _blobContainer;

        public DeleteBlobActivityFunction(DatabaseConfiguration databaseConfiguration, ILogger<DeleteBlobActivityFunction> logger)
        {
            var storageAccount = CloudStorageAccount.Parse(databaseConfiguration.ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = blobClient.GetContainerReference("customers");
            _logger = logger;
        }

        [FunctionName(nameof(DeleteBlobActivityFunction))]
        public async Task<bool> DeleteAsync([ActivityTrigger]IDurableActivityContext context)
        {
            var blobName = context.GetInput<string>();

            try
            {
                var blob = _blobContainer.GetBlobReference(blobName);
                await blob.DeleteIfExistsAsync();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error when deleting the blob from the container.");
            }

            return false;
        }
    }
}
