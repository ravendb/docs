namespace Raven.Documentation.CodeSamples.ClientApi.Listeners
{
	using Client;

	public class Query
	{
		#region document_query_listener
		public interface IDocumentQueryListener
		{
			/// <summary>
			/// Allow to customize a query globally
			/// </summary>
			void BeforeQueryExecuted(IDocumentQueryCustomization queryCustomization);
		}
		#endregion

		#region document_query_example
		public class DisableCachingQueryListener : IDocumentQueryListener
		{
			public void BeforeQueryExecuted(IDocumentQueryCustomization customization)
			{
				customization.NoCaching();
			}
		}

		#endregion
	}
}