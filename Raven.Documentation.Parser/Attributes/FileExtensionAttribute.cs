namespace Raven.Documentation.Parser.Attributes
{
	using System;

	public class FileExtensionAttribute : Attribute
	{
		private readonly string _extension;

		public FileExtensionAttribute(string extension)
		{
			_extension = extension;
		}

		public string Extension
		{
			get
			{
				return _extension;
			}
		}
	}
}