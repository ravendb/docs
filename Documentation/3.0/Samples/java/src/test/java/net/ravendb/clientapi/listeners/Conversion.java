package net.ravendb.clientapi.listeners;

import net.ravendb.abstractions.json.linq.RavenJObject;


public class Conversion {
  //region document_conversion_interface
  public interface IDocumentConversionListener {
    /**
     * Called before converting an entity to a document and metadata
     * @param key
     * @param entity
     * @param metadata
     */
    void beforeConversionToDocument(String key, Object entity, RavenJObject metadata);

    /**
     * Called after having converted an entity to a document and metadata
     * @param key
     * @param entity
     * @param document
     * @param metadata
     */
    void afterConversionToDocument(String key, Object entity, RavenJObject document, RavenJObject metadata);

    /**
     * Called before converting a document and metadata to an entity
     * @param key
     * @param document
     * @param metadata
     */
    void beforeConversionToEntity(String key, RavenJObject document, RavenJObject metadata);

    /**
     * Called before converting a document and metadata to an entity
     * @param key
     * @param document
     * @param metadata
     * @param entity
     */
    void afterConversionToEntity(String key, RavenJObject document, RavenJObject metadata, Object entity);
  }
  //endregion

  //region document_conversion_example
  public static class Item {
    private String id;
    private String name;
    private String revision;
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
    public String getRevision() {
      return revision;
    }
    public void setRevision(String revision) {
      this.revision = revision;
    }
  }

  public static class MetadataToPropertyConversionListener implements IDocumentConversionListener {
    @Override
    public void beforeConversionToDocument(String key, Object entity, RavenJObject metadata) {
      //empty by design
    }

    @Override
    public void afterConversionToDocument(String key, Object entity, RavenJObject document, RavenJObject metadata) {
      if (!(entity instanceof Item)) {
        return ;
      }
      document.remove("Revision");
    }

    @Override
    public void beforeConversionToEntity(String key, RavenJObject document, RavenJObject metadata) {
      //empty by design
    }

    @Override
    public void afterConversionToEntity(String key, RavenJObject document, RavenJObject metadata, Object entity) {
      if (!(entity instanceof Item)) {
        return;
      }
      ((Item)entity).setRevision(metadata.value(String.class, "Raven-Document-Revision"));
    }

  }
  //endregion
}
