using Raven.Abstractions.Indexing;

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Indexes.HowTo
{
	public class IndexHasChanged
	{
		private interface IFoo
		{
			#region index_has_changed_1
			bool IndexHasChanged(string name, IndexDefinition indexDef);
			#endregion
		} 
	}
}