namespace InstallKiosk
{
	public class Working_List
	{
		public string Title;

		public string Information;

		public string Parameters;

		public Constants.sStatus Status;

		public string Command;

		public bool Download;

		public bool ConfirmDownload;

		public bool Copy;

		public bool ConfirmCopy;

		public bool Create;

		public bool ConfirmCreate;

		public string Question;

		public bool YesNo;

		public int Tipo;

		public bool Run;

		public Working_List()
		{
			Status = Constants.sStatus.Idle;
		}
	}
}
