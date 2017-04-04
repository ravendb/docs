using System.Linq;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.UsersIssues
{
    class UnderstandingEventualConsistency
    {
        class Product
        {
            public string Name;
        }


        public void Sample1()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    var pageSize = 0;
                    var pageNumber = 0;

                    #region userissues_1
                    var results = session.Query<Product>()
                        .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
                        .OrderBy(x => x.Name)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);
                    #endregion
                }
            }
        }

        public void Sample2()
        {
            var productId = "Products/1";
            var pageSize = 0;
            var pageNumber = 0;
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
        #region userissues_2
                    var product = session.Load<Product>(productId);
                    var etag = session.Advanced.GetEtagFor(product);

                    var results = session.Query<Product>()
                        .Customize(x => x.WaitForNonStaleResultsAsOf(etag))
                        .OrderBy(x => x.Name)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);
        #endregion
                }
            }
        }

    }
}
