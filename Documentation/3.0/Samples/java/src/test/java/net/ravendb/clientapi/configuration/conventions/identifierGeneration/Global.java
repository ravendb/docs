package net.ravendb.clientapi.configuration.conventions.identifierGeneration;

import java.lang.reflect.Field;
import java.util.Arrays;

import net.ravendb.CodeOmitted;
import net.ravendb.abstractions.data.Constants;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.connection.IDatabaseCommands;
import net.ravendb.client.converters.ITypeConverter;
import net.ravendb.client.converters.Int32Converter;
import net.ravendb.client.converters.Int64Converter;
import net.ravendb.client.converters.UUIDConverter;
import net.ravendb.client.delegates.DocumentKeyFinder;
import net.ravendb.client.delegates.IdValuePartFinder;
import net.ravendb.client.delegates.IdentityPropertyFinder;
import net.ravendb.client.delegates.IdentityPropertyNameFinder;
import net.ravendb.client.delegates.JavaClassFinder;
import net.ravendb.client.delegates.JavaClassNameFinder;
import net.ravendb.client.delegates.TypeTagNameToDocumentKeyPrefixTransformer;
import net.ravendb.client.document.DocumentConvention;
import net.ravendb.client.document.DocumentKeyGenerator;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.document.MultiDatabaseHiLoGenerator;
import net.ravendb.client.document.TypeTagNameFinder;


public class Global {
  //region custom_converter
  public static class UInt32Converter implements ITypeConverter {

    @Override
    public boolean canConvertFrom(Class<?> sourceType) {
      throw new CodeOmitted();
    }

    @Override
    public String convertFrom(String tag, Object value, boolean allowNull) {
      throw new CodeOmitted();
    }

    @Override
    public Object convertTo(String value) {
      throw new CodeOmitted();
    }
  }
  //endregion

  public Global() {
    final DocumentStore store = new DocumentStore();

    final DocumentConvention conventions = store.getConventions();

    //region key_generator_hilo
    final MultiDatabaseHiLoGenerator hiLoGenerator = new MultiDatabaseHiLoGenerator(32);
    conventions.setDocumentKeyGenerator(new DocumentKeyGenerator() {
      @Override
      public String generate(String dbName, IDatabaseCommands dbCommands, Object entity) {
        return hiLoGenerator.generateDocumentKey(dbName, dbCommands, conventions, entity);
      }
    });
    //endregion

    //region key_generator_identityKeys
    conventions.setDocumentKeyGenerator(new DocumentKeyGenerator() {
      @Override
      public String generate(String dbName, IDatabaseCommands dbCommands, Object entity) {
        return store.getConventions().getTypeTagName(entity.getClass()) + "/";
      }
    });
    //endregion

    //region find_type_tagname
    conventions.setFindTypeTagName(new TypeTagNameFinder() {
      @Override
      public String find(Class<?> clazz) {
        // function that provides the collection name based on the entity type
        return clazz.getSimpleName();
      }
    });
    //endregion

    //region transform_tag_name_to_prefix
    conventions.setTransformTypeTagNameToDocumentKeyPrefix(new TypeTagNameToDocumentKeyPrefixTransformer() {
      @Override
      public String transform(String tag) {
        // transform the tag name to the prefix of a key, e.g. [prefix]/12
        throw new CodeOmitted();
      }
    });
    //endregion


    //region find_type_name
    conventions.setFindJavaClassName(new JavaClassNameFinder() {
      @Override
      public String find(Class<?> clazz) {
        // use reflection to determine the type;
        return clazz.getSimpleName();
      }
    });
    //endregion

    //region find_clr_type
    conventions.setFindJavaClass(new JavaClassFinder() {
      @Override
      public String find(String id, RavenJObject doc, RavenJObject metadata) {
        return metadata.value(String.class, Constants.RAVEN_JAVA_CLASS);
      }
    });
    //endregion

    //region find_identity_property
    conventions.setFindIdentityProperty(new IdentityPropertyFinder() {
      @SuppressWarnings("boxing")
      @Override
      public Boolean find(Field field) {
        return "Id".equals(field.getName());
      }
    });
    //endregion


    //region find_iden_propn_name_from_entity_name

    conventions.setFindIdentityPropertyNameFromEntityName(new IdentityPropertyNameFinder() {
      @Override
      public String find(String entityName) {
        return "Id";
      }
    });
    //endregion

    //region identity_part_separator
    conventions.setIdentityPartsSeparator("/");
    //endregion


    //region identity_type_convertors
    conventions.setIdentityTypeConvertors(Arrays.asList(
      new UUIDConverter(),
      new Int32Converter(),
      new Int64Converter()
      ));
    //endregion

    //region identity_type_convertors_2
    conventions.getIdentityTypeConvertors().add(new UInt32Converter());
    //endregion

    //region find_id_value_part_for_value_type_conversion
    conventions.setFindIdValuePartForValueTypeConversion(new IdValuePartFinder() {
      @Override
      public String find(Object entity, String id) {
        return id.split(conventions.getIdentityPartsSeparator())[1];
      }
    });
    //endregion

    //region find_full_doc_key_from_non_string_identifier
    conventions.setFindFullDocumentKeyFromNonStringIdentifier(new DocumentKeyFinder() {

      @Override
      public String find(Object id, Class<?> type, Boolean allowNull) {
        // by default returns [tagName]/[identityValue];
        throw new CodeOmitted();
      }
    });
    //endregion

  }
}
