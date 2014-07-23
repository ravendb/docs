using System;
using System.Collections.Generic;

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

		public string AddressId { get; set; }
	}

	public class Address
	{
		public string Id { get; set; }

		public string City { get; set; }

		public string Street { get; set; }
	}

	#region comment
	public class Comment
	{
		public string Author { get; set; }

		public string Message { get; set; }
	}
	#endregion

	public class Company
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<Employee> Employees { get; set; }
		public string Country { get; set; }
		public int NumberOfHappyCustomers { get; set; }
	}

	public class Employee
	{
		public string Name { get; set; }
		public string[] Specialties { get; set; }
		public DateTime HiredAt { get; set; }
		public double HourlyRate { get; set; }
	}
}