using Raven.Client.Documents;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{
	public class Querying
	{
		public Querying()
		{
			var store = new DocumentStore()
			{
                Conventions =
                {
                    #region find_prop_name
                    FindPropertyNameForIndex = (indexedType, indexName, path, prop) => 
                                                    (path + prop).Replace(",", "_").Replace(".", "_"),

                    FindPropertyNameForDynamicIndex = (indexedType, indexName, path, prop) => path + prop,
                    #endregion

                    #region throw_if_query_page_is_not_set
                    ThrowIfQueryPageSizeIsNotSet = true
                    #endregion
                }
			};
		} 
	}
}
