namespace Raven.Documentation.Parser.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Raven.Documentation.Parser.Data;

	public static class CategoryHelper
	{
		private static readonly IDictionary<Category, string> CategoryPrefixes = PrefixHelper.GetCategoryPrefixes();

		public static Category ExtractCategoryFromPath(string key)
		{
			var values = key.Split('/');

			try
			{
				return CategoryPrefixes.First(x => x.Value == values[0]).Key;
			}
			catch (Exception)
			{
				throw new NotSupportedException(string.Format("Could not find category: '{0}'", values[0]));
			}
		}
	}
}