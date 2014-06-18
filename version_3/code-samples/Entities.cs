namespace Raven.Documentation.CodeSamples
{
	public class Entities
	{
	}

	public class Person
	{
		public string Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Address { get; set; }
	}

	public class Address
	{
		public string Id { get; set; }

		public string City { get; set; }

		public string Street { get; set; }
	}

	public class Comment
	{
		public string Author { get; set; }

		public string Message { get; set; }
	}
}