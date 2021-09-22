package net.ravendb.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractJavaScriptIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;
import net.ravendb.client.documents.session.IRawDocumentQuery;

import java.util.Collections;
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
            setMaps(Collections.singleton(
                    "map('HotelRooms', function (room) {\n"+
                    "   var guests = cmpxchg(room.RoomID);\n"+
                    "   return {\n"+
                    "       RoomID: room.RoomID,\n"+
                    "       Guests: guests\n"+
                    "   }\n"+
                    "});"
                    )
            );
        };
    }
    //endregion
    public IndexingCompareExchange() {
        try (IDocumentStore store = new DocumentStore( new String[]{ "http://localhost:8080" }, "Northwind")) {
            store.initialize();
            try (IDocumentSession session = store.openSession()) {
                //region query_0
                IRawDocumentQuery<Hotelroom> VIPRooms = session.advanced().rawQuery(Hotelroom.class,
                    "from Hotelrooms as room\n" +
                        "where room.Guests == cmpxchg('VIP')");

                //endregion
            }
        }
    }
}



