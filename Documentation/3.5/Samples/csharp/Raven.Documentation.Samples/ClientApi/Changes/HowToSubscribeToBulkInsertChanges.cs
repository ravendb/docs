using System;

using Raven.Abstractions.Data;
using Raven.Client.Changes;
using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Changes
{
	public class HowToSubscribeToBulkInsertChanges
	{
		private interface IFoo
		{
			#region bulk_insert_changes_1
			IObservableWithTask<BulkInsertChangeNotification>
				ForBulkInsert(Guid operationId);
			#endregion
		}

		public HowToSubscribeToBulkInsertChanges()
		{
			using (var store = new DocumentStore())
			{
				#region bulk_insert_changes_2
				using (BulkInsertOperation bulkInsert = store.BulkInsert())
				{
					IDisposable subscription = store
						.Changes()
						.ForBulkInsert(bulkInsert.OperationId)
						.Subscribe(change =>
						{
							switch (change.Type)
							{
								case DocumentChangeTypes.BulkInsertStarted:
									// do something
									break;
								case DocumentChangeTypes.BulkInsertEnded:
									// do something
									break;
								case DocumentChangeTypes.BulkInsertError:
									// do something
									break;
							}
						});

					try
					{
						for (int i = 0; i < 1000 * 1000; i++)
						{
							bulkInsert.Store(new Employee
							{
								FirstName = "FirstName #" + i,
								LastName = "LastName #" + i
							});
						}
					}
					finally
					{
						if (subscription != null)
							subscription.Dispose();
					}
				}
				#endregion
			}
		}
	}
}
