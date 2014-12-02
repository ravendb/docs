using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RavenCodeSamples.ClientApi.Querying.StaticIndexes
{
	using System.Linq;

	using Raven.Abstractions.Indexing;
	using Raven.Client.Indexes;

	namespace Foo
	{
		#region configuring_index_options_3
		public enum FieldTermVector
		{
			/// <summary>
			/// Do not store term vectors
			/// </summary>
			No,
			/// <summary>
			/// Store the term vectors of each document. A term vector is a list of the document's
			/// terms and their number of occurrences in that document.
			/// </summary>
			Yes,
			/// <summary>
			/// Store the term vector + token position information
			/// </summary>
			WithPositions,
			/// <summary>
			/// Store the term vector + Token offset information
			/// </summary>
			WithOffsets,
			/// <summary>
			/// Store the term vector + Token position and offset information
			/// </summary>
			WithPositionsAndOffsets
		}

		#endregion
	}

	#region configuring_index_options_1
	public class SampleIndexWithTermVectors : AbstractIndexCreationTask<BlogPost, BlogPost>
	{
		public SampleIndexWithTermVectors()
		{
			Map = users => from doc in users
						   select new
							   {
								   doc.Tags,
								   doc.Content
							   };

			Indexes = new Dictionary<Expression<Func<BlogPost, object>>, FieldIndexing>
				{
					{ x => x.Content, FieldIndexing.Analyzed }
				};

			TermVectors = new Dictionary<Expression<Func<BlogPost, object>>, FieldTermVector>
				{
					{ x => x.Content, FieldTermVector.WithPositionsAndOffsets }
				};
		}
	}

	#endregion

	public class ConfiguringIndexOptions : CodeSampleBase
	{
		public void BasicStaticIndexes()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				#region analyzers1
				documentStore.DatabaseCommands.PutIndex("AnalyzersTestIdx",
					new IndexDefinitionBuilder<BlogPost, BlogPost>
					{
						Map =
							users =>
							from doc in users select new { doc.Tags, doc.Content },
						Analyzers =
				            {
				                {x => x.Tags, "SimpleAnalyzer"},
				                {x => x.Content, "SnowballAnalyzer"}
				            },
					});

				#endregion

				#region configuring_index_options_2
				documentStore.DatabaseCommands.PutIndex("SampleIndexWithTermVectors",
					new IndexDefinitionBuilder<BlogPost, BlogPost>
					{
						Map =
							users =>
							from doc in users select new { doc.Tags, doc.Content },
						Indexes =
							{
								{ x => x.Content, FieldIndexing.Analyzed }
							},
						TermVectors =
							{
								{ x => x.Content, FieldTermVector.WithPositionsAndOffsets }
							}
					});

				#endregion
			}
		}

		#region stores1
		public class StoresIndex : AbstractIndexCreationTask<BlogPost, BlogPost>
		{
			public StoresIndex()
			{
				Map = posts => from doc in posts
							   select new { doc.Tags, doc.Content };

				Stores.Add(x => x.Title, FieldStorage.Yes);

				Indexes.Add(x => x.Tags, FieldIndexing.NotAnalyzed);
				Indexes.Add(x => x.Comments, FieldIndexing.No);
			}
		}
		#endregion
	}
}
