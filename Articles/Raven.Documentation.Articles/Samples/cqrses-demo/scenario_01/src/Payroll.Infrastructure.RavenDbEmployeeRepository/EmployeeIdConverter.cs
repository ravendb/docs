using System;
using Payroll.Domain.Model;
using Raven.Client.Converters;

namespace Payroll.Infrastructure.RavenDbEmployeeRepository
{
    internal class EmployeeIdConverter : ITypeConverter
    {
        public bool CanConvertFrom(Type sourceType)
        {
            return sourceType == typeof (EmployeeId);
        }

        public string ConvertFrom(string tag, object value, bool allowNull)
        {
            return $"{tag}{value}";
        }

        public object ConvertTo(string value)
        {
            return value.IndexOf("/", StringComparison.Ordinal) != -1
                ? (EmployeeId) value.Split('/')[1]
                : (EmployeeId) value;
        }
    }
}