package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.indexes.spatial.SpatialRelation;
import net.ravendb.client.documents.queries.spatial.PointField;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class Spatial {

    private static class Event {

    }

    //region spatial_3_2
    public static class Events_ByCoordinates extends AbstractIndexCreationTask {
        public Events_ByCoordinates() {
            map = "docs.Events.Select(e => new {" +
                "    Coordinates = this.CreateSpatialField(((double ? ) e.latitude), ((double ? ) e.longitude))" +
                "})";
        }
    }
    //endregion

    public Spatial() {
        try (IDocumentStore store = new DocumentStore()) {
            try (IDocumentSession session = store.openSession()) {
                //region spatial_1_0
                List<Event> results = session
                    .query(Event.class)
                    .spatial(new PointField("latitude", "longitude"),
                        criteria -> criteria.withinRadius(500, 30, 30))
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region spatial_2_0
                List<Event> results = session
                    .query(Event.class)
                    .spatial(new PointField("latitude", "longitude"),
                        criteria -> criteria.relatesToShape(
                            "Circle(30 30 d=500.0000)",
                            SpatialRelation.WITHIN
                        ))
                    .toList();
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region spatial_3_0
                List<Event> results = session
                    .query(Event.class, Events_ByCoordinates.class)
                    .spatial("coordinates",
                        criteria -> criteria.withinRadius(500, 30, 30))
                    .toList();
                //endregion
            }
        }
    }
}
