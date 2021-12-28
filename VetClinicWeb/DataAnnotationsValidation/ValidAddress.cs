using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VetClinic.DataAnnotationsValidations
{
    public class ValidAddress : ValidationAttribute
    {
        public ValidAddress() : base("{0} is not a valid address.") { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string strValue = value as string;

            if (!string.IsNullOrEmpty(strValue))
            {
                if (Regex.IsMatch(strValue, @"^[A-Z][a-z]*[ ][1-9]{1,2}[a-z]?[ ][0-9]{2}[-][0-9]{3}"))
                    return ValidationResult.Success;

                if (Regex.IsMatch(strValue, @"^[A-Z][a-z]*[ ][A-Za-z1-9 ]*[a-z][ ][1-9][0-9]{0,2}[ ]m.[1-9][0-9]{0,2}[ ][0-9]{2}[-][0-9]{3}"))
                    return ValidationResult.Success;

                if (Regex.IsMatch(strValue, @"^[A-Z][a-z]*[ ][A-Za-z1-9 ]*[a-z][ ][1-9][0-9]{0,2}[ ][0-9]{2}[-][0-9]{3}"))
                    return ValidationResult.Success;

                var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
