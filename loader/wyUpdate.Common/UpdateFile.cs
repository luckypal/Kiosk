using System.Collections.Generic;
using System.Diagnostics;

namespace wyUpdate.Common
{
	public class UpdateFile
	{
		public string Filename;

		public string RelativePath;

		public string DeltaPatchRelativePath;

		public long NewFileAdler32;

		public bool Execute;

		public string CommandLineArgs;

		public bool ExBeforeUpdate;

		public bool WaitForExecution;

		public bool RollbackOnNonZeroRet;

		public List<int> RetExceptions;

		public ProcessWindowStyle ProcessWindowStyle;

		public ElevationType ElevationType;

		public bool IsNETAssembly;

		public CPUVersion CPUVersion;

		public FrameworkVersion FrameworkVersion;

		public bool DeleteFile;

		public COMRegistration RegisterCOMDll;
	}
}
