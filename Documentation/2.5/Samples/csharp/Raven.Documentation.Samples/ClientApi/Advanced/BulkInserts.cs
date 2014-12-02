using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace RavenCodeSamples.ClientApi.Advanced
{
	namespace Foo
	{
		#region bulk_inserts_2
		public interface IDocumentStore
		{
			BulkInsertOperation BulkInsert(string database = null, BulkInsertOptions options = null);
		}

		#endregion

		#region bulk_inserts_3
		public class BulkInsertOptions
		{
			public bool CheckForUpdates { get; set; }

			public bool CheckReferencesInIndexes { get; set; }

			public int BatchSize { get; set; }
		}

		#endregion

		/*
		#region bulk_inserts_4
		public class BulkInsertOperation
		{
		    public delegate void BeforeEntityInsert(string id, RavenJObject data, RavenJObject metadata);
		    
			public event BeforeEntityInsert OnBeforeEntityInsert = delegate { };

			public event Action<string> Report { ... }

			public void Store(object entity) { ... }

			public void Store(object entity, string id) { ... }
		}
		
		#endregion
		 */
	}

	public class BulkInserts : CodeSampleBase
	{
		private class User
		{
			public string Name { get; set; }
		}

		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				#region bulk_inserts_1
				using (var bulkInsert = store.BulkInsert())
				{
					for (int i = 0; i < 1000 * 1000; i++)
					{
						bulkInsert.Store(new User
							{
								Name = "Users #" + i
							});
					}
				}

				#endregion
			}
		}
	}
}