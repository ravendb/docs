namespace Raven.Documentation.Parser.Data
{
	public class DocumentationImage
	{
		protected bool Equals(DocumentationImage other)
		{
			return string.Equals(ImagePath, other.ImagePath) && string.Equals(ImageKey, other.ImageKey);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((ImagePath != null ? ImagePath.GetHashCode() : 0) * 397) ^ (ImageKey != null ? ImageKey.GetHashCode() : 0);
			}
		}

		public string ImagePath { get; set; }

		public string ImageKey { get; set; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != this.GetType())
			{
				return false;
			}
			return Equals((DocumentationImage)obj);
		}
	}
}