namespace Payroll.Domain.Model
{
    public sealed class EmployeeId
    {
        private readonly string _value;

        private EmployeeId(string value)
        {
            Throw.IfArgumentIsNullOrEmpty(value, nameof(value));
            _value = value;
        }

        public static implicit operator string(EmployeeId source)
        {
            return source._value;
        }

        public static implicit operator EmployeeId(string source)
        {
            return new EmployeeId(source);
        }

        public override string ToString()
        {
            return _value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as EmployeeId;
            if (other == null) return false;
            return (other._value == _value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
