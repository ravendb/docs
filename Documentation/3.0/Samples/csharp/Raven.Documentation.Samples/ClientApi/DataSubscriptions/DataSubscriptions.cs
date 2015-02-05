namespace Raven.Documentation.Samples.ClientApi.DataSubscriptions
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using Abstractions.Data;
	using Client;
	using Client.Document;
	using CodeSamples.Orders;
	using Json.Linq;

	public class DataSubscriptions
	{
		public DataSubscriptions()
		{
			IDocumentStore store = new DocumentStore();

			#region accessing_subscriptions
			IReliableSubscriptions subscriptions = store.Subscriptions;

			IAsyncReliableSubscriptions asyncSubscriptions = store.AsyncSubscriptions;
			#endregion

			{
				#region create_2
				var id = store.Subscriptions.Create(new SubscriptionCriteria
				{
					KeyStartsWith = "employees/",
					PropertiesNotMatch = new Dictionary<string, RavenJToken>()
					{
						{ "Address.City", "Seattle" }
					}
				});
				#endregion
			}

			{
				#region create_3
				var id = store.Subscriptions.Create(new SubscriptionCriteria<Order>()
				{
					PropertiesMatch = new Dictionary<Expression<Func<Order, object>>, RavenJToken>()
					{
						{ x => x.Employee, "employees/1"}
					}
				});
				#endregion

				#region open_2
				var orders = store.Subscriptions.Open<Order>(id, new SubscriptionConnectionOptions()
				{
					BatchOptions = new SubscriptionBatchOptions()
					{
						MaxDocCount = 16*1024,
						MaxSize = 4*1024*1024,
						AcknowledgmentTimeout = TimeSpan.FromMinutes(3)
					},
					IgnoreSubscribersErrors = false,
					ClientAliveNotificationInterval = TimeSpan.FromSeconds(30)
				});
				#endregion

				#region open_3
				orders.Subscribe(x =>
				{
					GenerateInvoice(x);
				});

				orders.Subscribe(x =>
				{
					if(x.RequireAt > DateTime.Now)
						SendReminder(x.Employee, x.Id);
				});
				#endregion

				#region delete_2
				store.Subscriptions.Delete(id);
				#endregion

				#region get_subscriptions_2
				var configs = store.Subscriptions.GetSubscriptions(0, 10);
				#endregion

				#region release_2
				store.Subscriptions.Release(id);
				#endregion
			}
		}

		private void SendReminder(string employee, string id)
		{
		}

		public void GenerateInvoice(Order o)
		{
			
		}

		private interface IFoo
		{
			#region create_1
			long Create(SubscriptionCriteria criteria, string database = null);

			long Create<T>(SubscriptionCriteria<T> criteria, string database = null);
			#endregion

			#region open_1
			Subscription<RavenJObject> Open(long id, SubscriptionConnectionOptions options, string database = null);

			Subscription<T> Open<T>(long id, SubscriptionConnectionOptions options, string database = null) 
			#endregion
				 where T : class;

			#region delete_1
			void Delete(long id, string database = null);
			#endregion

			#region get_subscriptions_1
			List<SubscriptionConfig> GetSubscriptions(int start, int take, string database = null);
			#endregion

			#region release_1
			void Release(long id, string database = null);
			#endregion
		}
		
	}
}