using System;

namespace SalaryCalculatorApp.Utils
{
    /// <summary>
    /// Provides a centralized way to handle exceptions and output error messages.
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Handles and logs the specified exception based on its type.
        /// </summary>
        /// <param name="ex">The exception to handle.</param>
        public static void Handle(Exception ex)
        {
            // Determine the type of exception and provide a relevant error message.
            switch (ex)
            {
                case FormatException _:
                    // Handle format-related errors, such as invalid input formats.
                    Console.WriteLine($"Input format error: {ex.Message}");
                    break;
                case ArgumentException _:
                    // Handle argument-related errors, such as invalid or out-of-range arguments.
                    Console.WriteLine($"Input error: {ex.Message}");
                    break;
                default:
                    // Handle other types of exceptions, providing a generic error message.
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    break;
            }
        }
    }
}
