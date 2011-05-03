using System;
using System.Collections.Generic;

namespace RavenCodeSamples
{

	#region company_classes
	public class Employee
	{
		public string Name { get; set; }
		public string[] Specialities { get; set; }
		public DateTime HiredAt { get; set; }
		public double HourlyRate { get; set; }
	}

	public class Company
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<Employee> Employees { get; set; }
		public string Country { get; set; }
		public int NumberOfHappyCustomers { get; set; }
	}
	#endregion

	#region blogpost_classes
	public class BlogPost
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Category { get; set; }
		public string Content { get; set; }
		public BlogComment[] Comments { get; set; }
	}

	public class BlogComment
	{
		public string Title { get; set; }
		public string Content { get; set; }
	}
	#endregion
}
