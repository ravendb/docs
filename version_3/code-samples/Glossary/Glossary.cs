namespace Raven.Documentation.CodeSamples.Glossary
{
	public class Glossary
	{
		/*
		#region bulk_insert_operation
		public class BulkInsertOperation
		{
			public delegate void BeforeEntityInsert(
				string id,
				RavenJObject data,
				RavenJObject metadata);

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

		#region bulk_insert_options
		public class BulkInsertOptions
		{
			public bool OverwriteExisting { get; set; }

			public bool CheckReferencesInIndexes { get; set; }

			public int BatchSize { get; set; }

			public int WriteTimeoutMilliseconds { get; set; }
		}
		#endregion
	}
}