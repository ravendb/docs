using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Search;
using Raven.Abstractions.Commands;
using Raven.Abstractions.Data;

namespace RavenCodeSamples.Consumer
{
	public class SetBased : CodeSampleBase
	{
		public void Simple()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region setbased1
				documentStore.DatabaseCommands.DeleteByIndex("IndexName",
				                                             new IndexQuery
				                                             	{
				                                             		Query = "Title:RavenDB" // where entity.Title contains RavenDB
				                                             	}, allowStale: false);
				#endregion

				#region setbased2

				documentStore.DatabaseCommands.UpdateByIndex("IndexName",
				                                             new IndexQuery {Query = "Title:RavenDB"},
				                                             new[]
				                                             	{
				                                             		new PatchRequest
				                                             			{
				                                             				Type = PatchCommandType.Add,
				                                             				Name = "Comments",
				                                             				Value = "New automatic comment we added programmatically"
				                                             			}
				                                             	}, allowStale: false);

				#endregion
			}
		}
	}
}
