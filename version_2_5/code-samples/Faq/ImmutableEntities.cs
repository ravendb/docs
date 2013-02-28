namespace RavenCodeSamples.Faq
{
	using System.Reflection;

	using Raven.Imports.Newtonsoft.Json;
	using Raven.Imports.Newtonsoft.Json.Serialization;

	public class ImmutableEntities : CodeSampleBase
	{
		#region immutable_entities_1
		public class PrivatePropertySetterResolver : DefaultContractResolver
		{
			protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
			{
				var prop = base.CreateProperty(member, memberSerialization);
				if (!prop.Writable)
				{
					var property = member as PropertyInfo;
					if (property != null)
					{
						var hasPrivateSetter = property.GetSetMethod(true) != null;
						prop.Writable = hasPrivateSetter;
					}
				}

				return prop;
			}
		}

		#endregion
	}
}