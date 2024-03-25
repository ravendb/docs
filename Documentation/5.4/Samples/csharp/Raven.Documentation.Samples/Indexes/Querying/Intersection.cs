using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    #region intersection_1
    public class TShirt
    {
        public string Id { get; set; }

        public int ReleaseYear { get; set; }

        public string Manufacturer { get; set; }

        public List<TShirtType> Types { get; set; }
    }

    public class TShirtType
    {
        public string Color { get; set; }

        public string Size { get; set; }
    }
    #endregion

    #region intersection_2
    public class TShirts_ByManufacturerColorSizeAndReleaseYear : AbstractIndexCreationTask<TShirt>
    {
        public class Result
        {
            public string Manufacturer { get; set; }

            public string Color { get; set; }

            public string Size { get; set; }

            public int ReleaseYear { get; set; }
        }

        public TShirts_ByManufacturerColorSizeAndReleaseYear()
        {
            Map = tshirts => from tshirt in tshirts
                             from type in tshirt.Types
                             select new
                             {
                                 Manufacturer = tshirt.Manufacturer,
                                 Color = type.Color,
                                 Size = type.Size,
                                 ReleaseYear = tshirt.ReleaseYear
                             };
        }
    }
    #endregion

    public class Intersection
    {
        public void Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region intersection_3
                    session.Store(new TShirt
                    {
                        Id = "tshirts/1",
                        Manufacturer = "Raven",
                        ReleaseYear = 2010,
                        Types = new List<TShirtType>
                                {
                                    new TShirtType {Color = "Blue", Size = "Small"},
                                    new TShirtType {Color = "Black", Size = "Small"},
                                    new TShirtType {Color = "Black", Size = "Medium"},
                                    new TShirtType {Color = "Gray", Size = "Large"}
                                }
                    });

                    session.Store(new TShirt
                    {
                        Id = "tshirts/2",
                        Manufacturer = "Wolf",
                        ReleaseYear = 2011,
                        Types = new List<TShirtType>
                                {
                                    new TShirtType { Color = "Blue",  Size = "Small" },
                                    new TShirtType { Color = "Black", Size = "Large" },
                                    new TShirtType { Color = "Gray",  Size = "Medium" }
                                }
                    });

                    session.Store(new TShirt
                    {
                        Id = "tshirts/3",
                        Manufacturer = "Raven",
                        ReleaseYear = 2011,
                        Types = new List<TShirtType>
                                {
                                    new TShirtType { Color = "Yellow",  Size = "Small" },
                                    new TShirtType { Color = "Gray",  Size = "Large" }
                                }
                    });

                    session.Store(new TShirt
                    {
                        Id = "tshirts/4",
                        Manufacturer = "Raven",
                        ReleaseYear = 2012,
                        Types = new List<TShirtType>
                                {
                                    new TShirtType { Color = "Blue",  Size = "Small" },
                                    new TShirtType { Color = "Gray",  Size = "Large" }
                                }
                    });
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region intersection_4
                    IList<TShirt> results = session.Query<TShirts_ByManufacturerColorSizeAndReleaseYear.Result, TShirts_ByManufacturerColorSizeAndReleaseYear>()
                           .Where(x => x.Manufacturer == "Raven")
                           .Intersect()
                           .Where(x => x.Color == "Blue" && x.Size == "Small")
                           .Intersect()
                           .Where(x => x.Color == "Gray" && x.Size == "Large")
                           .OfType<TShirt>()
                           .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region intersection_5
                    IList<TShirt> results = session
                        .Advanced
                        .DocumentQuery<TShirt, TShirts_ByManufacturerColorSizeAndReleaseYear>()
                        .WhereEquals("Manufacturer", "Raven")
                        .Intersect()
                        .WhereEquals("Color", "Blue")
                        .AndAlso()
                        .WhereEquals("Size", "Small")
                        .Intersect()
                        .WhereEquals("Color", "Gray")
                        .AndAlso()
                        .WhereEquals("Size", "Large")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
