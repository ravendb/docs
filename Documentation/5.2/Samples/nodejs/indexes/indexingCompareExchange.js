import {
    DocumentStore,
    IndexDefinition,
    PutIndexesOperation,
    IndexDefinitionBuilder, AbstractJavaScriptIndexCreationTask, TimeSeriesAggregationResult,
    AbstractIndexCreationTask
} from "ravendb";

let key, keys;

class HotelRoom{}

const store = new DocumentStore();
const session = store.openSession();
{
    //region methods
    const loadcompare = loadCompareExchangeValue<string>(key)

    const loadcompares = loadCompareExchangeValue<string>(keys)
    //endregion
}

{
    //region index_1
    class Compare_Exchange_Index extends AbstractIndexCreationTask {
        constructor() {
            super();

            this.map = "map('HotelRooms', function (room) {\n" +
                "    var guests = cmpxchg(room.RoomID);\n" +
                "    return {\n" +
                "       RoomID: room.RoomID,\n" +
                "       Guests: guests\n" +
                "    };\n" +
                "})"
        }
    }
    //endregion
}

{
    //region query_2
    const VIPRooms = session
        .advanced.rawQuery<HotelRoom>(
        "   from Hotelrooms as room\n"+
        "   where room.Guests == cmpxchg('VIP')"
    )
    //endregion
}