using Forum_API.Common;
using Forum_API.Cores.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Forum_API.Cores.Validations
{
    public class IsEmailAttribute : ValidationAttribute
    {
        public IsEmailAttribute(string messageError = null)
        {
            this.ErrorMessage = messageError ?? MessageCodeContants.EMAIL_INVALID;
        }
        public override bool IsValid(object value)
        {
            try
            {
                var email = value?.ToString();
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!this.IsValid(value))
            {
                throw new BadRequestException(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
