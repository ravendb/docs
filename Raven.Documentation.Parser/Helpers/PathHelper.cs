namespace Raven.Documentation.Parser.Helpers
{
	using System.IO;

	public static class PathHelper
	{
		public static string GetProperDirectoryCapitalization(DirectoryInfo dirInfo)
		{
			DirectoryInfo parentDirInfo = dirInfo.Parent;
			if (null == parentDirInfo)
				return dirInfo.Name;
			return Path.Combine(GetProperDirectoryCapitalization(parentDirInfo),
								parentDirInfo.GetDirectories(dirInfo.Name)[0].Name);
		}

		public static string GetProperFilePathCapitalization(string filename)
		{
			FileInfo fileInfo = new FileInfo(filename);
			DirectoryInfo dirInfo = fileInfo.Directory;
			return Path.Combine(GetProperDirectoryCapitalization(dirInfo),
								dirInfo.GetFiles(fileInfo.Name)[0].Name);
		}
	}
}
