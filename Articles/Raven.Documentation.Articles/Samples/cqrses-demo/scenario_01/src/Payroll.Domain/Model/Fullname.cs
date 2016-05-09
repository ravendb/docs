namespace Payroll.Domain
{
    public sealed class FullName
    {
        public string GivenName { get;  }
        public string Surname { get; }
        public FullName(
            string givenName,
            string surname
            )
        {
            Throw.IfArgumentIsNullOrEmpty(givenName, nameof(givenName));
            Throw.IfArgumentIsNullOrEmpty(surname, nameof(surname));
            GivenName = givenName;
            Surname = surname;
        }

        public override string ToString()
        {
            return $"{GivenName} {Surname}";
        }

        public override bool Equals(object obj)
        {
            var other = obj as FullName;
            if (other == null) return false;

            return GivenName == other.GivenName && Surname == other.Surname;
        }

        public override int GetHashCode()
        {
            return GivenName.GetHashCode() ^ Surname.GetHashCode();
        }
    }
}