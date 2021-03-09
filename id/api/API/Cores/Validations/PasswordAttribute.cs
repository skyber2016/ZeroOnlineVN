using API.Cores.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using API.Common;

namespace API.Cores.Validations
{
    public class PasswordAttribute : ValidationAttribute
    {
        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        private PasswordScore Score { get; set; }
        public PasswordAttribute(PasswordScore score, Message message)
        {
            this.Score = score;
            this.ErrorMessage = MessageConstants.GetMessage(message);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }
            var password = (string)value;
            var scorePassword = CheckStrength(password);
            if(scorePassword < this.Score)
            {
                throw new BadRequestException(this.ErrorMessage);
            }
                return ValidationResult.Success;
        }
        private PasswordScore CheckStrength(string password)
        {
            int score = 0;

            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;

            if (password.Length >= 8) // >= 8
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.Match(password, @"\d+", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"[a-z]", RegexOptions.ECMAScript).Success &&
              Regex.Match(password, @"[A-Z]", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success)
                score++;

            return (PasswordScore)score;
        }
    }
}
