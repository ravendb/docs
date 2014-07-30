using System;

using Raven.Abstractions.Data;
using Raven.Client.Changes;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Changes
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
				using (var bulkInsert = store.BulkInsert())
				{
					var subscribtion = store
						.Changes()
						.ForBulkInsert(bulkInsert.OperationId)
						.Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));

					try
					{
						for (int i = 0; i < 1000 * 1000; i++)
						{
							bulkInsert.Store(new Person
							{
								FirstName = "FirstName #" + i,
								LastName = "LastName #" + i
							});
						}
					}
					finally
					{
						if (subscribtion != null)
							subscribtion.Dispose();
					}
				}
				#endregion
			}
		}
	}
}