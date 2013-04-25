using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;

namespace AntichamberSaveWatcher
{
	class Program
	{
		static string path = @"C:\Steam\SteamApps\common\Antichamber\Binaries\Win32\";
		static string file = "SavedGame.bin";
		static AntichamberSave save;

		static bool noResize = false;
		static bool trackSigns = true;
		static bool trackCubes = false;
		
		static void Main(string[] args)
		{
			parseArgs(args);

			if (!noResize)
			{
				Console.SetBufferSize(115, 150);
				Console.SetWindowSize(115, 8);
			}

			save = new AntichamberSave(path + file);

			FileSystemWatcher fsw = new FileSystemWatcher(path, file);
			fsw.Changed += update;
			fsw.EnableRaisingEvents = true;

			Console.ReadLine();
		}

		static void parseArgs(string[] args)
		{
			if (args.Length > 0)
			{
				trackCubes = false;
				trackSigns = false;
			}

			int i = 0;
			while (i < args.Length)
			{
				switch (args[i])
				{
					case "-s":
					case "--signs":
						trackSigns = true;
						break;
					case "-c":
					case "--cubes":
						trackCubes = true;
						break;
					case "-f":
					case "--file":
						if (args.Length > i + 1)
						{
							string location = args[++i];
							path = Path.GetDirectoryName(location) + Path.DirectorySeparatorChar;
							file = Path.GetFileName(location);
						}
						break;
					case "-n":
					case "--no-resize":
						noResize = true;
						break;
				}
				i++;
			}
		}

		private static void update(object sender, FileSystemEventArgs e)
		{
			List<int> previousSigns = new List<int>();
			foreach (Trigger trigger in save.SavedTriggers)
				if (trigger.SignNum > 0)
					previousSigns.Add(trigger.SignNum);

			List<string> previousCubes = new List<string>();
			foreach (Secret secret in save.SavedSecrets)
				previousCubes.Add(secret.FullName);

			save.Reload();

			int signs = previousSigns.Count + 1;
			if (signs == 1)
				Console.Clear();

			int cubes = previousCubes.Count;

			if (trackSigns)
			{
				foreach (Trigger trigger in save.SavedTriggers)
					if (trigger.SignNum > 0 && !previousSigns.Contains(trigger.SignNum))
						Console.WriteLine(String.Format("{0} - SIGN {1}/120 - {2}", new TimeSpan(0, 0, (int)save.PlayTime), ++signs, trigger.SignText));
			}

			if (trackCubes)
			{
				foreach (Secret secret in save.SavedSecrets)
					if (!previousCubes.Contains(secret.FullName))
						Console.WriteLine(String.Format("{0} - PINK CUBE {1}/13", new TimeSpan(0, 0, (int)save.PlayTime), ++cubes));
			}
		}
	}
}
