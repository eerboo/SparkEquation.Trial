using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SparkEquation.Trial.WebAPI.Attributes
{
    public class CategoryValidationAttribute : ValidationAttribute
    {       
        public string GetErrorMessage() => "Invalid category";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var Categories = (int[])value;
            if (Categories == null || Categories.Length == 0)
            {
                return ValidationResult.Success;
            }
            else if (Categories.Any(c => c == 0 || c > 5))
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}
