using System.Collections.Generic;

namespace RavenCodeSamples
{
	public class Employee
	{
		public string Name { get; set; }
	}

	public class Company
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<Employee> Employees { get; set; }
		public string Country { get; set; }
	}

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
