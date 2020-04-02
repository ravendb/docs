using System.Dynamic;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Sparrow.Json;

namespace Raven.Documentation.Samples.ClientApi.Configuration.IdentifierGeneration
{
    public class Global
    {
        public Global()
        {
            var store = new DocumentStore()
            {
                Conventions =
                {
                    #region find_type_name

                    FindClrTypeName = type => // use reflection to determine the type;

                        #endregion

                        string.Empty,

                    #region find_clr_type

                    FindClrType = (id, doc) =>
                    {
                        if (doc.TryGet(Constants.Documents.Metadata.Key, out BlittableJsonReaderObject metadata) &&
                            metadata.TryGet(Constants.Documents.Metadata.RavenClrType, out string clrType))
                            return clrType;

                        return null;
                    },

                    #endregion

                    #region find_type_collection_name

                    FindCollectionName = type => // function that provides the collection name based on the entity type

                        #endregion

                        string.Empty,

                    #region find_dynamic_collection_name

                    FindCollectionNameForDynamic =
                        dynamicObject => // function to determine the collection name for the given dynamic object

                            #endregion

                            string.Empty,

                    #region transform_collection_name_to_prefix

                    TransformTypeCollectionNameToDocumentIdPrefix =
                        collectionName => // transform the collection name to the prefix of a identifier, e.g. [prefix]/12

                            #endregion

                            string.Empty,

                    #region find_identity_property

                    FindIdentityProperty = memberInfo => memberInfo.Name == "Id"

                    #endregion

                    ,

                    #region find_identity_property_name_from_collection_name

                    FindIdentityPropertyNameFromCollectionName = collectionName => "Id"

                    #endregion

                    ,

                    #region identity_part_separator

                    IdentityPartsSeparator = "/"

                    #endregion
                }
            };
        }

        private void Sample()
        {
            var store = new DocumentStore()
            {
                Conventions =
                {
                    #region find_dynamic_collection_name_sample_1
                    FindCollectionNameForDynamic = o => o.Collection
                    #endregion
                }
            };

            using (var session = store.OpenSession())
            {
                #region find_dynamic_collection_name_sample_2
                dynamic car = new ExpandoObject();
                car.Name = "Ford";
                car.Collection = "Cars";

                session.Store(car);

                dynamic animal = new ExpandoObject();
                animal.Name = "Rhino";
                animal.Collection = "Animals";

                session.Store(animal);
                #endregion
            }
        }
    }
}
