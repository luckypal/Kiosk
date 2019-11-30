namespace wyUpdate
{
	public class FileFolder
	{
		public string Path;

		public bool isFolder;

		public bool deleteFolder;

		public FileFolder(string filePath)
		{
			Path = filePath;
			isFolder = false;
		}

		public FileFolder(string path, bool deleteFolder)
		{
			Path = path;
			isFolder = true;
			this.deleteFolder = deleteFolder;
		}
	}
}
