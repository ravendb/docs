using System;
using System.Collections.Generic;

namespace RavenCodeSamples
{

	#region company_classes
	public class Employee
	{
		public string Name { get; set; }
		public string[] Specialties { get; set; }
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
		public DateTime PublishedAt { get; set; }
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

	public class MyIndexClass
	{
	}


	#region order_classes_basic

	public class Order
	{
		public string CustomerId { get; set; }
		public string[] SupplierIds { get; set; }
		public Referral Referral { get; set; }
		public LineItem[] LineItems { get; set; }
		public double TotalPrice { get; set; }
	}

	public class Customer
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public short Age { get; set; }
		public string HashedPassword { get; set; }
	}

	#endregion

	#region order_classes_denormalizedcustomer

	public class DenormalizedCustomer
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
	}

	#endregion

	#region order_classes_ordertodenormalizedcustomer

	public class Order3
	{
		public DenormalizedCustomer Customer { get; set; }
		public string[] SupplierIds { get; set; }
		public Referral Referral { get; set; }
		public LineItem[] LineItems { get; set; }
		public double TotalPrice { get; set; }
	}

	#endregion

	#region order_classes_supplier

	public class Supplier
	{
		public string Name { get; set; }
		public string Address { get; set; }
	}

	#endregion

	#region order_classes_referral

	public class Referral
	{
		public string CustomerId { get; set; }
		public double CommissionPercentage { get; set; }
	}

	#endregion

	#region order_classes_lineitem

	public class LineItem
	{
		public string ProductId { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
	}

	#endregion

	#region order_classes_product

	public class Product
	{
		public string Name { get; set; }
		public string[] Images { get; set; }
		public double Price { get; set; }
	}

	#endregion

	#region order_classes2

	public class Order2
	{
		public int Customer2Id { get; set; }
		public Guid[] Supplier2Ids { get; set; }
		public Referral2 Referral2 { get; set; }
		public LineItem2[] LineItem2s { get; set; }
		public double TotalPrice { get; set; }
	}

	public class Customer2
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public short Age { get; set; }
		public string HashedPassword { get; set; }
	}

	public class Referral2
	{
		public int Customer2Id { get; set; }
		public double CommissionPercentage { get; set; }
	}

	public class LineItem2
	{
		public Guid Product2Id { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		public double Price { get; set; }
	}

	#endregion

	public class Product2
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string[] Images { get; set; }
		public double Price { get; set; }
	}

	public class Supplier2
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
	}

}
