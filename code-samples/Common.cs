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
		public string[] Tags { get; set; }
		public BlogComment[] Comments { get; set; }
	}

	public class BlogComment
	{
		public string Title { get; set; }
		public string Content { get; set; }
	}
	#endregion

	public class BlogAuthor
	{
		public string Name { get; set; }
		public string ImageUrl { get; set; }
	}

	#region blogpost_mapreduce_classes
	public class BlogTagPostsCount
	{
		public string Tag { get; set; }
		public int Count { get; set; }
	}
	#endregion

	#region order_classes
	public class Order
	{
		public Product[] Items { get; set; }
		public string CustomerId { get; set; }
		public double TotalPrice { get; set; }
	}

	public class Product
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string[] Images { get; set; }
		public double Price { get; set; }
	}

	public class Customer
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public short Age { get; set; }
		public string HashedPassword { get; set; }
	}
	#endregion
}
