namespace Payroll.Domain.Model
{
    public abstract partial class Address
    {
        class NullAddress : Address
        {
            public override string Country => "Untold.";

            public override string ToString()
            {
                return "Untold";
            }
        }

        public static Address NotInformed => new NullAddress();
    }


}