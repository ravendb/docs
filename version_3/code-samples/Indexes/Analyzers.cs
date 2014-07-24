using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.Indexes
{
	public class Analyzers
	{
		public Analyzers()
		{
			using (var store = new DocumentStore())
			{
				#region analyzers_1
				store
					.DatabaseCommands
					.PutIndex(
						"AnalyzersTestIdx",
						new IndexDefinitionBuilder<BlogPost, BlogPost>
						{
							Map = users => from doc in users
										   select new
													  {
														  doc.Tags,
														  doc.Content
													  },
							Analyzers =
								{
									{ x => x.Tags, "SimpleAnalyzer" }, 
									{ x => x.Content, "SnowballAnalyzer" }
								},
						});
				#endregion
			}
		}
	}
}