using System;

namespace Fiskalizacija2 {
    public class ValidationError : Exception {
        public object? Value { get; }

        public ValidationError(string message, object? value) : base(message) {
            Value = value;
        }

        public override string ToString() {
            if (Value == null) {
                return $"ValidationError: {Message}";
            }
            var display = Value is string ? $"\"{Value}\"" : Value.ToString();
            return $"ValidationError: {Message} (value: {display})";
        }
    }

    public static class ErrorUtils {
        public static ErrorWithMessage ParseError(Exception error) {
            return new ErrorWithMessage {
                Message = error.Message,
                Thrown = error
            };
        }
    }
}
