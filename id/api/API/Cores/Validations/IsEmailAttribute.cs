using API.Cores.Exceptions;
using System.ComponentModel.DataAnnotations;
using API.Common;

namespace API.Cores.Validations
{
    public class IsEmailAttribute : ValidationAttribute
    {
        public IsEmailAttribute(Message message)
        {
            this.ErrorMessage = MessageConstants.GetMessage(message);
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
