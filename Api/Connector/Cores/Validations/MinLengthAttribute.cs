using Forum_API.Cores.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Forum_API.Cores.Validations
{
    public class MaxLengthAttribute : ValidationAttribute
    {
        private int MaxLength { get; set; }
        public MaxLengthAttribute(int maxLength,string messageError = "{0} có độ dài tối đa là {1}")
        {
            this.MaxLength = maxLength;
            this.ErrorMessage = messageError;
        }
        public override bool IsValid(object value)
        {
            var lengthValue = value?.ToString()?.Length ?? 0;
            return lengthValue <= this.MaxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (this.IsValid(value))
            {
                return ValidationResult.Success;
            }
            throw new BadRequestException(string.Format(ErrorMessage, validationContext.DisplayName, this.MaxLength));
        }
    }
}
