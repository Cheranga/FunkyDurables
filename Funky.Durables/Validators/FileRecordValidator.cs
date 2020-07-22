using System;
using FluentValidation;
using Funky.Durables.Models;

namespace Funky.Durables.Validators
{
    public class FileRecordValidator : ModelValidatorBase<CustomerFileRecord>
    {
        public FileRecordValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Address).NotNull().NotEmpty();
            RuleFor(x => x.Name).Must(x => !string.Equals(x, "name", StringComparison.OrdinalIgnoreCase)).WithErrorCode("HeaderName");
            RuleFor(x => x.Address).Must(x => !string.Equals(x, "address", StringComparison.OrdinalIgnoreCase)).WithErrorCode("HeaderName");
        }
    }
}