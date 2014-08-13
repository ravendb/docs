using System;
using System.Collections.Generic;

namespace Raven.Documentation.CodeSamples
{
	public class Entities
	{
	}

	public class Camera
	{
		public int Cost { get; set; }
	}

	public class User
	{
		public string Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
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

	#region order_classes_basic
	public class Order
	{
		public string CustomerId { get; set; }
		public string[] SupplierIds { get; set; }
		public Referral Refferal { get; set; }
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

	public class Supplier
	{
		public string Name { get; set; }
		public string Address { get; set; }
	}

	public class Product
	{
		public string Name { get; set; }
		public string[] Images { get; set; }
		public double Price { get; set; }
	}

	#region order_classes2
	public class Order2
	{
		public int Customer2Id { get; set; }
		public Guid[] Supplier2Ids { get; set; }
		public Referral2 Refferal2 { get; set; }
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
	}
	#endregion

	public class Supplier2
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
	}

	public class Product2
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string[] Images { get; set; }
		public double Price { get; set; }
	}

	public class Order3
	{
		public DenormalizedCustomer Customer { get; set; }
		public string[] SupplierIds { get; set; }
		public Referral Refferal { get; set; }
		public LineItem[] LineItems { get; set; }
		public double TotalPrice { get; set; }
	}

	#region order_classes_denormalizedcustomer
	public class DenormalizedCustomer
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
	}
	#endregion
}