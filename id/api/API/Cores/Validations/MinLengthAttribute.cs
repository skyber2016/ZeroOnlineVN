using API.Cores.Exceptions;
using System.ComponentModel.DataAnnotations;
using API.Common;

namespace API.Cores.Validations
{
    public class MinLengthAttribute : ValidationAttribute
    {
        private int MinLength { get; set; }
        private string ColumnName { get; set; } = string.Empty;
        public MinLengthAttribute(int minLength, Message message)
        {
            this.MinLength = minLength;
            this.ErrorMessage = MessageConstants.GetMessage(message);
        }
        public MinLengthAttribute(int minLength, Message message, string columnName)
        {
            this.MinLength = minLength;
            this.ErrorMessage = MessageConstants.GetMessage(message);
            this.ColumnName = columnName;
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
            throw new BadRequestException(string.Format(ErrorMessage, this.ColumnName, this.MinLength));
        }
    }
}
