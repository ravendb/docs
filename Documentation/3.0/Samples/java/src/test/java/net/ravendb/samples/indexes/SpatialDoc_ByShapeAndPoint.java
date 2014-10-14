package net.ravendb.samples.indexes;

import net.ravendb.abstractions.indexing.SpatialOptionsFactory;
import net.ravendb.client.indexes.AbstractIndexCreationTask;


public class SpatialDoc_ByShapeAndPoint extends AbstractIndexCreationTask {

  public SpatialDoc_ByShapeAndPoint() {
    QSpatialDoc s = QSpatialDoc.spatialDoc;
    map = "from spatial in docs"
      + "select new "
      + "{ "
      + "   Shape = spatial.Shape, "
      + "   Point = spatial.Point"
      + "}";

    spatial(s.shape, new SpatialOptionsFactory().getGeography().defaultOptions());
    spatial(s.point, new SpatialOptionsFactory().getCartesian().boundingBoxIndex());
  }
}
