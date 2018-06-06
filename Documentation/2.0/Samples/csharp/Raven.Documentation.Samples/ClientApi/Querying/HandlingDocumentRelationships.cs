namespace RavenCodeSamples.ClientApi.Querying
{
	using System.Collections.Generic;
	using System.Linq;

	using Raven.Client;
	using Raven.Client.Indexes;

	public class HandlingDocumentRelationships : CodeSampleBase
	{
		public class User
		{
			public string Id { get; set; }
			public string Name { get; set; }
			public string AliasId { get; set; }
		}

		public class UserAndAlias
		{
			public string UserName { get; set; }
			public string Alias { get; set; }
		}

		public void Includes()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region includes1
					var order = session.Include<Order>(x => x.CustomerId)
						.Load("orders/1234");

					// this will not require querying the server!
					var cust = session.Load<Customer>(order.CustomerId);

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes_1
					var orders = session.Include<Order>(x => x.CustomerId)
						.Load("orders/1234", "orders/4321");

					foreach (var order in orders)
					{
						// this will not require querying the server!
						var cust = session.Load<Customer>(order.CustomerId);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes1_2
					var order = session.Include<Order2, Customer2>(x => x.Customer2Id)
						.Load("orders/1234");

					// this will not require querying the server!
					var cust2 = session.Load<Customer2>(order.Customer2Id);

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes2
					var orders = session.Query<Order>()
						.Customize(x => x.Include<Order>(o => o.CustomerId))
						.Where(x => x.TotalPrice > 100)
						.ToList();

					foreach (var order in orders)
					{
						// this will not require querying the server!
						var cust = session.Load<Customer>(order.CustomerId);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes2_2
					var orders = session.Query<Order2>()
						.Customize(x => x.Include<Order2, Customer2>(o => o.Customer2Id))
						.Where(x => x.TotalPrice > 100)
						.ToList();

					foreach (var order in orders)
					{
						// this will not require querying the server!
						var cust2 = session.Load<Customer2>(order.Customer2Id);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes3
					var order = session.Include<Order>(x => x.SupplierIds)
						.Load("orders/1234");

					foreach (var supplierId in order.SupplierIds)
					{
						// this will not require querying the server!
						var supp = session.Load<Supplier>(supplierId);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes_3
					var orders = session.Include<Order>(x => x.SupplierIds)
						.Load("orders/1234", "orders/4321");

					foreach (var order in orders)
					{
						foreach (var supplierId in order.SupplierIds)
						{
							// this will not require querying the server!
							var supp = session.Load<Supplier>(supplierId);
						}
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes3_2
					var order = session.Include<Order2, Supplier2>(x => x.Supplier2Ids)
						.Load("orders/1234");

					foreach (var supplier2Id in order.Supplier2Ids)
					{
						// this will not require querying the server!
						var supp2 = session.Load<Supplier2>(supplier2Id);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes4
					var order = session.Include<Order>(x => x.Referral.CustomerId)
						.Load("orders/1234");

					// this will not require querying the server!
					var referrer = session.Load<Customer>(order.Referral.CustomerId);

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes_4
					var order = session.Include("Referral.CustomerId")
						.Load<Order>("orders/1234");

					// this will not require querying the server!
					var referrer = session.Load<Customer>(order.Referral.CustomerId);

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes4_2
					var order = session.Include<Order2, Customer2>(x => x.Referral2.Customer2Id)
						.Load("orders/1234");

					// this will not require querying the server!
					var referrer2 = session.Load<Customer2>(order.Referral2.Customer2Id);

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes5
					var order = session.Include<Order>(x => x.LineItems.Select(li => li.ProductId))
						.Load("orders/1234");

					foreach (var lineItem in order.LineItems)
					{
						// this will not require querying the server!
						var product = session.Load<Product>(lineItem.ProductId);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes5_2
					var order = session.Include<Order2, Product2>(x => x.LineItem2s.Select(li => li.Product2Id))
					.Load("orders/1234");

					foreach (var lineItem2 in order.LineItem2s)
					{
						// this will not require querying the server!
						var product2 = session.Load<Product2>(lineItem2.Product2Id);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes6
					var order = session.Include<Order3, Customer2>(x => x.Customer.Id)
						.Load("orders/1234");

					// this will not require querying the server!
					var fullCustomer = session.Load<Customer2>(order.Customer.Id);

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes_7
					var orders = session.Advanced.LuceneQuery<Order2>()
						.Include(x => x.Customer2Id)
						.WhereGreaterThan(x => x.TotalPrice, 100)
						.ToList();

					foreach (var order in orders)
					{
						// this will not require querying the server!
						var cust2 = session.Load<Customer2>(order.Customer2Id);
					}

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region includes_8
					var orders = session.Advanced.LuceneQuery<Order2>()
						.Include("CustomerId")
						.WhereGreaterThan(x => x.TotalPrice, 100)
						.ToList();

					foreach (var order in orders)
					{
						// this will not require querying the server!
						var cust2 = session.Load<Customer2>(order.Customer2Id);
					}

					#endregion
				}

			}
		}

		public void LiveProjections()
		{
			using (var documentStore = NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					#region liveprojections1
					// Storing a sample entity
					var entity = new User { Name = "Ayende" };
					session.Store(entity);
					session.Store(new User { Name = "Oren", AliasId = entity.Id });
					session.SaveChanges();

					// ...
					// ...

					// Get all users, mark AliasId as a field we want to use for Including
					var usersWithAliases = from user in session.Query<User>().Include(x => x.AliasId)
										   where user.AliasId != null
										   select user;

					var results = new List<UserAndAlias>(); // Prepare our results list
					foreach (var user in usersWithAliases)
					{
						// For each user, load its associated alias based on that user Id
						results.Add(new UserAndAlias
						{
							UserName = user.Name,
							Alias = session.Load<User>(user.AliasId).Name
						}
							);
					}

					#endregion
				}

				using (var session = documentStore.OpenSession())
				{
					#region liveprojections3
					var usersWithAliases =
						(from user in session.Query<User, Users_ByAlias>()
						 where user.AliasId != null
						 select user).As<UserAndAlias>();

					#endregion
				}
			}
		}

		#region liveprojections2
		public class Users_ByAlias : AbstractIndexCreationTask<User>
		{
			public Users_ByAlias()
			{
				Map = users => from user in users
							   select new { user.AliasId };

				TransformResults =
					(database, users) => from user in users
										 let alias = database.Load<User>(user.AliasId)
										 select new
													{
														Name = user.Name,
														Alias = alias.Name
													};
			}
		}

		#endregion
	}
}