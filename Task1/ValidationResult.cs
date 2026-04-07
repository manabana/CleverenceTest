using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    internal class ValidationResult
    {
        public Boolean IsSuccess { get; }
        public String? ErrorText { get; }

        private ValidationResult(Boolean isSuccess, String? errorText)
        {
            IsSuccess = isSuccess;
            ErrorText = errorText;
        }

        public static ValidationResult Success() => new ValidationResult(true, null);
        public static ValidationResult Error(String errorText) { }
    }
}
