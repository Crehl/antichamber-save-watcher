using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.IO;
using System.Threading;

namespace AntichamberSaveWatcher
{
	class Program
	{
		static bool customPath = false;
		static string path = @"C:\Program Files (x86)\Steam\SteamApps\common\Antichamber\Binaries\Win32\";
		static string file = "SavedGame.bin";
		static AntichamberSave save;

		static bool noResize = false;
		static bool trackSigns = true;
		static bool trackCubes = false;
		static bool trackGuns = false;

		public static bool ShowDebug = false;
		
		static void Main(string[] args)
		{
			Console.CursorVisible = false;
			
			parseArgs(args);

			//If a path wasn't specified in a command line arg, attempt to find one from the running processes
			if (!customPath)
				findPath();

			if (!noResize)
			{
				Console.SetBufferSize(115, 150);
				Console.SetWindowSize(115, 8);
			}

			if (!trackCubes && !trackSigns && !trackGuns)
				Console.WriteLine("Currently tracking nothing - are the command line arguments correct?");

			save = new AntichamberSave(path + file);

			//Watch the save file for changes
			FileSystemWatcher fsw = new FileSystemWatcher(path, file);
			fsw.Changed += update;
			fsw.EnableRaisingEvents = true;

			//Quit program on enter
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
				string next = (i < args.Length - 1) ? args[i + 1] : "";

				if (args[i].Length > 2 && args[i].StartsWith("--")) //e.g. --signs
					handleFlag(args[i].Substring(2), next);
				else if (args[i][0] == '-') //e.g. -s or -sc
				{
					for (int j = 1; j < args[i].Length; j++) 
						handleFlag(args[i].Substring(j, 1), next);
				}

				i++;
			}
		}

		static void handleFlag(string flag, string next)
		{
			switch (flag)
			{
				case "s":
				case "signs":
					trackSigns = true;
					break;
				case "c":
				case "cubes":
					trackCubes = true;
					break;
				case "g":
				case "guns":
					trackGuns = true;
					break;
				case "d":
				case "debug":
					ShowDebug = true;
					break;
				case "f":
				case "file":
					if (next != "")
					{
						path = Path.GetDirectoryName(next) + Path.DirectorySeparatorChar;
						file = Path.GetFileName(next);
						customPath = true;
					}
					break;
				case "n":
				case "no-resize":
					noResize = true;
					break;
			}
		}

		static void findPath()
		{
			Process[] processes = Process.GetProcesses();
			string fullName = "";
			bool foundSteam = false;

			foreach (Process process in processes)
			{
				//Don't always have permissions to read the FileName for a process
				try
				{
					fullName = process.MainModule.FileName;
				}
				catch
				{
					continue;
				}

				//Antichamber binary
				if (fullName.EndsWith(@"Antichamber\Binaries\Win32\UDK.exe"))
				{
					string p = Path.GetDirectoryName(fullName) + Path.DirectorySeparatorChar;
					if (File.Exists(Path.Combine(p, "SavedGame.bin")))
					{
						Console.WriteLine("Found running Antichamber process, setting path:\n" + p + "SavedGame.bin");
						path = p;
						return;
					}
				}
				
				//Steam binary
				if (fullName.EndsWith(@"Steam\Steam.exe"))
				{
					string p = Path.Combine(Path.GetDirectoryName(fullName), "SteamApps", "common", "Antichamber", "Binaries", "Win32") + Path.DirectorySeparatorChar;
					if (File.Exists(Path.Combine(p, "SavedGame.bin")))
					{
						foundSteam = true;
						path = p;
						continue;
					}
				}
			}

			if (foundSteam)
				Console.WriteLine("Found running Steam process, setting path:\n" + path + "SavedGame.bin");
		}

		private static void update(object sender, FileSystemEventArgs e)
		{
			try
			{
				//Store all the previous sign #s...
				List<int> previousSigns = new List<int>();
				foreach (Trigger trigger in save.SavedTriggers)
					if (trigger.SignNum > 0)
						previousSigns.Add(trigger.SignNum);

				//... and cube (trigger) names...
				List<string> previousCubes = new List<string>();
				foreach (Secret secret in save.SavedSecrets)
					previousCubes.Add(secret.FullName);

				//... and guns.
				List<Pickup.Gun> previousGuns = new List<Pickup.Gun>();
				foreach (Pickup pickup in save.SavedPickups)
					if (pickup.AssociatedGun != Pickup.Gun.Unknown)
						previousGuns.Add(pickup.AssociatedGun);

				//Reload the save file
				if (!save.Reload())
				{
					if (ShowDebug)
						Console.WriteLine("Unable to reload save file.");
					return;
				}

				int signs = previousSigns.Count + 1;
				if (signs == 1)
				{
					Console.Clear();
					Console.WriteLine("00:00:00 - SIGN 1/120 - Every journey is a series of choices. The first is to begin the journey.");
				}

				int cubes = previousCubes.Count;

				//Compare new signs, cubes, etc against the stored previous lists
				//Write any new things to the console

				if (trackSigns)
				{
					List<int> signNums = new List<int>();
					int extraSigns = 0;

					foreach (Trigger trigger in save.SavedTriggers)
						signNums.Add(trigger.SignNum);

					if (signNums.Contains(55) && signNums.Contains(120)) extraSigns++;
					if (signNums.Contains(106) && signNums.Contains(142)) extraSigns++;
					signs -= extraSigns;

					string signExtra = extraSigns == 0 ? "" : " (+" + extraSigns.ToString() + ")";

					foreach (Trigger trigger in save.SavedTriggers)
					{
						if (trigger.SignNum > 0 && !previousSigns.Contains(trigger.SignNum))
							Console.WriteLine(String.Format("{0} - SIGN {1}/120{3} - {2}", new TimeSpan(0, 0, (int)save.PlayTime), ++signs, trigger.SignText, signExtra));
						
					}
				}

				if (trackCubes)
				{
					List<string> secretNames = new List<string>();
					int extraCubes = 0;

					foreach (Secret secret in save.SavedSecrets)
						secretNames.Add(secret.FullName);

					if (secretNames.Contains("HazardSeamless.TheWorld:PersistentLevel.HazardSecretTile_15")) extraCubes++;
					if (secretNames.Contains("HazardIGFChinaSplit.TheWorld:PersistentLevel.HazardSecretTile_0")) extraCubes++;
					cubes -= extraCubes;

					string cubeExtra = extraCubes == 0 ? "" : " (+" + extraCubes.ToString() + ")";

					foreach (Secret secret in save.SavedSecrets)
					{
						if (!previousCubes.Contains(secret.FullName))
							Console.WriteLine(String.Format("{0} - PINK CUBE {1}/13{2}", new TimeSpan(0, 0, (int)save.PlayTime), ++cubes, cubeExtra));
					}
				}

				if (trackGuns)
				{
					foreach (Pickup pickup in save.SavedPickups)
						if (pickup.AssociatedGun != Pickup.Gun.Unknown && !previousGuns.Contains(pickup.AssociatedGun))
							Console.WriteLine(String.Format("{0} - GUN: {1}", new TimeSpan(0, 0, (int)save.PlayTime), pickup.AssociatedGun.ToString()));
				}
			}
			catch (Exception exc)
			{
				if (Program.ShowDebug)
					Console.WriteLine(exc.StackTrace);
			}
		}
	}
}
