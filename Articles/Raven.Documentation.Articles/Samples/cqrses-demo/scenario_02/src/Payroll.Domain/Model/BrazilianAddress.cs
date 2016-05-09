namespace Payroll.Domain.Model
{
    public sealed class BrazilianAddress : Address
    {
        public string StreetName { get; private set; }
        public int Number { get; private set; }
        public string AdditionalInfo { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string PostalCode { get; private set; }
        public string Country => "Brazil";

        public override bool Equals(object obj)
        {
            var other = obj as BrazilianAddress;
            if (other == null) return false;

            return
                StreetName == other.StreetName &&
                Number == other.Number &&
                AdditionalInfo == other.AdditionalInfo &&
                Neighborhood == other.Neighborhood &&
                City == other.City &&
                State == other.State &&
                PostalCode == other.PostalCode;
        }

        public override int GetHashCode()
        {
            return
                StreetName.GetHashCode() ^
                Number.GetHashCode() ^
                AdditionalInfo.GetHashCode() ^
                Neighborhood.GetHashCode() ^
                City.GetHashCode() ^
                State.GetHashCode() ^
                PostalCode.GetHashCode() ^
                Country.GetHashCode();
        }

        public override string ToString()
        {
            return $"{StreetName}, {Number} ({City})";
        }

        public static class Factory
        {
            public static BrazilianAddress New(
                string street,
                int number,
                string addtionalInfo,
                string neighborhood,
                string city,
                string state,
                string postalCode
                )
            {
                Throw.IfArgumentIsNullOrEmpty(street, nameof(street));
                Throw.IfArgumentIsNegative(number, nameof(number));
                Throw.IfArgumentIsNullOrEmpty(city, nameof(city));
                Throw.IfArgumentIsNullOrEmpty(state, nameof(state));
                Throw.IfArgumentIsNullOrEmpty(postalCode, nameof(postalCode));

                return new BrazilianAddress
                {
                    StreetName = street,
                    Number = number,
                    AdditionalInfo = addtionalInfo,
                    Neighborhood = neighborhood ?? "",
                    City = city,
                    State = state,
                    PostalCode = postalCode
                };
            }
        }
    }
}