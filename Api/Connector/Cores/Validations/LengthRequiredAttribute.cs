using Forum_API.Cores.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Forum_API.Cores.Validations
{
    public class LengthRequiredAttribute : ValidationAttribute 
    {
        private int length { get; set; }
        public LengthRequiredAttribute(int length, string messageError = "{0} bắt buộc là {1} ký tự")
        {
            this.ErrorMessage = messageError;
            this.length = length;
        }
        public override bool IsValid(object value)
        {
            var lengthValue = value?.ToString()?.Length ?? 0;
            return lengthValue == this.length;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!this.IsValid(value))
            {
                throw new BadRequestException(string.Format(ErrorMessage, validationContext.DisplayName, this.length));
            }
            return ValidationResult.Success;
        }
    }
}
