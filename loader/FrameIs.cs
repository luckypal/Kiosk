public static class FrameIs
{
	public static bool ErrorFinish(Frame frame)
	{
		if (frame != Frame.UpdatedSuccessfully && frame != Frame.AlreadyUpToDate && frame != Frame.NoUpdatePathAvailable)
		{
			return frame == Frame.Error;
		}
		return true;
	}

	public static bool Interaction(Frame frame)
	{
		if (frame != Frame.UpdateInfo && frame != Frame.UpdatedSuccessfully && frame != Frame.AlreadyUpToDate && frame != Frame.NoUpdatePathAvailable)
		{
			return frame == Frame.Error;
		}
		return true;
	}
}
