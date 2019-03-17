using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;

namespace Raven.Documentation.Samples.Server
{
    public class ScalarToRawString
    {
        #region interstellar_trip
        public class InterstellarTrip
        {
            public class Segment
            {
                public string SourcePlanet { get; set; }
                public string DestinationPlanet { get; set; }
                public decimal DistanceInKilemeters { get; set; }
            }
            public string TripName { get; set; }            
            public List<Segment> TripTrack { get; set; }                        
            public decimal TotalDistance { get; set; }
            public decimal GasBill { get; set; }
        }
        #endregion

        public void ProjectBigNumber()
        {
            var store = new DocumentStore();

            var session = store.OpenSession();

            
            var trips = session.Advanced.RawQuery<InterstellarTrip>(@"
                    #region query_with_big_number_projection
                    From InterstellarTrips as trip
                    Where trip.Name = $name
                    Select {
                        Name:a.Name,
                        TotalDistance: scalarToRawString(a, x=> x.TotalDistance)
                        GasBill: scalarToRawString(a, x=> x.GasBill)
                    }
                    #endregion
").AddParameter("$name", "John Doe").ToList();


            trips = session.Advanced.RawQuery<InterstellarTrip>(@"
                #region query_with_big_number_projection
                    From InterstellarTrips as trip
                    Where trip.TotalDistance = '314159265358979323846264'
                    Select x.Name, x.TotalDistance, x.GasBill // note that here we did not use a JavaScript projection                    
                #endregion
").AddParameter("$name", "John Doe").ToList();
        }

        public class InterstellarIndex : AbstractIndexCreationTask<InterstellarTrip>
        {
            public InterstellarIndex()
            {
                #region interstellar_static_index
                Map = interstellarTrips => from trip in interstellarTrips
                                           select new
                                           {
                                               TotalDistance = trip.TotalDistance.ToString()
                                           };
                #endregion
            }
        }
        
    }
}
