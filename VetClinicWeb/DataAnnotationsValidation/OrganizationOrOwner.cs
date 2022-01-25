using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VetClinic.DataAnnotationsValidations
{
    public class OrganizationOrOwner : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int? organization;
            int? owner;
            try
            {
                organization = (int)validationContext.ObjectType.GetProperty("Organization").GetValue(validationContext.ObjectInstance, null);
            }
            catch (System.NullReferenceException)
            {
                organization = null;
            }

            try
            {
                owner = (int)validationContext.ObjectType.GetProperty("Owner").GetValue(validationContext.ObjectInstance, null);
            }
            catch (System.NullReferenceException)
            {
                owner = null;
            }

            if (organization != null && owner != null)
                return new ValidationResult("You can't set both organization and owner!");
            else if (organization == null && owner == null)
                return new ValidationResult("Both organization and onwer shouldn't be empty");

            return ValidationResult.Success;
        }
    }
}
