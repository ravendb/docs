package net.ravendb.clientapi.session.querying;

import java.util.List;

import net.ravendb.abstractions.indexing.SpatialOptions.SpatialRelation;
import net.ravendb.abstractions.indexing.SpatialOptions.SpatialUnits;
import net.ravendb.client.IDocumentQueryCustomization;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentQueryCustomizationFactory;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.linq.IRavenQueryable;
import net.ravendb.client.spatial.SpatialCriteria;
import net.ravendb.client.spatial.SpatialCriteriaFactory;
import net.ravendb.samples.indexes.QSpatialDoc;
import net.ravendb.samples.indexes.SpatialDoc;
import net.ravendb.samples.indexes.SpatialDoc_ByShapeAndPoint;

import com.mysema.query.types.Path;


public class HowToQuerySpatialIndex {
  @SuppressWarnings("unused")
  private interface IFoo<TResult> {
    //region spatial_1
    IRavenQueryable<TResult> spatial(Path<?> path, SpatialCriteria criteria);
    //endregion

    //region spatial_3
    public IDocumentQueryCustomization relatesToShape(String fieldName, String shapeWKT, SpatialRelation rel);
    //endregion

    //region spatial_5
    public IDocumentQueryCustomization sortByDistance();
    //endregion

    //region spatial_7
    public IDocumentQueryCustomization withinRadiusOf(double radius, double latitude, double longitude);

    public IDocumentQueryCustomization withinRadiusOf(double radius, double latitude, double longitude, SpatialUnits radiusUnits);

    public IDocumentQueryCustomization withinRadiusOf(String fieldName, double radius, double latitude, double longitude);

    public IDocumentQueryCustomization withinRadiusOf(String fieldName, double radius, double latitude, double longitude, SpatialUnits radiusUnits);
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToQuerySpatialIndex() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region spatial_2
        // return all matching entities
        // where 'Shape' (spatial field) is within 10 kilometers radius
        // from 32.1234 latitude and 23.4321 longitude coordinates
        QSpatialDoc s = QSpatialDoc.spatialDoc;
        List<SpatialDoc> results = session
          .query(SpatialDoc.class, SpatialDoc_ByShapeAndPoint.class)
          .spatial(s.shape, new SpatialCriteriaFactory().withinRadiusOf(10, 32.1234, 23.4321))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region spatial_4
        // return all matching entities
        // where 'Shape' (spatial field) is within 10 kilometers radius
        // from 32.1234 latitude and 23.4321 longitude coordinates
        // this equals to WithinRadiusOf(10, 32.1234, 23.4321)
        List<SpatialDoc> results = session
          .query(SpatialDoc.class, SpatialDoc_ByShapeAndPoint.class)
          .customize(new DocumentQueryCustomizationFactory()
            .relatesToShape("Shape", "Circle(32.1234 23.4321 d=10.0000)", SpatialRelation.WITHIN))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region spatial_6
        // return all matching entities
        // where 'Shape' (spatial field) is within 10 kilometers radius
        // from 32.1234 latitude and 23.4321 longitude coordinates
        // sort results by distance from origin point
        QSpatialDoc s = QSpatialDoc.spatialDoc;
        List<SpatialDoc> results = session
          .query(SpatialDoc.class, SpatialDoc_ByShapeAndPoint.class)
          .customize(new DocumentQueryCustomizationFactory().sortByDistance())
          .spatial(s.shape, new SpatialCriteriaFactory().withinRadiusOf(10, 32.1234, 23.4321))
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region spatial_8
        // return all matching entities
        // where 'Shape' (spatial field) is within 10 kilometers radius
        // from 32.1234 latitude and 23.4321 longitude coordinates
        QSpatialDoc s = QSpatialDoc.spatialDoc;
        List<SpatialDoc> results = session
          .query(SpatialDoc.class, SpatialDoc_ByShapeAndPoint.class)
          .customize(new DocumentQueryCustomizationFactory().withinRadiusOf("Shape", 10, 32.1234, 23.4321))
          .toList();
        //endregion
      }
    }
  }
}
