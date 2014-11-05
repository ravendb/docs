package net.ravendb.indexes.querying;

import java.util.Arrays;
import java.util.List;

import com.mysema.query.annotations.QueryEntity;

import net.ravendb.abstractions.data.IndexQuery;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.indexes.AbstractIndexCreationTask;


public class Intersection {
  //region intersection_1
  public static class TShirt {
    private String id;
    private int releaseYear;
    private String manufacturer;
    private List<TShirtType> types;

    public String getId() {
      return id;
    }
    public void setId(String id) {
      this.id = id;
    }
    public int getReleaseYear() {
      return releaseYear;
    }
    public void setReleaseYear(int releaseYear) {
      this.releaseYear = releaseYear;
    }
    public String getManufacturer() {
      return manufacturer;
    }
    public void setManufacturer(String manufacturer) {
      this.manufacturer = manufacturer;
    }
    public List<TShirtType> getTypes() {
      return types;
    }
    public void setTypes(List<TShirtType> types) {
      this.types = types;
    }
  }

  public static class TShirtType {
    private String color;
    private String size;

    public String getColor() {
      return color;
    }
    public void setColor(String color) {
      this.color = color;
    }
    public String getSize() {
      return size;
    }
    public void setSize(String size) {
      this.size = size;
    }
    public TShirtType(String color, String size) {
      super();
      this.color = color;
      this.size = size;
    }
    public TShirtType() {
      super();
    }
  }
  //endregion

  //region intersection_2
  public static class TShirts_ByManufacturerColorSizeAndReleaseYear extends AbstractIndexCreationTask {
    @QueryEntity
    public static class Result {
      private String manufacturer;
      private String color;
      private String size;
      private int releaseYear;

      public String getManufacturer() {
        return manufacturer;
      }
      public void setManufacturer(String manufacturer) {
        this.manufacturer = manufacturer;
      }
      public String getColor() {
        return color;
      }
      public void setColor(String color) {
        this.color = color;
      }
      public String getSize() {
        return size;
      }
      public void setSize(String size) {
        this.size = size;
      }
      public int getReleaseYear() {
        return releaseYear;
      }
      public void setReleaseYear(int releaseYear) {
        this.releaseYear = releaseYear;
      }
    }

    public TShirts_ByManufacturerColorSizeAndReleaseYear() {
      map =
       " from tshirt in docs.Tshirts                 " +
       " from type in tshirt.Types                   " +
       " select new                                  " +
       "    {                                        " +
       "        Manufacturer = tshirt.Manufacturer,  " +
       "        Color = type.Color,                  " +
       "        Size = type.Size,                    " +
       "        ReleaseYear = tshirt.ReleaseYear     " +
       "    }; ";
    }
  }

  //endregion


  @SuppressWarnings("unused")
  public void sample() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      try (IDocumentSession session = store.openSession()) {
        //region intersection_3
        TShirt tShirt1 = new TShirt();
        tShirt1.setId("tshirts/1");
        tShirt1.setManufacturer("Raven");
        tShirt1.setReleaseYear(2010);
        tShirt1.setTypes(Arrays.asList(
          new TShirtType("Blue", "Small"),
          new TShirtType("Black", "Small"),
          new TShirtType("Black", "Medium"),
          new TShirtType("Gray", "Large")));

        session.store(tShirt1);

        TShirt tShirt2 = new TShirt();
        tShirt2.setId("tshirts/2");
        tShirt2.setManufacturer("Wolf");
        tShirt2.setReleaseYear(2011);
        tShirt2.setTypes(Arrays.asList(
          new TShirtType("Blue", "Small"),
          new TShirtType("Black", "Large"),
          new TShirtType("Gray", "Medium")));

        session.store(tShirt2);

        TShirt tShirt3 = new TShirt();
        tShirt3.setId("tshirts/3");
        tShirt3.setManufacturer("Raven");
        tShirt3.setReleaseYear(2011);
        tShirt3.setTypes(Arrays.asList(
          new TShirtType("Yellow", "Small"),
          new TShirtType("Gray", "Large")));

        session.store(tShirt3);

        TShirt tShirt4 = new TShirt();
        tShirt4.setId("tshirts/4");
        tShirt4.setManufacturer("Raven");
        tShirt4.setReleaseYear(2012);
        tShirt4.setTypes(Arrays.asList(
          new TShirtType("Blue", "Small"),
          new TShirtType("Gray", "Large")));

        session.store(tShirt4);
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region intersection_4
        QIntersection_TShirts_ByManufacturerColorSizeAndReleaseYear_Result x =
          QIntersection_TShirts_ByManufacturerColorSizeAndReleaseYear_Result.result;

        List<TShirt> results = session
          .query(TShirts_ByManufacturerColorSizeAndReleaseYear.Result.class, TShirts_ByManufacturerColorSizeAndReleaseYear.class)
          .where(x.manufacturer.eq("Raven"))
          .intersect()
          .where(x.color.eq("Blue").and(x.size.eq("Small")))
          .intersect()
          .where(x.color.eq("Gray").and(x.size.eq("Large")))
          .as(TShirt.class)
          .toList();
        //endregion
      }

      try (IDocumentSession session = store.openSession()) {
        //region intersection_5
        List<TShirt> results = session
          .advanced()
          .documentQuery(TShirt.class, TShirts_ByManufacturerColorSizeAndReleaseYear.class)
          .whereEquals("Manufacturer", "Raven")
          .intersect()
          .whereEquals("Color", "Blue")
          .andAlso()
          .whereEquals("Size", "Small")
          .intersect()
          .whereEquals("Color", "Gray")
          .andAlso()
          .whereEquals("Size", "Large")
          .toList();
        //endregion
      }
    }

    try (IDocumentStore store = new DocumentStore()) {
      //region intersection_6
      store
        .getDatabaseCommands()
        .query("TShirts/ByManufacturerColorSizeAndReleaseYear",
           new IndexQuery("Manufacturer:Raven INTERSECT Color:Blue AND Size:Small INTERSECT Color:Gray AND Size:Large"));
      //endregion
    }
  }

}
