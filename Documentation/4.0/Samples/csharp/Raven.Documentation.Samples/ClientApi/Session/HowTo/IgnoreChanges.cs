using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class IgnoreChanges
    {
        private interface IFoo
        {
            #region ignore_changes_1
            void IgnoreChangesFor(object entity);
            #endregion
        }

        public IgnoreChanges()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region ignore_changes_2
                    Product product = session.Load<Product>("products/1-A");
                    session.Advanced.IgnoreChangesFor(product);
                    product.UnitsInStock += 1; //this will be ignored for SaveChanges
                    session.SaveChanges();
                    #endregion

                }

            }
        }
    }
}
