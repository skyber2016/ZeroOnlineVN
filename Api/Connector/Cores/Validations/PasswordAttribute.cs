using Forum_API.Cores.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Forum_API.Cores.Validations
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

        private bool Nullable { get; set; }
        private int Length { get; set; }
        public PasswordAttribute(bool nullable = false, int length = 8)
        {
            this.Nullable = nullable;
            this.Length = length;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = (string)value;
            if (this.Nullable && password == null)
            {
                return ValidationResult.Success;
            }
            if (password == null)
            {
                throw new BadRequestException(string.Format("Please enter the {0}", validationContext.DisplayName));
            }
            if(password.Length < this.Length)
            {
                throw new BadRequestException($"New password must be between 6-20 characters, include at least 1 numeric character and 1 alphanumeric character");
            }
            var scorePassword = CheckStrength(value.ToString());
            if(scorePassword < PasswordScore.Strong)
            {
                throw new BadRequestException($"New password must be between 6-20 characters, include at least 1 numeric character and 1 alphanumeric character");
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

            if (password.Length >= this.Length) // >= 8
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
