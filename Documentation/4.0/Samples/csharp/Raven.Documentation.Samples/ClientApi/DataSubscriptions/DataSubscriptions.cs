using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Raven.Client.Documents.Queries;

namespace Raven.Documentation.Samples.ClientApi.DataSubscriptions
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using Raven.Client.Documents;
	using Raven.Client.Documents.Subscriptions;
	using Raven.Documentation.Samples.Orders;
    
	public class DataSubscriptions
	{

	    #region subscriptions_example
        public async Task Worker(TaskCompletionSource<bool> subscriptionWorkerTcs)
	    {
	        IDocumentStore store = new DocumentStore();
            
	        var subscriptionName = await store.Subscriptions.CreateAsync<Order>(x => x.Company == "companies/11");
	        var subscription = store.Subscriptions.Open<Order>(subscriptionName);
	        var subscriptionTask = subscription.Run(x =>
	            x.Items.ForEach(item =>
	                Console.WriteLine($"Order #{item.Result.Id} will be shipped via: {item.Result.ShipVia}")));
	        _ = subscriptionWorkerTcs.Task.ContinueWith(async x => await subscription.DisposeAsync());
	        await subscriptionTask;

	    }
	    #endregion


	    public async Task CreationExamples()
	    {
	        string id;
	        IDocumentStore store = new DocumentStore();

	        #region create_whole_collection_generic

	        id = await store.Subscriptions.CreateAsync<Order>();

	        #endregion

	        #region create_whole_collection_RQL

	        id = await store.Subscriptions.CreateAsync(new SubscriptionCreationOptions()
	        {
	            Query = "From Orders"
	        });

	        #endregion

	        #region create_filter_only_generic

	        id = await store.Subscriptions.CreateAsync<Order>(x =>
	            x.Lines.Sum(line => line.PricePerUnit * line.Quantity) > 100);

	        #endregion

	        #region create_filter_only_RQL

	        id = await store.Subscriptions.CreateAsync(new SubscriptionCreationOptions()
	        {
	            Query = @"
declare function getOrderLinesSum(doc){
    var sum = 0;
    for (var i in doc.Lines) { sum += doc.Lines[i];}
    return sum;
}
From Orders as o 
Where getOrderLinesSum(o) > 100"
	        });

	        #endregion

	        #region create_filter_and_projection_generic

	        id = store.Subscriptions.Create(
	            new SubscriptionCreationOptions<Order>()
	            {
	                Filter = x => x.Lines.Sum(line => line.PricePerUnit * line.Quantity) > 100,
	                Project = x => new
	                {
	                    Id = x.Id,
	                    Total = x.Lines.Sum(line => line.PricePerUnit * line.Quantity),
	                    ShipTo = x.ShipTo,
	                    EmployeeName = RavenQuery.Load<Employee>(x.Employee).FirstName + " " +
	                                   RavenQuery.Load<Employee>(x.Employee).LastName
	                }
	            });

	        #endregion

	        #region create_filter_and_projection_RQL

	        id = await store.Subscriptions.CreateAsync(new SubscriptionCreationOptions()
	        {
	            Query = @"
declare function getOrderLinesSum(doc){
    var sum = 0;
    for (var i in doc.Lines) { sum += doc.Lines[i];}
    return sum;
}

declare function projectOrder(doc){
    var employee = LoadDocument(doc.Employee);
    return {
        Id: order.Id,
        Total: getOrderLinesSum(order),
        ShipTo: order.ShipTo,
        EmployeeName: employee.FirstName + ' ' + employee.LastName

    };
}

From Orders as o 
Where getOrderLinesSum(o) > 100
Select projectOrder(o)"
	        });

	        #endregion

	        #region create_versioned_subscription_generic

	        id = store.Subscriptions.Create(
	            new SubscriptionCreationOptions<Revision<Order>>());

	        #endregion

	        #region create_filter_and_projection_RQL

	        id = await store.Subscriptions.CreateAsync(new SubscriptionCreationOptions()
	        {
	            Query = @"From Orders (Revisions = true)"
	        });

	        #endregion
	    }


	    public DataSubscriptions()
		{
			IDocumentStore store = new DocumentStore();
       
			//{

			//	#region open_2
			//	var orders = store.Subscriptions.Open<Order>(id, new SubscriptionConnectionOptions()
			//	{
			//		BatchOptions = new SubscriptionBatchOptions()
			//		{
			//			MaxDocCount = 16*1024,
			//			MaxSize = 4*1024*1024,
			//			AcknowledgmentTimeout = TimeSpan.FromMinutes(3)
			//		},
			//		IgnoreSubscribersErrors = false,
			//		ClientAliveNotificationInterval = TimeSpan.FromSeconds(30)
			//	});
			//	#endregion

			//	#region open_3
			//	orders.Subscribe(x =>
			//	{
			//		GenerateInvoice(x);
			//	});

			//	orders.Subscribe(x =>
			//	{
			//		if(x.RequireAt > DateTime.Now)
			//			SendReminder(x.Employee, x.Id);
			//	});
			//	#endregion

			//	#region open_4
			//	var subscriber = orders.Subscribe(x => { });

			//	subscriber.Dispose();
			//	#endregion

			//	#region delete_2
			//	store.Subscriptions.Delete(id);
			//	#endregion

			//	#region get_subscriptions_2
			//	var configs = store.Subscriptions.GetSubscriptions(0, 10);
			//	#endregion

			//	#region release_2
			//	store.Subscriptions.Release(id);
			//	#endregion
			//}
		}

		private void SendReminder(string employee, string id)
		{
		}

		public void GenerateInvoice(Order o)
		{
			
		}

		//private interface IFoo
		//{

		//	#region open_1
		//	Subscription<RavenJObject> Open(long id, SubscriptionConnectionOptions options, string database = null);

		//	Subscription<T> Open<T>(long id, SubscriptionConnectionOptions options, string database = null) 
		//	#endregion
		//		 where T : class;

		//	#region delete_1
		//	void Delete(long id, string database = null);
		//	#endregion

		//	#region get_subscriptions_1
		//	List<SubscriptionConfig> GetSubscriptions(int start, int take, string database = null);
		//	#endregion

		//	#region release_1
		//	void Release(long id, string database = null);
		//	#endregion
		//}

		//#region events
		//public delegate void BeforeBatch();

		//public delegate bool BeforeAcknowledgment();

		//public delegate void AfterAcknowledgment(Etag lastProcessedEtag);

		//public delegate void AfterBatch(int documentsProcessed);
		//#endregion
	}
}
