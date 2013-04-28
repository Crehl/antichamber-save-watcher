﻿using System;
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
				string next = (i < args.Length - 1) ? args[i + 1] : "";

				if (args[i].Length > 2 && args[i].StartsWith("--"))
					handleFlag(args[i].Substring(2), next);
				else if (args[i][0] == '-')
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
				
				try
				{
					fullName = process.MainModule.FileName;
				}
				catch
				{
					continue;
				}

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
				List<int> previousSigns = new List<int>();
				foreach (Trigger trigger in save.SavedTriggers)
					if (trigger.SignNum > 0)
						previousSigns.Add(trigger.SignNum);

				List<string> previousCubes = new List<string>();
				foreach (Secret secret in save.SavedSecrets)
					previousCubes.Add(secret.FullName);

				List<Pickup.Gun> previousGuns = new List<Pickup.Gun>();
				foreach (Pickup pickup in save.SavedPickups)
					if (pickup.AssociatedGun != Pickup.Gun.Unknown)
						previousGuns.Add(pickup.AssociatedGun);

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
