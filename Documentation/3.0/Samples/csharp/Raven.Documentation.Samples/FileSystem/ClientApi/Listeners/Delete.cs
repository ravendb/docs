namespace Raven.Documentation.Samples.FileSystem.ClientApi.Listeners
{
	using System;
	using Abstractions.FileSystem;

	public class Delete
	{
		#region delete_interface
		public interface IFilesDeleteListener
		{
			/// <summary>
			/// Invoked before the delete request is sent to the server.
			/// </summary>
			/// <param name="file">The file to delete</param>
			bool BeforeDelete(FileHeader file);

			/// <summary>
			/// Invoked after the delete operation was effective on the server.
			/// </summary>
			/// <param name="filename">The filename of the deleted file</param>
			void AfterDelete(string filename);
		}
		#endregion

		#region example
		public class PreventDeletingReadOnlyFilesListener : IFilesDeleteListener
		{
			public bool BeforeDelete(FileHeader file)
			{
				return file.Metadata.Value<bool>("Read-Only") == false;
			}

			public void AfterDelete(string filename)
			{
			}
		}
		#endregion
	}
}