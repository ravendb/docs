using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    class IndexingCompareExchange
    {
        private interface IFoo
        {
            #region methods
            //Load one compare exchange value
            T LoadCompareExchangeValue<T>(string key);

            //Load multiple compare exchange values
            T[] LoadCompareExchangeValue<T>(IEnumerable<string> keys);
            #endregion
        }

        #region index_0
        private class Compare_Exchange_Index : AbstractIndexCreationTask<HotelRoom>
        {
            public class Result
            {
                public string Room;
                public string Guests;
            }

            public Compare_Exchange_Index()
            {
                Map = HotelRooms => from room in HotelRooms
                                    let guests = LoadCompareExchangeValue<string>(room.RoomID)
                                    select new Result
                                    {
                                        Room = room.RoomID,
                                        Guests = guests
                                    };
            }
        }
        #endregion

        #region index_1
        private class Compare_Exchange_JS_Index : AbstractJavaScriptIndexCreationTask
        {
            public Compare_Exchange_JS_Index()
            {
                Maps = new HashSet<string>
                {
                    @"map('HotelRooms', function (room) { 
                        var guests = cmpxchg(room.RoomID); 
                        return { 
                            RoomID: room.RoomID, 
                            Guests: guests 
                        };
                    })",
                };
            }
        }
        #endregion

        public async void Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query_0
                    // Return all vacant hotel rooms
                    List<HotelRoom> vacantRooms = session
                        .Query<HotelRoom, Compare_Exchange_Index>()
                        .Where(x => RavenQuery.CmpXchg<string>(x.RoomID) == null)
                        .OfType<HotelRoom>()
                        .ToList();
                    #endregion

                    #region query_2
                    // Return the room occupied by VIP guests
                    List<HotelRoom> VIPRooms = session
                        .Advanced.RawQuery<HotelRoom>(
                            @"from Hotelrooms as room
                            where room.Guests == cmpxchg('VIP')"
                        )
                        .ToList();
                    #endregion
                }
                using (var asyncSession = store.OpenAsyncSession())
                {

                    #region query_1
                    // Return all vacant hotel rooms
                    List<HotelRoom> vacantRooms = await asyncSession
                        .Query<HotelRoom, Compare_Exchange_Index>()
                        .Where(x => RavenQuery.CmpXchg<string>(x.RoomID) == null)
                        .OfType<HotelRoom>()
                        .ToListAsync();
                    #endregion
                }
            }
        }
    }

    internal class HotelRoom
    {
        public bool Occupied;
        public string RoomID;
        public string Guests;
    }
}
