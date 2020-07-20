package net.ravendb.ClientApi.Session.attachments;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.operations.attachments.AttachmentDetails;
import net.ravendb.client.documents.operations.attachments.AttachmentName;
import net.ravendb.client.documents.operations.attachments.CloseableAttachmentResult;
import net.ravendb.client.documents.session.IDocumentSession;

import java.io.FileInputStream;
import java.io.InputStream;

public class Attachments {
    private interface IFoo {
        //region StoreSyntax
        void store(String documentId, String name, InputStream stream);

        void store(String documentId, String name, InputStream stream, String contentType);

        void store(Object entity, String name, InputStream stream);

        void store(Object entity, String name, InputStream stream, String contentType);
        //endregion

        //region GetSyntax
        AttachmentName[] getNames(Object entity);

        boolean exists(String documentId, String name);

        CloseableAttachmentResult get(String documentId, String name);

        CloseableAttachmentResult get(Object entity, String name);

        CloseableAttachmentResult getRevision(String documentId, String name, String changeVector);
        //endregion

        //region DeleteSyntax
        void delete(String documentId, String name);

        void delete(Object entity, String name);
        //endregion
    }

    public Attachments() throws Exception {
        try (IDocumentStore store = new DocumentStore()) {
            //region StoreAttachment
            try (IDocumentSession session = store.openSession()) {
                try (
                    FileInputStream file1 = new FileInputStream("001.jpg");
                    FileInputStream file2 = new FileInputStream("002.jpg");
                    FileInputStream file3 = new FileInputStream("003.jpg");
                    FileInputStream file4 = new FileInputStream("004.mp4")
                ) {
                    Album album = new Album();
                    album.setName("Holidays");
                    album.setDescription("Holidays travel pictures of the all family");
                    album.setTags(new String[] { "Holidays Travel", "All Family" });
                    session.store(album, "albums/1");

                    session.advanced().attachments()
                        .store("albums/1", "001.jpg", file1, "image/jpeg");
                    session.advanced().attachments()
                        .store("albums/1", "002.jpg", file2, "image/jpeg");
                    session.advanced().attachments()
                        .store("albums/1", "003.jpg", file3, "image/jpeg");
                    session.advanced().attachments()
                        .store("albums/1", "004.mp4", file4, "video/mp4");

                    session.saveChanges();
                }
            }
            //endregion
        }
    }

    public void getAttachment() throws Exception {
        try (IDocumentStore store = new DocumentStore()) {
            //region GetAttachment
            try (IDocumentSession session = store.openSession()) {
                Album album = session.load(Album.class, "albums/1");

                try (CloseableAttachmentResult file1 = session
                        .advanced().attachments().get(album, "001.jpg");
                    CloseableAttachmentResult file2 = session
                        .advanced().attachments().get("albums/1", "002.jpg")) {

                    InputStream inputStream = file1
                        .getData();

                    AttachmentDetails attachmentDetails = file1.getDetails();
                    String name = attachmentDetails.getName();
                    String contentType = attachmentDetails.getContentType();
                    String hash = attachmentDetails.getHash();
                    long size = attachmentDetails.getSize();
                    String documentId = attachmentDetails.getDocumentId();
                    String changeVector = attachmentDetails.getChangeVector();
                }

                AttachmentName[] attachmentNames = session.advanced().attachments().getNames(album);
                for (AttachmentName attachmentName : attachmentNames) {

                    String name = attachmentName.getName();
                    String contentType = attachmentName.getContentType();
                    String hash = attachmentName.getHash();
                    long size = attachmentName.getSize();
                }

                boolean exists = session.advanced().attachments().exists("albums/1", "003.jpg");
            }
            //endregion
        }
    }

    public void deleteAttachment() {
        try (IDocumentStore store = new DocumentStore()) {
            //region DeleteAttachment
            try (IDocumentSession session = store.openSession()) {
                Album album = session.load(Album.class, "albums/1");
                session.advanced().attachments().delete(album, "001.jpg");
                session.advanced().attachments().delete("albums/1", "002.jpg");

                session.saveChanges();
            }
            //endregion
        }
    }

    public static class Album {
        private String id;
        private String name;
        private String description;
        private String[] tags;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public String[] getTags() {
            return tags;
        }

        public void setTags(String[] tags) {
            this.tags = tags;
        }
    }
}
