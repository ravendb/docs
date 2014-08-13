namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Indexes
{
	using System.Linq;

	using Raven.Abstractions.Indexing;
	using Raven.Client.Document;
	using Raven.Client.Indexes;

	public class Put
	{
		private interface IFoo
		{
			#region put_1_0
			string PutIndex(string name, IndexDefinition indexDef);

			string PutIndex(string name, IndexDefinition indexDef, bool overwrite);
			#endregion

			#region put_2_0
			string PutIndex<TDocument, TReduceResult>(string name, IndexDefinitionBuilder<TDocument, TReduceResult> indexDef);

			string PutIndex<TDocument, TReduceResult>(string name, IndexDefinitionBuilder<TDocument, TReduceResult> indexDef, bool overwrite);
			#endregion
		}

		private class BlogPost
		{
			public string Title { get; set; }
		}

		public Put()
		{
			using (var store = new DocumentStore())
			{
				#region put_1_1
				var indexName = store
					.DatabaseCommands
					.PutIndex(
						"BlogPosts/ByTitles",
						new IndexDefinition
							{
								Map = "from post in docs.Posts select new { post.Title }"
							});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region put_2_1
				var indexName = store
					.DatabaseCommands
					.PutIndex(
						"BlogPosts/ByTitles",
						new IndexDefinitionBuilder<BlogPost>
							{
								Map = posts => from post in posts select new { post.Title }
							});
				#endregion
			}
		}
	}
}