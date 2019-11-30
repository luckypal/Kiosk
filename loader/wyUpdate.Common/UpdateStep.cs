namespace wyUpdate.Common
{
	internal enum UpdateStep
	{
		CheckForUpdate = 0,
		ForceRecheckForUpdate = 5,
		DownloadUpdate = 1,
		BeginExtraction = 2,
		RestartInfo = 3,
		Install = 4
	}
}
