namespace Raven.Documentation.Samples.Migration.Server
{
    public class Indexes
    {
        /*
        #region indexes_1
        from doc in doc.Orders
        let company = LoadDocument(doc.Company)
        select new 
        {
            CompanyName: company.Name
        }
        #endregion
         */

        /*
        #region indexes_2
        from doc in doc.Orders
        let company = LoadDocument(doc.Company, "Companies")
        select new 
        {
            CompanyName: company.Name
        }
        #endregion
         */

        /*
        #region indexes_3
        from b in docs.Places
        select new {
	        _ = AbstractIndexCreationTask.SpatialGenerate(
                                    (double?)b.Latitude, 
                                    (double?)b.Longitude)
        }
        #endregion
        */

        /*
        #region indexes_4
        from b in docs.Places
        select new {
            Coordinates = CreateSpatialField(
                                (double?)b.Latitude, 
                                (double?)b.Longitude)
        }
        #endregion
        */

        /*
        #region indexes_5
        from user in users
        select new
        {
            Query = AsDocument(user).Select(x => x.Value)
        };
        #endregion
        */

        /*
        #region indexes_6
        from user in users
        select new
        {
            Query = AsJson(user).Select(x => x.Value)
        };
        #endregion
        */
    }
}
