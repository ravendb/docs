package net.ravendb;

import java.util.Date;

public class Faceted {

    //region camera
    public class Camera {
        private Date dateOfListing;
        private String model;
        private double cost;
        private int zoom;
        private double megapixels;
        private boolean imageStabilizer;
        private String manufacturer;

        public Date getDateOfListing() {
            return dateOfListing;
        }

        public void setDateOfListing(Date dateOfListing) {
            this.dateOfListing = dateOfListing;
        }

        public String getModel() {
            return model;
        }

        public void setModel(String model) {
            this.model = model;
        }

        public double getCost() {
            return cost;
        }

        public void setCost(double cost) {
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

        public String getManufacturer() {
            return manufacturer;
        }

        public void setManufacturer(String manufacturer) {
            this.manufacturer = manufacturer;
        }
    }
    //endregion
}
