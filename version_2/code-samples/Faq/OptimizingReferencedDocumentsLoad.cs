namespace RavenCodeSamples.Faq
{
	using System.Linq;

	public class OptimizingReferencedDocumentsLoad : CodeSampleBase
	{
		private class Order
		{
			public string CustomerId { get; set; }
		}

		private class User
		{
			public string[] Roles { get; set; }
		}

		private class Role
		{
		}

		public void Sample()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					#region optimizing_referenced_documents_load_1
					var order = session.Load<Order>("orders/1");
					var customer = session.Load<Customer>(order.CustomerId);
					// do something that requires both order and customer

					#endregion
				}

				using (var session = documentStore.OpenSession())
				{
					#region optimizing_referenced_documents_load_2
					var order = session
						.Include("CustomerId")
						.Load<Order>("orders/1");

					var customer = session.Load<Customer>(order.CustomerId);
					// do something that requires both order and customer

					#endregion
				}

				using (var session = documentStore.OpenSession())
				{
					#region optimizing_referenced_documents_load_3
					var order = session
					   .Include<Order>(x => x.CustomerId)
					   .Load("orders/1");

					var customer = session.Load<Customer>(order.CustomerId);
					// do something that requires both order and customer

					#endregion
				}

				using (var session = documentStore.OpenSession())
				{
					#region optimizing_referenced_documents_load_4
					var orders = session.Advanced.LuceneQuery<Order>("Orders/Unpaid")
						.Include("CustomerId")
						.ToList();

					#endregion
				}

				using (var session = documentStore.OpenSession())
				{
					#region optimizing_referenced_documents_load_5
					var orders = session.Advanced.LuceneQuery<Order>("Orders/Unpaid")
						.Include(x => x.CustomerId)
						.ToList();

					#endregion
				}

				using (var session = documentStore.OpenSession())
				{
					#region optimizing_referenced_documents_load_6
					var user = session
						.Include<User>(x => x.Roles)
						.Load("users/ayende");

					foreach (var roleId in user.Roles)
					{
						var role = session.Load<Role>(roleId);
						// do something with role document
					}

					#endregion
				}
			}
		}
	}
}