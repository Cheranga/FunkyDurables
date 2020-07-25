using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Funky.Durables.Core;
using Funky.Durables.DataAccess;
using Funky.Durables.Models;
using Funky.Durables.Requests;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.WindowsAzure.Storage.File.Protocol;

namespace Funky.Durables.Activities
{
    public class ReadCustomerRecordsActivityFunction
    {
        private readonly CloudBlobContainer _blobContainer;

        public ReadCustomerRecordsActivityFunction(DatabaseConfiguration databaseConfiguration)
        {
            var storageAccount = CloudStorageAccount.Parse(databaseConfiguration.ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = blobClient.GetContainerReference("customers");
        }

        [FunctionName(nameof(ReadCustomerRecordsActivityFunction))]
        public async Task<Result<InsertCustomersRequest>> ReadFileRecordsAsync([ActivityTrigger]IDurableActivityContext context)
        {
            var fileName = context.GetInput<string>();

            var insertCustomersRequest = await GetFileContent(fileName);
            
            return Result<InsertCustomersRequest>.Success(insertCustomersRequest);
        }

        private async Task<InsertCustomersRequest> GetFileContent(string fileName)
        {
            var blob = _blobContainer.GetBlobReference(fileName);

            using (var stream = new MemoryStream())
            {
                await blob.DownloadToStreamAsync(stream);
                stream.Position = 0;

                using (var streamReader = new StreamReader(stream))
                {
                    using (var csv = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.CurrentCulture)
                    {
                        PrepareHeaderForMatch = (header, index) => header.ToUpper(),
                        HeaderValidated = null,
                        MissingFieldFound = null,
                        Delimiter = ","
                    }))
                    {
                        var customers = csv.GetRecords<CustomerFileRecord>()?.ToList() ?? new List<CustomerFileRecord>();
                        
                        return new InsertCustomersRequest
                        {
                            Records = customers
                        };
                    }
                }
            }
        }
    }
}
