using API.Cores.Exceptions;
using System.ComponentModel.DataAnnotations;
using API.Common;

namespace API.Cores.Validations
{
    public class NotNullAttribute : ValidationAttribute
    {
        private string ColumnName { get; set; } = string.Empty;
        public NotNullAttribute(Message message, string columnName)
        {
            this.ErrorMessage = MessageConstants.GetMessage(message);
            this.ColumnName = columnName;
        }
        public NotNullAttribute(Message message)
        {
            this.ErrorMessage = MessageConstants.GetMessage(message);
        }
        public override bool IsValid(object value)
        {
            if(string.IsNullOrEmpty(value?.ToString()) || string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return false;
            }
            return true;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!this.IsValid(value))
            {
                throw new BadRequestException(string.Format(ErrorMessage, ColumnName));
            }
            return ValidationResult.Success;
        }
    }
}
