using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Funky.Durables.Models;
using Funky.Durables.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Funky.Durables.Activities
{
    public class ClassifyFileRecordsActivityFunction
    {
        private readonly IValidator<FileRecord> _validator;

        public ClassifyFileRecordsActivityFunction(IValidator<FileRecord> validator)
        {
            _validator = validator;
        }

        [FunctionName(nameof(ClassifyFileRecordsActivityFunction))]
        public Task<FileInformation> ClassifyAsync([ActivityTrigger]IDurableActivityContext context)
        {
            var fileRecordsRequest = context.GetInput<FileRecordsRequest>();

            var validRecords = new List<FileRecord>();
            var invalidRecords = new List<FileRecord>();
            var invalidRowRecords = new List<FileRecord>();

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

            var fileInformation = new FileInformation
            {
                ValidRecords = validRecords,
                InvalidRecords = invalidRecords,
                InvalidRowRecords = invalidRowRecords
            };

            return Task.FromResult(fileInformation);
        }
    }
}