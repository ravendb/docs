package net.ravendb.Indexes.Querying;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.Arrays;
import java.util.List;

public class Intersection {

    //region intersection_1
    public class TShirt {
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

    public class TShirtType {
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

        public TShirtType() {
        }

        public TShirtType(String color, String size) {
            this.color = color;
            this.size = size;
        }
    }
    //endregion

    //region intersection_2
    public static class TShirts_ByManufacturerColorSizeAndReleaseYear extends AbstractIndexCreationTask {
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
            map = "docs.TShirts.SelectMany(tshirt => tshirt.types, (tshirt, type) => new {" +
                "    manufacturer = tshirt.manufacturer," +
                "    color = type.color," +
                "    size = type.size," +
                "    releaseYear = tshirt.releaseYear" +
                "})";
        }
    }
    //endregion

    public Intersection() {
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
                    new TShirtType("Gray", "Large")
                ));
                session.store(tShirt1);

                TShirt tShirt2 = new TShirt();
                tShirt2.setId("tshirts/2");
                tShirt2.setManufacturer("Wolf");
                tShirt2.setReleaseYear(2011);
                tShirt2.setTypes(Arrays.asList(
                    new TShirtType("Blue", "Small"),
                    new TShirtType("Black", "Large"),
                    new TShirtType("Gray", "Medium")
                ));
                session.store(tShirt2);

                TShirt tShirt3 = new TShirt();
                tShirt3.setId("tshirts/3");
                tShirt3.setManufacturer("Raven");
                tShirt3.setReleaseYear(2011);
                tShirt3.setTypes(Arrays.asList(
                    new TShirtType("Yellow", "Small"),
                    new TShirtType("Gray", "Large")
                ));
                session.store(tShirt3);

                TShirt tShirt4 = new TShirt();
                tShirt4.setId("tshirts/4");
                tShirt4.setManufacturer("Raven");
                tShirt4.setReleaseYear(2012);
                tShirt4.setTypes(Arrays.asList(
                    new TShirtType("Blue", "Small"),
                    new TShirtType("Gray", "Large")
                ));
                session.store(tShirt4);
                //endregion
            }

            try (IDocumentSession session = store.openSession()) {
                //region intersection_4
                List<TShirts_ByManufacturerColorSizeAndReleaseYear.Result> result = session.query(TShirts_ByManufacturerColorSizeAndReleaseYear.Result.class,
                    TShirts_ByManufacturerColorSizeAndReleaseYear.class)
                    .whereEquals("manufacturer", "Raven")
                    .intersect()
                    .whereEquals("color", "Blue")
                    .andAlso()
                    .whereEquals("size", "Small")
                    .intersect()
                    .whereEquals("color", "Gray")
                    .andAlso()
                    .whereEquals("size", "large")
                    .toList();
                //endregion
            }
        }
    }
}
