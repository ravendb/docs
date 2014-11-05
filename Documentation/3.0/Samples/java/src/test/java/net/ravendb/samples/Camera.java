package net.ravendb.samples;

import java.util.Date;
import java.util.List;

//region camera
public class Camera {
  private int id;

  private Date dateOfListing;
  private String manufacturer;
  private String model;
  private Double cost;

  private int zoom;
  private double megapixels;
  private boolean imageStabilizer;
  private List<String> advancedFeatures;

  public int getId() {
    return id;
  }
  public void setId(int id) {
    this.id = id;
  }
  public Date getDateOfListing() {
    return dateOfListing;
  }
  public void setDateOfListing(Date dateOfListing) {
    this.dateOfListing = dateOfListing;
  }
  public String getManufacturer() {
    return manufacturer;
  }
  public void setManufacturer(String manufacturer) {
    this.manufacturer = manufacturer;
  }
  public String getModel() {
    return model;
  }
  public void setModel(String model) {
    this.model = model;
  }
  public Double getCost() {
    return cost;
  }
  public void setCost(Double cost) {
    this.cost = cost;
  }
  public int getZoom() {
    return zoom;
  }
  public void setZoom(int zoom) {
    this.zoom = zoom;
  }
  public double getMegapixels() {
    return megapixels;
  }
  public void setMegapixels(double megapixels) {
    this.megapixels = megapixels;
  }
  public boolean isImageStabilizer() {
    return imageStabilizer;
  }
  public void setImageStabilizer(boolean imageStabilizer) {
    this.imageStabilizer = imageStabilizer;
  }
  public List<String> getAdvancedFeatures() {
    return advancedFeatures;
  }
  public void setAdvancedFeatures(List<String> advancedFeatures) {
    this.advancedFeatures = advancedFeatures;
  }
}
//endregion