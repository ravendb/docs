package net.ravendb.clientapi.session.querying;

import java.util.List;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.linq.IRavenQueryable;
import net.ravendb.samples.indexes.querying.Intersection.TShirt;
import net.ravendb.samples.indexes.querying.Intersection.TShirts_ByManufacturerColorSizeAndReleaseYear;
import net.ravendb.samples.indexes.querying.QIntersection_TShirt;
import net.ravendb.samples.indexes.querying.QIntersection_TShirtType;


public class HowToUseIntersect {
  @SuppressWarnings("unused")
  private interface IFoo<T> {
    //region intersect_1
    public IRavenQueryable<T> intersect();
    //endregion
  }

  @SuppressWarnings("unused")
  public HowToUseIntersect() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region intersect_2
        // return all T-shirts that are manufactured by 'Raven'
        // and contain both 'Small Blue' and 'Large Gray' types
        QIntersection_TShirt t = QIntersection_TShirt.tShirt;
        QIntersection_TShirtType tt = QIntersection_TShirtType.tShirtType;
        List<TShirt> tshirts = session
          .query(TShirt.class, TShirts_ByManufacturerColorSizeAndReleaseYear.class)
          .where(t.manufacturer.eq("Raven"))
          .intersect()
          .where(t.types.any(tt.color.eq("Blue").and(tt.size.eq("Small"))))
          .intersect()
          .where(t.types.any(tt.color.eq("Gray").and(tt.size.eq("Large"))))
          .toList();
        //endregion
      }
    }
  }
}
