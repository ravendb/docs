using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Indexes.Querying;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToUseIntersect
    {
        private interface IFoo
        {
            #region intersect_1
            IRavenQueryable<T> Intersect<T>();
            #endregion
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region intersect_2
                    // return all T-shirts that are manufactured by 'Raven'
                    // and contain both 'Small Blue' and 'Large Gray' types
                    IList<TShirt> tshirts = session.Query<TShirts_ByManufacturerColorSizeAndReleaseYear.Result, TShirts_ByManufacturerColorSizeAndReleaseYear>()
                        .Where(x => x.Manufacturer == "Raven")
                        .Intersect()
                        .Where(x => x.Color == "Blue" && x.Size == "Small")
                        .Intersect()
                        .Where(x => x.Color == "Gray" && x.Size == "Large")
                        .OfType<TShirt>()
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region intersect_3
                    // return all T-shirts that are manufactured by 'Raven'
                    // and contain both 'Small Blue' and 'Large Gray' types
                    IList<TShirt> tshirts = await asyncSession.Query<TShirts_ByManufacturerColorSizeAndReleaseYear.Result, TShirts_ByManufacturerColorSizeAndReleaseYear>()
                        .Where(x => x.Manufacturer == "Raven")
                        .Intersect()
                        .Where(x => x.Color == "Blue" && x.Size == "Small")
                        .Intersect()
                        .Where(x => x.Color == "Gray" && x.Size == "Large")
                        .OfType<TShirt>()
                        .ToListAsync();
                    #endregion
                }
            }
        }
    }
}
