// -----------------------------------------------------------------------
//  <copyright file="QueryStreaming.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System;
using Raven.Abstractions.Data;
using Raven.Client.Linq;

namespace RavenCodeSamples.ClientApi.Advanced
{
	public class UnboundedResults : CodeSampleBase
	{
		class User
		{
			public bool Active { get; set; }
		}

		public void QueryStreamingByUsingSessionAdvancedStream()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region query_streaming_1
					var query = session.Query<User>("Users/ByActive").Where(x => x.Active);

					using (var enumerator = session.Advanced.Stream(query))
					{
						while (enumerator.MoveNext())
						{
							User activeUser = enumerator.Current.Document;
						}
					}
					#endregion

					#region query_streaming_2
					var luceneQuery = session.Advanced.LuceneQuery<User>("Users/ByActive").Where("Active:true");

					using (var enumerator = session.Advanced.Stream(luceneQuery))
					{
						while (enumerator.MoveNext())
						{
							User activeUser = enumerator.Current.Document;
						}
					}
					#endregion

					#region query_streaming_3
					QueryHeaderInformation queryHeaderInformation;
					session.Advanced.Stream(query, out queryHeaderInformation);
					#endregion
				}
			}
		}

		public void DocumentStreamingByUsingSessionAdvancedStream()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region doc_streaming_1
					using (var enumerator = session.Advanced.Stream<User>(fromEtag: Etag.Empty,
					                                                      start: 0, pageSize: int.MaxValue))
					{
						while (enumerator.MoveNext())
						{
							User activeUser = enumerator.Current.Document;
						}
					}
					#endregion

					#region doc_streaming_2

					using (var enumerator = session.Advanced.Stream<User>(startsWith: "users/",
					                                                      matches: "*Ra?en",
					                                                      start: 0, pageSize: int.MaxValue))
					{
						while (enumerator.MoveNext())
						{
							User activeUser = enumerator.Current.Document;
						}
					}

					#endregion
				}
			}
		}
	}
}

namespace QueryHeaderSample
{
	#region query_streaming_4
	public class QueryHeaderInformation
	{
		public string Index { get; set; }
		public bool IsStable { get; set; }
		public DateTime IndexTimestamp { get; set; }
		public int TotalResults { get; set; }
		public Etag ResultEtag { get; set; }
		public Etag IndexEtag { get; set; }
	}
	#endregion
}