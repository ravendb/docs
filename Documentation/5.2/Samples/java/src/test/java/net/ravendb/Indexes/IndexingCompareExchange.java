import com.google.common.collect.Sets;
import net.ravendb.client.documents.indexes.AbstractJavaScriptIndexCreationTask;

import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class IndexingCompareExchange {

    public class Hotelroom{

    }
    //region index_1
    private class Compare_Exchange_JS_Index extends AbstractJavaScriptIndexCreationTask{
        public Compare_Exchange_JS_Index()
        {
            setMaps(Sets.newHashSet(
                    "map('HotelRooms', function (room) {"+
                            "var guests = cmpxchg(room.RoomID);"+
                            "return {"+
                            "RoomID: room.RoomID,"+
                            "Guests: guests"+
                            "});"
                    )
            );
        };
    }
    //endregion

    //region query_0
    List<Hotelroom> VIPRooms=session.advanced().rawQuery(Hotelroom.class,
            "from Hotelrooms as room"+
            "where room.Guests == cmpxchg('VIP')");
    //endregion
}



