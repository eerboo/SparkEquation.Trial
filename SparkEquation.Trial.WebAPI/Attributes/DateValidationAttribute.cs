using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SparkEquation.Trial.WebAPI.Attributes
{
    public class ExpirationDateValidationAttribute : ValidationAttribute
    {       
        public string GetErrorMessage() => "Expiration date should expire not less than 30 days since now";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ExpirationDate = (DateTime?)value;
            if (!ExpirationDate.HasValue)
            {
                return ValidationResult.Success;
            }
            else if ((ExpirationDate.Value - DateTime.Now).TotalDays < 30)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}
