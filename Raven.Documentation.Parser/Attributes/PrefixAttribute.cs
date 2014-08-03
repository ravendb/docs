namespace Raven.Documentation.Parser.Attributes
{
	using System;

	public class PrefixAttribute : Attribute
	{
		private readonly string _prefix;

		public PrefixAttribute(string prefix)
		{
			_prefix = prefix;
		}

		public string Prefix
		{
			get
			{
				return _prefix;
			}
		}
	}
}