using API.Cores.Exceptions;
using System.ComponentModel.DataAnnotations;
using API.Common;

namespace API.Cores.Validations
{
    public class MaxLengthAttribute : ValidationAttribute
    {
        private int MaxLength { get; set; }
        private string ColumnName { get; set; }
        public MaxLengthAttribute(int maxLength, Message message, string columnName)
        {
            this.MaxLength = maxLength;
            this.ErrorMessage = MessageConstants.GetMessage(message);
            this.ColumnName = columnName;
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
            throw new BadRequestException(string.Format(ErrorMessage, this.ColumnName, this.MaxLength));
        }
    }
}
