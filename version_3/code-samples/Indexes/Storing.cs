using System.Linq;

using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.Indexes
{
	public class Storing
	{
		#region storing_1
		public class StoresIndex : AbstractIndexCreationTask<BlogPost, BlogPost>
		{
			public StoresIndex()
			{
				Map = posts => from doc in posts
							   select new
										  {
											  doc.Tags,
											  doc.Content
										  };

				Stores.Add(x => x.Title, FieldStorage.Yes);

				Indexes.Add(x => x.Tags, FieldIndexing.NotAnalyzed);
				Indexes.Add(x => x.Comments, FieldIndexing.No);
			}
		}
		#endregion
	}
}