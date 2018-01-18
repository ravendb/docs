using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations
{

    public class IndexError
    {
        private interface IFoo
        {
            /*
            #region errors_1
            public GetIndexErrorsOperation()

            public GetIndexErrorsOperation(string[] indexNames)
            #endregion
            */
        }

        private class Foo
        {
            #region errors_2
            public class IndexErrors
            {
                public string Name { get; set; }
                public IndexingError[] Errors { get; set; }
            }
            #endregion

            #region errors_3
            public class IndexingError
            {
                public string Error { get; set; }
                public DateTime Timestamp { get; set; }
                public string Document { get; set; }
                public string Action { get; set; }
            }
            #endregion

        }

        public IndexError()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region errors_4
                    // gets errors for all indexes
                    IndexErrors[] indexErrors = store.Maintenance.Send(new GetIndexErrorsOperation());
                    #endregion
                }

                {
                    #region errors_5
                    // gets errors only for 'Orders/Totals' index
                    IndexErrors[] indexErrors = store.Maintenance.Send(new GetIndexErrorsOperation(new[] { "Orders/Totals" }));
                    #endregion
                }

            }
        }
    }
}
