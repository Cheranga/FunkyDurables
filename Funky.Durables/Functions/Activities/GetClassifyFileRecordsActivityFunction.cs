using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Funky.Durables.Models;
using Funky.Durables.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Functions.Activities
{
    public class GetClassifyFileRecordsActivityFunction
    {
        private readonly IValidator<CustomerFileRecord> _validator;

        public GetClassifyFileRecordsActivityFunction(IValidator<CustomerFileRecord> validator)
        {
            _validator = validator;
        }

        [FunctionName(nameof(GetClassifyFileRecordsActivityFunction))]
        public Task<CustomerFileInformation> ClassifyAsync([ActivityTrigger]IDurableActivityContext context)
        {
            var fileRecordsRequest = context.GetInput<InsertCustomersRequest>();

            var validRecords = new List<CustomerFileRecord>();
            var invalidRecords = new List<CustomerFileRecord>();
            var invalidRowRecords = new List<CustomerFileRecord>();

            foreach (var fileRecord in fileRecordsRequest.Records)
            {
                var validationResult = _validator.Validate(fileRecord);
                if (validationResult.IsValid)
                {
                    validRecords.Add(fileRecord);
                }
                else if(validationResult.Errors.Any(x=>string.Equals(x.ErrorCode, "HeaderName", StringComparison.OrdinalIgnoreCase)))
                {
                    invalidRowRecords.Add(fileRecord);
                }
                else
                {
                    invalidRecords.Add(fileRecord);
                }
            }

            var fileInformation = new CustomerFileInformation
            {
                ValidRecords = validRecords,
                InvalidRecords = invalidRecords,
                InvalidRowRecords = invalidRowRecords
            };

            return Task.FromResult(fileInformation);
        }
    }
}