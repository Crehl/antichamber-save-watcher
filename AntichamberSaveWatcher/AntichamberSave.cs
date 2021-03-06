﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;

namespace AntichamberSaveWatcher
{
    class AntichamberSave
    {
        public string Path { get; private set; }
        private FileStream stream;

        // Total of 8 magic bytes at the start of the file
        public int MagicOne { get; private set; }
        public int MagicTwo { get; private set; }

        public float PlayTime { get; private set; } // Play time in seconds
        public TimeSpan TimeLeft
        {
            get
            {
                // Time remaining from the starting 1h30m
                TimeSpan left = new TimeSpan(1, 30, 0);
                left = left.Subtract(new TimeSpan(0, 0, (int)PlayTime));

                if (left.TotalSeconds < 0)
                    left = new TimeSpan(0, 0, 0);

                return left;
            }
        }

        public List<Pickup> SavedPickups { get; private set; } // Guns
        public List<Secret> SavedSecrets { get; private set; } // Pink cubes
        public List<Trigger> SavedTriggers { get; private set; } // Mostly signs, but also other stuff (map rooms?)

        public bool HiddenSignHints { get; private set; }

        public AntichamberSave(string fullPath)
        {
            SavedPickups = new List<Pickup>();
            SavedSecrets = new List<Secret>();
            SavedTriggers = new List<Trigger>();

            Path = fullPath;
            if (!File.Exists(Path))
                Console.WriteLine("Save file doesn't seem to exist - is the path correct?");

            if (!Reload(5))
            {
                if (Program.ShowDebug)
                    Console.WriteLine("Unable to load save file.");
            }
        }

        public bool Reload(int retries = 0, int sleepTime = 100)
        {
            // After an initial sleep, attempt to read the save file (retries + 1) times every sleepTime milliseconds.
            Thread.Sleep(50);

            do
            {
                try
                {
                    stream = File.OpenRead(Path);
                }
                catch (Exception e) {

                    if (Program.ShowDebug)
                        Console.WriteLine(e);

                    Thread.Sleep(sleepTime);
                    continue;
                }

                readFile();
                stream.Close();
                return true;

            } while (retries-- > 0);

            return false;
        }

        private void readFile()
        {
            SavedPickups = new List<Pickup>();
            SavedSecrets = new List<Secret>();
            SavedTriggers = new List<Trigger>();

            MagicOne = (int)readLittleEndian(4);
            MagicTwo = (int)readLittleEndian(4);

            while (true)
            {
                string propName = readString();
                if (propName == "None")
                    break;

                string propType = readString();
                switch (propName)
                {
                    case "PlayTime":
                        PlayTime = readFloatProperty();
                        break;
                    case "bHiddenSignHints":
                        HiddenSignHints = readBoolProperty();
                        break;
                    case "SavedPickups":
                        foreach (string pickup in readArrayProperty())
                            SavedPickups.Add(new Pickup(pickup));
                        break;
                    case "SavedSecrets":
                        foreach (string secret in readArrayProperty())
                            SavedSecrets.Add(new Secret(secret));
                        break;
                    case "SavedTriggers":
                        foreach (string trigger in readArrayProperty())
                            SavedTriggers.Add(new Trigger(trigger));
                        break;
                    default:
                        readProperty(propType);
                        break;
                }
            }
        }

        private void readProperty(string propType)
        {
            switch (propType)
            {
                case "BoolProperty":
                    readBoolProperty();
                    break;
                case "FloatProperty":
                    readFloatProperty();
                    break;
                case "ArrayProperty":
                    readArrayProperty();
                    break;
            }
        }

        private long readLittleEndian(int bytes)
        {
            long val = 0;

            for (int i = 0; i < bytes; i++)
                val += stream.ReadByte() * (1 << i * 8);

            return val;
        }

        private string readString()
        {
            long length = readLittleEndian(4);
            StringBuilder sb = new StringBuilder((int)length - 1);

            while (length-- > 1)
                sb.Append((char)stream.ReadByte());

            stream.ReadByte();
            return sb.ToString();
        }

        private bool readBoolProperty()
        {
            readLittleEndian(8);
            return readLittleEndian(1) != 0;
        }

        private string[] readArrayProperty()
        {
            long length = readLittleEndian(8);
            long count = readLittleEndian(4);

            string[] elements = new string[count];

            for (int i = 0; i < count; i++)
                elements[i] = readString();

            return elements;
        }

        private float readFloatProperty()
        {
            stream.Seek(8, SeekOrigin.Current);
            byte[] fbytes = new byte[4];
            stream.Read(fbytes, 0, 4);
            return System.BitConverter.ToSingle(fbytes, 0);
        }
    }
}
