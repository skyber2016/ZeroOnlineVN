using Forum_API.Cores.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Forum_API.Cores.Validations
{
    public class MinLengthAttribute : ValidationAttribute
    {
        private int MinLength { get; set; }
        public MinLengthAttribute(int minLength,string messageError = "{0} có độ dài ít nhất là {1}")
        {
            this.MinLength = minLength;
        }
        public override bool IsValid(object value)
        {
            var lengthValue = value?.ToString()?.Length ?? 0;
            return lengthValue >= this.MinLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (this.IsValid(value))
            {
                return ValidationResult.Success;
            }
            throw new BadRequestException(string.Format(ErrorMessage, validationContext.DisplayName, this.MinLength));
        }
    }
}
