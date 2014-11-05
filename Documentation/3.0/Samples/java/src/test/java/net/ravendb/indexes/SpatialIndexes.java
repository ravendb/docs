package net.ravendb.indexes;

import java.util.List;

import com.mysema.query.annotations.QueryEntity;

import net.ravendb.abstractions.basic.UseSharpEnum;
import net.ravendb.abstractions.indexing.SpatialOptions;
import net.ravendb.abstractions.indexing.SpatialOptions.SpatialRelation;
import net.ravendb.abstractions.indexing.SpatialOptions.SpatialUnits;
import net.ravendb.abstractions.indexing.SpatialOptionsFactory.CartesianSpatialOptionsFactory;
import net.ravendb.abstractions.indexing.SpatialOptionsFactory.GeographySpatialOptionsFactory;
import net.ravendb.abstractions.indexing.SpatialOptionsFactory.SpatialBounds;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentQueryCustomizationFactory;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;
import net.ravendb.client.spatial.SpatialCriteria;
import net.ravendb.client.spatial.SpatialCriteriaFactory;


public class SpatialIndexes {

  //region spatial_search_enhancements_3
  public static class SpatialOptionsFactory {
    public GeographySpatialOptionsFactory getGeography() {
      return new GeographySpatialOptionsFactory();
    }

    public CartesianSpatialOptionsFactory getCartesian() {
      return new CartesianSpatialOptionsFactory();
    }
  }
  //endregion

  public interface IGeographySpatialOptionsFactory {
    //region spatial_search_enhancements_4
    public SpatialOptions defaultOptions();

    // GeohashPrefixTree strategy with maxTreeLevel set to 9
    public SpatialOptions defaultOptions(SpatialUnits circleRadiusUnits);

    public SpatialOptions boundingBoxIndex();

    public SpatialOptions boundingBoxIndex(SpatialUnits circleRadiusUnits);

    public SpatialOptions geohashPrefixTreeIndex(int maxTreeLevel);

    public SpatialOptions geohashPrefixTreeIndex(int maxTreeLevel, SpatialUnits circleRadiusUnits);

    public SpatialOptions quadPrefixTreeIndex(int maxTreeLevel);

    public SpatialOptions quadPrefixTreeIndex(int maxTreeLevel, SpatialUnits circleRadiusUnits);
    //endregion
  }

  public interface ISpatialCriteriaFactory {
    //region spatial_search_enhancements_a
    public SpatialCriteria relatesToShape(Object shape, SpatialRelation relation);

    public SpatialCriteria intersects(Object shape);

    public SpatialCriteria contains(Object shape);

    public SpatialCriteria disjoint(Object shape);

    public SpatialCriteria within(Object shape);

    public SpatialCriteria withinRadiusOf(double radius, double x, double y);
    //endregion
  }

  public interface ICartesianSpatialOptionsFactory {
    //region spatial_search_enhancements_5
    public SpatialOptions boundingBoxIndex();

    public SpatialOptions quadPrefixTreeIndex(int maxTreeLevel, SpatialBounds bounds);
    //endregion
  }

  public interface Foo {
    //region spatial_search_5
    public DocumentQueryCustomizationFactory relatesToShape(final String fieldName, final String shapeWKT, final SpatialRelation rel);
    //endregion
  }

  //region spatial_search_7
  @UseSharpEnum
  public enum SpatialRelation {
    WITHIN, CONTAINS, DISJOINT, INTERSECTS,

    /**
     * Does not filter the query, merely sort by the distance
     */
    NEARBY
  }
  //endregion

  @QueryEntity
  public static class SpatialDoc {
    private Object shape;
    private Object point;

    public Object getShape() {
      return shape;
    }
    public void setShape(Object shape) {
      this.shape = shape;
    }
    public Object getPoint() {
      return point;
    }
    public void setPoint(Object point) {
      this.point = point;
    }
  }

  //region spatial_search_enhancements_8
  public static class SpatialDoc_ByShapeAndPoint extends AbstractIndexCreationTask {
    public SpatialDoc_ByShapeAndPoint() {
      QSpatialIndexes_SpatialDoc s = QSpatialIndexes_SpatialDoc.spatialDoc;
      map =
       " from spatial in docs        " +
       " select new                  " +
       " {                           " +
       "     Shape = spatial.Shape,  " +
       "     Point = spatial.Point   " +
       " }; ";

      spatial(s.shape, new SpatialOptionsFactory().getGeography().defaultOptions());
      spatial(s.point, new SpatialOptionsFactory().getCartesian().boundingBoxIndex());
    }
  }
  //endregion

  //region spatial_search_enhancements_1
  @QueryEntity
  public static class EventWithWKT {
    private String id;
    private String name;
    private String WKT;

    public String getId() {
      return id;
    }

    public void setId(String id) {
      this.id = id;
    }

    public String getName() {
      return name;
    }

    public void setName(String name) {
      this.name = name;
    }

    public String getWKT() {
      return WKT;
    }

    public void setWKT(String wKT) {
      WKT = wKT;
    }
  }
  //endregion

  //region spatial_search_1
  public static class Event {
    private String id;
    private String name;
    private double latiude;
    private double longitude;

    public String getId() {
      return id;
    }
    public void setId(String id) {
      this.id = id;
    }
    public String getName() {
      return name;
    }
    public void setName(String name) {
      this.name = name;
    }
    public double getLatiude() {
      return latiude;
    }
    public void setLatiude(double latiude) {
      this.latiude = latiude;
    }
    public double getLongitude() {
      return longitude;
    }
    public void setLongitude(double longitude) {
      this.longitude = longitude;
    }
  }
  //endregion

  //region spatial_search_2
  public static class Events_ByNameAndCoordinates extends AbstractIndexCreationTask {
    public Events_ByNameAndCoordinates() {
      map =
       " from e in docs.Events                                             " +
       " select new                                                        " +
       " {                                                                 " +
       "     Name = e.Name,                                                " +
       "     __ = SpatialGenerate(\"Coordinates\", e.Latitude, e.Longitude)" +
       " }; ";
    }
  }
  //endregion

  //region spatial_search_enhancements_2
  public static class EventsWithWKT_ByNameAndWKT extends AbstractIndexCreationTask {
    public EventsWithWKT_ByNameAndWKT() {
      QSpatialIndexes_EventWithWKT e = QSpatialIndexes_EventWithWKT.eventWithWKT;
      map =
       " from e in docs.Events    " +
       " select new          " +
       " {                   " +
       "     Name = e.Name,  " +
       "     WKT = e.WKT     " +
       " }; ";

      spatial(e.WKT, new SpatialOptionsFactory().getGeography().defaultOptions());
    }
  }
  //endregion

  @SuppressWarnings("unused")
  public void sample() throws Exception {


    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region spatial_search_3
        List<Event> results = session
          .query(Event.class, Events_ByNameAndCoordinates.class)
          .customize(new DocumentQueryCustomizationFactory().withinRadiusOf("Coordinates", 10, 32.1234, 23.4321))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region spatial_search_8
        List<Event> results = session
          .advanced()
          .documentQuery(Event.class, Events_ByNameAndCoordinates.class)
          .withinRadiusOf("Coordinates", 10, 32.1234, 23.4321)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region spatial_search_4
        List<Event> results = session
          .query(Event.class, Events_ByNameAndCoordinates.class)
          .customize(new DocumentQueryCustomizationFactory()
            .relatesToShape("Coordinates", "Circle(32.1234 23.4321 d=10.0000)",
              net.ravendb.abstractions.indexing.SpatialOptions.SpatialRelation.WITHIN))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region spatial_search_9
        List<Event> results = session
          .advanced()
          .documentQuery(Event.class, Events_ByNameAndCoordinates.class)
          .relatesToShape("Coordinates", "Circle(32.1234 23.4321 d=10.0000)",
              net.ravendb.abstractions.indexing.SpatialOptions.SpatialRelation.WITHIN)
          .toList();
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        Object someWktShape = null;

        //region spatial_search_enhancements_9
        QSpatialIndexes_SpatialDoc s = QSpatialIndexes_SpatialDoc.spatialDoc;
        List<SpatialDoc> results = session
          .query(SpatialDoc.class, SpatialDoc_ByShapeAndPoint.class)
          .spatial(s.shape, new SpatialCriteriaFactory().withinRadiusOf(500, 30, 30))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        Object someWktShape = null;

        //region spatial_search_enhancements_1_0
        QSpatialIndexes_SpatialDoc s = QSpatialIndexes_SpatialDoc.spatialDoc;
        List<SpatialDoc> results = session
          .advanced()
          .documentQuery(SpatialDoc.class, SpatialDoc_ByShapeAndPoint.class)
          .spatial(s.shape, new SpatialCriteriaFactory().intersects(someWktShape))
          .toList();
        //endregion
      }
    }
  }

}
