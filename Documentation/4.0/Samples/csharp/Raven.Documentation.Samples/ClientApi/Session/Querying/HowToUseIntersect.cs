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
                    List<TShirt> tshirts = session
                        .Query<TShirt, TShirts_ByManufacturerColorSizeAndReleaseYear>()
                        .Where(x => x.Manufacturer == "Raven")
                        .Intersect()
                        .Where(x => x.Types.Any(t => t.Color == "Blue" && t.Size == "Small"))
                        .Intersect()
                        .Where(x => x.Types.Any(t => t.Color == "Gray" && t.Size == "Large"))
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region intersect_3
                    // return all T-shirts that are manufactured by 'Raven'
                    // and contain both 'Small Blue' and 'Large Gray' types
                    List<TShirt> tshirts = await asyncSession
                        .Query<TShirt, TShirts_ByManufacturerColorSizeAndReleaseYear>()
                        .Where(x => x.Manufacturer == "Raven")
                        .Intersect()
                        .Where(x => x.Types.Any(t => t.Color == "Blue" && t.Size == "Small"))
                        .Intersect()
                        .Where(x => x.Types.Any(t => t.Color == "Gray" && t.Size == "Large"))
                        .ToListAsync();
                    #endregion
                }
            }
        }
    }
}
