using System;
using System.Diagnostics;

namespace Payroll.Domain
{
    internal static class Throw
    {
        [DebuggerStepThrough]
        public static T IfArgumentIsNull<T>(
            T value,
            string argumentName
            )
        {
            if (value == null)
                throw new ArgumentNullException(argumentName);

            return value;
        }

        [DebuggerStepThrough]
        public static string IfArgumentIsNullOrEmpty(
            string value,
            string argumentName
            )
        {
            if (value == null)
                throw new ArgumentNullException(argumentName);

            if (value == string.Empty)
                throw new ArgumentException($"'{argumentName}' should not be empty.");

            return value;
        }

        [DebuggerStepThrough]
        public static int IfArgumentIsNegative(
            int value,
            string argumentName)
        {
            if (value < 0)
                throw new ArgumentException($"'{argumentName}' could not be negative.");
            return value;

        }

        [DebuggerStepThrough]
        public static decimal IfArgumentIsNegative(
            decimal value,
            string argumentName)
        {
            if (value < 0)
                throw new ArgumentException($"'{argumentName}' could not be negative.");
            return value;

        }
    }
}