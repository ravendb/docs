using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Raven.Abstractions.Indexing;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.Indexes
{
	namespace Foo
	{
		#region term_vectors_3
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

	public class TermVectors
	{
		#region term_vectors_1
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

		public TermVectors()
		{
			using (var store = new DocumentStore())
			{
				#region term_vectors_2
				store
					.DatabaseCommands
					.PutIndex(
						"SampleIndexWithTermVectors",
						new IndexDefinitionBuilder<BlogPost, BlogPost>
						{
							Map = users => from doc in users
										   select new
													   {
														   doc.Tags,
														   doc.Content
													   },
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
	}
}