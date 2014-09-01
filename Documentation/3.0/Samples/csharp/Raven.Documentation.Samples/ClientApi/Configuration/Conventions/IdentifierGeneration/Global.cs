namespace Raven.Documentation.Samples.ClientApi.Configuration.Conventions.IdentifierGeneration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Client.Converters;
	using Client.Document;
	using CodeSamples;

	public class Global
	{
		#region custom_converter
		public class UInt32Converter : ITypeConverter
		{
			public bool CanConvertFrom(Type sourceType)
			{
				throw new CodeOmitted();
			}

			public string ConvertFrom(string tag, object value, bool allowNull)
			{
				throw new CodeOmitted();
			}

			public object ConvertTo(string value)
			{
				throw new CodeOmitted();
			}
		}
		#endregion

		public Global()
		{
			var store = new DocumentStore();

			DocumentConvention Conventions = store.Conventions;

			#region key_generator_hilo
			var hiLoGenerator = new MultiDatabaseHiLoGenerator(32);
			Conventions.DocumentKeyGenerator = (dbName, databaseCommands, entity) =>
								hiLoGenerator.GenerateDocumentKey(dbName, databaseCommands, Conventions, entity);

			var asyncHiLoGenerator = new AsyncMultiDatabaseHiLoKeyGenerator(32);
			Conventions.AsyncDocumentKeyGenerator = (dbName, commands, entity) =>
								asyncHiLoGenerator.GenerateDocumentKeyAsync(dbName, commands, Conventions, entity);
			#endregion

			#region key_generator_identityKeys
			Conventions.DocumentKeyGenerator = (dbname, commands, entity) =>
								store.Conventions.GetTypeTagName(entity.GetType()) + "/";
			#endregion

			#region find_type_name
			Conventions.FindClrTypeName = type => // use reflection to determine the type;
			#endregion
				string.Empty;

			#region find_clr_type
			Conventions.FindClrType = (id, doc, metadata) =>
								metadata.Value<string>(Abstractions.Data.Constants.RavenClrType);
			#endregion

			#region find_type_tagname
			Conventions.FindTypeTagName = type => // function that provides the collection name based on the entity type
			#endregion
				string.Empty;

			#region find_dynamic_tag_name
			Conventions.FindDynamicTagName = dynamicObject => // function to determine the collection name for the given dynamic object
			#endregion
				string.Empty;

			#region find_identity_property
			Conventions.FindIdentityProperty = memberInfo => memberInfo.Name == "Id";
			#endregion

			#region find_iden_propn_name_from_entity_name
			Conventions.FindIdentityPropertyNameFromEntityName = entityName => "Id";
			#endregion

			#region identity_part_separator
			Conventions.IdentityPartsSeparator = "/";
			#endregion

			#region identity_type_convertors
			Conventions.IdentityTypeConvertors = new List<ITypeConverter>
			{
				new GuidConverter(),
				new Int32Converter(),
				new Int64Converter()
			};
			#endregion

			#region identity_type_convertors_2
			Conventions.IdentityTypeConvertors.Add(new UInt32Converter());
			#endregion

			#region find_id_value_part_for_value_type_conversion
			Conventions.FindIdValuePartForValueTypeConversion = (entity, id) =>
				id.Split(new[] { Conventions.IdentityPartsSeparator }, StringSplitOptions.RemoveEmptyEntries).Last();
			#endregion

			#region find_full_doc_key_from_non_string_identifier
			Conventions.FindFullDocumentKeyFromNonStringIdentifier = (id, type, allowNull) => // by default returns [tagName]/[identityValue];
			#endregion
				string.Empty;
		}
	}
}