package net.ravendb.Indexes;

import com.google.common.collect.Lists;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.operations.attachments.AttachmentName;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class Gapfiller {



    try (IDocumentSession session = store.openSession()) {
    //region RQL_Query
        IRawDocumentQuery<RawQueryResult> query = session.advanced().rawQuery(RawQueryResult.class, "declare timeseries out(p)\n" +
                "{\n" +
                "    from p.HeartRate between $start and $end\n" +
                "    group by 1h\n" +
                "    select min(), max()\n" +
                "}\n" +
                "from index 'People' as p\n" +
                "where p.age > 49\n" +
                "select out(p) as heartRate, p.name")
                .addParameter("start", baseLine)
                .addParameter("end", DateUtils.addDays(baseLine, 1));
        //endregion
    }

}