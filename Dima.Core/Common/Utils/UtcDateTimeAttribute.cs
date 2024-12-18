using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Common.Utils;
public class UtcDateTimeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            // Null is considered valid, as it might be an optional field
            return ValidationResult.Success;
        }

        if (value is DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                return new ValidationResult("The date must be in UTC format.");
            }
        }
        else
        {
            return new ValidationResult("The value must be a DateTime.");
        }

        return ValidationResult.Success;
    }
}
