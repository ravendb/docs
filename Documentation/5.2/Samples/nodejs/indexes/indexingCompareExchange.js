import {
    DocumentStore,
     AbstractCsharpIndexCreationTask
} from "ravendb";

let key, keys;

class HotelRoom{}

const store = new DocumentStore('http://127.0.0.1:8080', 'Northwind2');
const session = store.openSession();
{
    //region methods
    const loadcompare = loadCompareExchangeValue<string>(key)

    const loadcompares = loadCompareExchangeValue<string>(keys)
    //endregion
}

{
    //region index_1
    class Compare_Exchange_Index extends AbstractCsharpIndexCreationTask {
        constructor() {
            super();

            this.map = `docs.HotelRooms.Select(room => new { 
                             RoomID = room.RoomID,     
                             Guests = cmpxchg(room.RoomID)
                        })`;
        }
    }
    //endregion

}

{
    //region query_2
    const VIPRooms = await session
        .advanced.rawQuery<HotelRoom>(
        "   from Hotelrooms as room\n"+
        "   where room.Guests == cmpxchg('VIP')"
    )
    //endregion
}