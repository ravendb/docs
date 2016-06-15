package net.ravendb.samples.indexes.querying;

import java.util.List;

import net.ravendb.client.indexes.AbstractIndexCreationTask;

import com.mysema.query.annotations.QueryEntity;


public class Intersection {
  @QueryEntity
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

  @QueryEntity
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
  }

  public static class TShirts_ByManufacturerColorSizeAndReleaseYear extends AbstractIndexCreationTask {
    // empty
  }
}
