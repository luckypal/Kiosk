namespace InstallKiosk
{
	public class Working_List
	{
		public string Title;

		public string Information;

		public string Parameters;

		public Constants.sStatus Status;

		public string Command;

		public bool Download = false;

		public bool ConfirmDownload = false;

		public bool Copy = false;

		public bool ConfirmCopy = false;

		public bool Create = false;

		public bool ConfirmCreate = false;

		public string Question;

		public bool YesNo = false;

		public int Tipo = 0;

		public bool Run = false;

		public Working_List()
		{
			Status = Constants.sStatus.Idle;
		}
	}
}
