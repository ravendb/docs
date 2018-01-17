using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Indexes
{
	public class Get
	{
		private interface IFoo
		{
            /*
            #region get_1_0
            public GetIndexOperation(string indexName)
            #endregion
            */

            /*
            #region get_2_0
            public GetIndexesOperation(int start, int pageSize)
            #endregion
            */

            /* 
            #region get_3_0
            public GetIndexNamesOperation(int start, int pageSize);
            #endregion
            */
        }

		public Get()
		{
			using (var store = new DocumentStore())
			{
                #region get_1_1
                IndexDefinition index = store.Maintenance.Send(new GetIndexOperation("Orders/Totals"));
                #endregion

                #region get_2_1
                IndexDefinition[] indexes = store.Maintenance.Send(new GetIndexesOperation(0, 10));
                #endregion

                #region get_3_1
                string[] indexNames = store.Maintenance.Send(new GetIndexNamesOperation(0, 10));
				#endregion
			}
		}
	}
}
