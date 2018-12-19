//region camera
export class Camera {
    constructor(manufacturer, model, {
        dateOfListing,
        cost,
        zoom,
        megapixels,
        imageStabilizer
    }) {
        this.manufacturer = manufacturer;
        this.model = model;
        this.dateOfListing = dateOfListing;
        this.cost = cost;
        this.zoom = zoom;
        this.megapixels = megapixels;
        this.imageStabilizer = imageStabilizer;
    }
}
//endregion

