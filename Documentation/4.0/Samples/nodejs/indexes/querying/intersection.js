import { DocumentStore, AbstractIndexCreationTask } from "ravendb";

const documentStore = new DocumentStore();
const session = documentStore.openSession();

//region intersection_1
class TShirt {
    constructor(data) {
        this.id = data.id;
        this.manufacturer = data.manufacturer;
        this.releaseYear = data.releaseYear;
        this.types = data.types;
    }
}

class TShirtType {
    constructor(color, size) {
        this.color = color;
        this.size = size;
    }
}
//endregion

    //region intersection_2
    class TShirts_ByManufacturerColorSizeAndReleaseYearResult {
        constructor(data) {
            this.manufacturer = data.manufacturer;
            this.color = data.color;
            this.size= data.size;
            this.releaseYear = data.releaseYear;
        }
    }

    class TShirts_ByManufacturerColorSizeAndReleaseYear extends AbstractIndexCreationTask {

        constructor() {
            super();

            this.map = `docs.TShirts.SelectMany(tshirt => tshirt.types, (tshirt, type) => new {
                    manufacturer = tshirt.manufacturer,
                    color = type.color,
                    size = type.size,
                    releaseYear = tshirt.releaseYear
                })`;
        }
    }
    //endregion

async function intersection() {
    {
        //region intersection_3
        const tShirt1 = new TShirt({
            id: "tshirts/1",
            manufacturer: "Raven",
            releaseYear: 2010,
            types: [
                new TShirtType("Blue", "Small"),
                new TShirtType("Black", "Small"),
                new TShirtType("Black", "Medium"),
                new TShirtType("Gray", "Large")
            ]
        });
        await session.store(tShirt1);

        const tShirt2 = new TShirt({
            id: "tshirts/2",
            manufacturer: "Wolf",
            releaseYear: 2011,
            types: [
                new TShirtType("Blue", "Small"),
                new TShirtType("Black", "Large"),
                new TShirtType("Gray", "Medium")
            ]
        });
        await session.store(tShirt2);

        const tShirt3 = new TShirt({
            id: "tshirts/3",
            manufacturer: "Raven",
            releaseYear: 2011,
            types: [
                new TShirtType("Yellow", "Small"),
                new TShirtType("Gray", "Large")
            ]
        });
        await session.store(tShirt3);

        const tShirt4 = new TShirt({
            id: "tshirts/4",
            manufacturer: "Raven",
            releaseYear: 2012,
            types: [
                new TShirtType("Blue", "Small"),
                new TShirtType("Gray", "Large")
            ]
        });
        await session.store(tShirt4);
        //endregion
    }

    {
        //region intersection_4
        const result = await session.query({ 
                indexName: "TShirts/ByManufacturerColorSizeAndReleaseYear",
                documentType: TShirts_ByManufacturerColorSizeAndReleaseYearResult
            })
            .whereEquals("manufacturer", "Raven")
            .intersect()
            .whereEquals("color", "Blue")
            .andAlso()
            .whereEquals("size", "Small")
            .intersect()
            .whereEquals("color", "Gray")
            .andAlso()
            .whereEquals("size", "large")
            .all();
        //endregion
    }
}
