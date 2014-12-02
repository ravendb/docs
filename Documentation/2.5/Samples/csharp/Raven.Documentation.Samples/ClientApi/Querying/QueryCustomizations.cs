// -----------------------------------------------------------------------
//  <copyright file="QueryCustomizations.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
namespace RavenCodeSamples.ClientApi.Querying
{
	public class QueryCustomizations : CodeSampleBase
	{
		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region customize_usage
					session.Query<User>().Customize(x => x.WaitForNonStaleResultsAsOfLastWrite());
					#endregion

					#region customize_usage_lucene
					session.Advanced.LuceneQuery<User>().WaitForNonStaleResultsAsOfLastWrite();
					#endregion

					#region no_cache
					session.Query<User>().Customize(x => x.NoCaching());
					#endregion

					#region no_tracking
					session.Query<User>().Customize(x => x.NoTracking());
					#endregion
				}
			}
			
		}
	}
}