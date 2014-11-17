namespace Raven.Documentation.Samples.Studio.Overview.Query
{
	using System.Linq;
	using Raven.Client;
	using Raven.Client.Document;


	public class ReportingView
	{

		public class Order
		{
			public double Total { get; set; }

			public string Employee { get; set; }

			public string Company { get; set; }
		}

		public ReportingView()
		{
			var store = new DocumentStore();

			using (var session = store.OpenSession())
			{
				#region sample_csharp
				session.Query<Order>("Orders/Total")
					.Where(x => x.Company == "companies/1")
					.AggregateBy(x => x.Employee)
					.SumOn(x => x.Total)
					.ToList();
				#endregion
			}
			
		} 
	}
}