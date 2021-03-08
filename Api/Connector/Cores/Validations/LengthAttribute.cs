using Forum_API.Cores.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Forum_API.Cores.Validations
{
    public class LengthAttribute : ValidationAttribute
    {
        private int? Min { get; set; }
        private int? Max { get; set; }
        public LengthAttribute(int min = 0, int max = -1, string messageError = "{0} phải có độ dài từ {1} đến {2}")
        {
            this.Min = min;
            this.Max = max;
            this.ErrorMessage = messageError;
            if(this.Max == -1)
            {
                this.ErrorMessage = "{0} phải có độ dài ít nhất {1} ký tự";
            }
        }
        public override bool IsValid(object value)
        {
            if (string.IsNullOrEmpty(value?.ToString()) || string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return false;
            }
            var length = value?.ToString()?.Length ?? 0;
            if(length < Min)
            {
                return false;
            }
            if(Max != null)
            {
                if(length > Max)
                {
                    return false;
                }
                return true;
            }
            return true;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!this.IsValid(value) && Max != -1)
            {
                throw new BadRequestException(string.Format(ErrorMessage, validationContext.DisplayName, Min, Max));
            }
            else if(!this.IsValid(value))
            {
                throw new BadRequestException(string.Format(ErrorMessage, validationContext.DisplayName, Min));
            }
            return ValidationResult.Success;
        }
    }
}
