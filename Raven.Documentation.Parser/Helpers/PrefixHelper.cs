namespace Raven.Documentation.Parser.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Raven.Documentation.Parser.Attributes;
	using Raven.Documentation.Parser.Data;

	public class PrefixHelper
	{
		public static IDictionary<Category, string> GetCategoryPrefixes()
		{
			return Enum.GetValues(typeof(Category))
				.Cast<Category>()
				.ToDictionary(value => value, GetCategoryPrefix);
		}

		public static string GetCategoryPrefix(Category category)
		{
			var type = typeof(Category);
			var memInfo = type.GetMember(category.ToString());
			var attributes = memInfo[0].GetCustomAttributes(typeof(PrefixAttribute), false);
			return ((PrefixAttribute)attributes[0]).Prefix;
		} 
	}
}