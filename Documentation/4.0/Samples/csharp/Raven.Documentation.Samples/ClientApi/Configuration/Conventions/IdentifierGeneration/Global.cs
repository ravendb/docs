using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Sparrow.Json;

namespace Raven.Documentation.Samples.ClientApi.Configuration.Conventions.IdentifierGeneration
{
	public class Global
	{
		public Global()
		{
			var store = new DocumentStore();

			DocumentConventions Conventions = store.Conventions;

			#region find_type_name
			Conventions.FindClrTypeName = type => // use reflection to determine the type;
			#endregion
				string.Empty;

			#region find_clr_type
			Conventions.FindClrType = (id, doc) =>
			{
			    if (doc.TryGet(Constants.Documents.Metadata.Key, out BlittableJsonReaderObject metadata) &&
			        metadata.TryGet(Constants.Documents.Metadata.RavenClrType, out string clrType))
			        return clrType;

			    return null;
			};
			#endregion

			#region find_type_tagname
			Conventions.FindClrTypeName = type => // function that provides the collection name based on the entity type
			#endregion
				string.Empty;

			#region find_dynamic_tag_name
			Conventions.FindClrTypeName = dynamicObject => // function to determine the collection name for the given dynamic object
			#endregion
				string.Empty;

			#region transform_tag_name_to_prefix
			Conventions.TransformTypeCollectionNameToDocumentIdPrefix = tagName => // transform the tag name to the prefix of a key, e.g. [prefix]/12
				#endregion
				string.Empty;

			#region find_identity_property
			Conventions.FindIdentityProperty = memberInfo => memberInfo.Name == "Id";
            #endregion

		    #region find_identity_property_name_from_coolection_name
            Conventions.FindIdentityPropertyNameFromCollectionName = collectionName => "Id";
			#endregion

			#region identity_part_separator
			Conventions.IdentityPartsSeparator = "/";
			#endregion
		}
	}
}
