using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.BulkInsert
{
	#region bulk_inserts_2
	public class BulkInsertOptions
	{
		public bool OverwriteExisting { get; set; }

		public bool CheckReferencesInIndexes { get; set; }

		public int BatchSize { get; set; }

		public int WriteTimeoutMilliseconds { get; set; }
	}
	#endregion

	/*
	#region bulk_inserts_3
	public class BulkInsertOperation
	{
		public delegate void BeforeEntityInsert(string id, RavenJObject data, RavenJObject metadata);

		public event BeforeEntityInsert OnBeforeEntityInsert = delegate { };
		
		public bool IsAborted { get { ... } }
		
		public void Abort() { ... }
		
		public Guid OperationId { get { ... } }
		
		public event Action<string> Report { ... }
		
		public void Store(object entity) { ... }
		
		public void Store(object entity, string id) { ... }
		
		public void Dispose() { ... }
	}
		
	#endregion
	*/

	public class BulkInserts
	{
		private interface IFoo
		{
			#region bulk_inserts_1
			BulkInsertOperation BulkInsert(
				string database = null,
				BulkInsertOptions options = null);
			#endregion
		}

		public BulkInserts()
		{
			using (var store = new DocumentStore())
			{
				#region bulk_inserts_4
				using (var bulkInsert = store.BulkInsert())
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
				#endregion
			}
		}
	}
}