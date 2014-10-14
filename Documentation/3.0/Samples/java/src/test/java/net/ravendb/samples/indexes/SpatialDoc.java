package net.ravendb.samples.indexes;

import com.mysema.query.annotations.QueryEntity;

@QueryEntity
public class SpatialDoc {
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
