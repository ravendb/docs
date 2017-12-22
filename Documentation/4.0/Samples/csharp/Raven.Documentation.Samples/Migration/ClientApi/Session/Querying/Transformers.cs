using System.Linq;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Migration.ClientApi.Session.Querying
{
    public class Transformers
    {
        /*
        #region transformers_1
        public class Orders_Total : AbstractTransformerCreationTask<Order>
        {
            public class Result
            {
                public double Total { get; set; }
            }

            public Orders_Total()
            {
                TransformResults = orders => 
                    from o in orders
                    select new
                    {
                        Total = o.Lines.Sum(l => l.PricePerUnit * l.Quantity)
                    };
            }
        }
        #endregion
        */

        private class Foo
        {
            public Foo()
            {
                /*
                #region transformers_2
                List<Orders_Total.Result> results = session
                    .Query<Order>()
                    .TransformWith<Orders_Total, Orders_Total.Result>()
                    .ToList();
                #endregion
                */
            }
        }

        public Transformers()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region transformers_3
                    var results = session
                        .Query<Order>()
                        .Select(x => new
                        {
                            Total = x.Lines.Sum(l => l.PricePerUnit * l.Quantity),
                        })
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
