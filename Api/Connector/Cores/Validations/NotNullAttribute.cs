using Forum_API.Cores.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Forum_API.Cores.Validations
{
    public class NotNullAttribute : ValidationAttribute
    {
        public NotNullAttribute(string messageError = "{0} must not be null")
        {
            this.ErrorMessage = messageError;
        }
        public override bool IsValid(object value)
        {
            if(string.IsNullOrEmpty(value?.ToString()) || string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return false;
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!this.IsValid(value))
            {
                throw new BadRequestException(string.Format(ErrorMessage, validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
