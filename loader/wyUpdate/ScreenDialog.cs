namespace wyUpdate
{
	public class ScreenDialog
	{
		public string Title;

		public string SubTitle;

		public string Content;

		public bool IsEmpty
		{
			get
			{
				if (string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(SubTitle))
				{
					return string.IsNullOrEmpty(Content);
				}
				return false;
			}
		}

		public ScreenDialog(string title, string subtitle, string content)
		{
			Title = title;
			SubTitle = subtitle;
			Content = content;
		}

		public void Clear()
		{
			Title = (SubTitle = (Content = null));
		}
	}
}
