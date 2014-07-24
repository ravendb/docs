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

	public class BlogPost
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Category { get; set; }
		public string Content { get; set; }
		public DateTime PublishedAt { get; set; }
		public string[] Tags { get; set; }
		public BlogComment[] Comments { get; set; }
	}

	public class BlogComment
	{
		public string Title { get; set; }
		public string Content { get; set; }
	}
}