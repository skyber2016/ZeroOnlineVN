using System.ComponentModel.DataAnnotations;

namespace API_CORE.Commons.Validations
{
    public class HtmlSecurityAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            validationContext
                .ObjectType
                .GetProperty(validationContext.MemberName)
                .SetValue(validationContext.ObjectInstance, HtmlHelper.Clean(value.ToString()), null);
            return ValidationResult.Success;
        }
    }
}
