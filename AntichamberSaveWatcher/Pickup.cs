using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace AntichamberSaveWatcher
{
	class Pickup
	{
		public enum Gun
		{
			Unknown,
			Blue,
			Green,
			Yellow,
			Red
		};

		static Dictionary<string, Gun> GunNames = new Dictionary<string, Gun>()
		{
			{"HazardIGFChinaSplit.TheWorld:PersistentLevel.HazardPickupFactory_0", Gun.Blue},
			{"HazardIGFChinaSplit.TheWorld:PersistentLevel.HazardPickupFactory_1", Gun.Green},
			{"HazardIGFChinaSplit.TheWorld:PersistentLevel.HazardPickupFactory_8", Gun.Yellow},
			{"HazardIGFChinaSplit.TheWorld:PersistentLevel.HazardPickupFactory_3", Gun.Red}
		};

		public string FullName { get; private set; }
		public Gun AssociatedGun { get; private set; }

		public Pickup(string name)
		{
			FullName = name;
			if (GunNames.ContainsKey(name))
				AssociatedGun = GunNames[name];
			else
				AssociatedGun = Gun.Unknown;
		}
	}
}
