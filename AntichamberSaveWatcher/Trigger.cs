using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntichamberSaveWatcher
{
	class Trigger
	{
		private static Tuple<string, int[]> duplicate(string desc, params int[] nums)
		{
			return new Tuple<string, int[]>(desc, nums);
		}

		public static readonly Tuple<string, int[]>[] Duplicates =
		{
			duplicate("Some things don't have a deeper meaning.",                       55, 120),
			duplicate("Peeking behind the curtains lets us see how everything works.",  106, 142),
			duplicate("The further we explore, the more conencted everything becomes.", 35, 41, 134, 135),
			duplicate("Some paths are straight forward.",                               46, 89),
			duplicate("Dig a little deeper and you may find something new.",            87, 148, 149, 150, 151, 152, 153, 154),
			duplicate("Old solutions can apply to new problems.",                       105, 107, 126, 127),
			duplicate("We fall down when there is nothing to support us.",              60, 118)
		};

		public static Dictionary<int, string> Signs = new Dictionary<int, string>()
		{
			{ 2, "Some outcomes are more favourable than others." },
			{ 3, "The world is full of secret waiting to be uncovered." },
			{ 5, "Some challenges exist just to test how much we know." },
			{ 6, "Some choices leave us running around a lot without really getting anywhere." },
			{ 7, "A problem may only be difficult when you are missing the right tools." },
			{ 8, "New skills enable further progress." },
			{ 9, "What we've done before may impact what we can do next." },
			{ 12, "Falling down teaches us how to get up and try again." },
			{ 13, "Life is full of ups and downs." },
			{ 14, "The world is always finding new ways to surprise us." },
			{ 15, "Some hurdles are too high to jump over." },
			{ 16, "When you look beyond the surface, there may be more to find." },
			{ 17, "Moving through a problem slower may help find the solution." },
			{ 18, "The solution to a problem may just required a more thorough look at it." },
			{ 19, "There are multiple ways to approach a situation." },
			{ 20, "Moving forwards may require making the most of what you've got." },
			{ 22, "Understanding a problem requires filling in the pieces." },
			{ 23, "It's harder to progress if you're leaving things behind." },
			{ 24, "The further we get, the less help we need." },
			{ 25, "Half way through is half way finished." },
			{ 26, "Failing to succeed does not mean failing to progress." },
			{ 27, "Small steps can take you great distances." },
			{ 28, "Life has a way of pushing us in the right direction." },
			{ 30, "Straightforward problems can often require roundabout solutions." },
			{ 31, "Some problems just come down to size." },
			{ 32, "If you lead the way others will follow." },
			{ 33, "Venturing into the unknown can lead to great rewards." },
			{ 36, "Sometimes we do things just to go along for the ride." },
			{ 37, "Some doors will close unless we hold them open." },
			{ 38, "How we perceive a problem can change every time we see it." },
			{ 39, "Old skills are useful even after we have learned new ones." },
			{ 40, "The right answers may also be the most obvious ones." },
			{ 42, "Getting to the end requires tying off the loose ends." },
			{ 43, "No matter how high you climb, there's always more to achieve." },
			{ 44, "Look a little harder and you will find a way forward." },
			{ 45, "Some obstacles are more stubborn than others." },
			{ 47, "A few steps backwards may keep you moving forwards." },
			{ 48, "When you've hit rock bottom, the only way is up." },
			{ 49, "A dead end will only stop you if you don't try to move through it." },
			{ 50, "What looks out of reach may only be a few steps away." },
			{ 51, "There comes a time when you can work your way through anything." },
			{ 52, "Taking the first step can be harder than the rest of the challenge." },
			{ 53, "Live on your own watch, not someone else's." },
			{ 54, "Complicated problems are easier when solved one step at a time." },
			{ 56, "Patience has it's own rewards." },
			{ 57, "At times we may need to view the world from someone else's perspective." },
			{ 58, "We often fall into things when we least expect." },
			{ 59, "Sometimes we only have just enough to get by." },
			{ 61, "Building a bridge can get you over a problem." },
			{ 62, "What appears impossible may have a very simple answer." },
			{ 64, "Signs may be helping you more than you realise." },
			{ 65, "When you absorb your surroundings you may notice things you didn't see before." },
			{ 66, "With forethought, things have a way of just working themselves out." },
			{ 67, "There's nothing wrong with taking shortcuts." },
			{ 69, "The choice doesn't matter if the outcome is the same." },
			{ 70, "If you are missing information, it's easy to be mislead." },
			{ 71, "Some choices are only useful when we make them early." },
			{ 72, "The consequences of one choice can cut us off from making others." },
			{ 73, "With more experience previous challenges aren't so difficult." },
			{ 74, "Many small obstacles can make for one large problem." },
			{ 76, "Getting to a solution requires cutting out what doesn't work." },
			{ 77, "When what you have is not enough, find ways to turn it into more." },
			{ 78, "If you're only focusing on right now, you won't have enough for later." },
			{ 79, "The world looks different on the other side." },
			{ 80, "A little kind direction can get obstacles out of your way." },
			{ 81, "If you lose sight of what's important, it may not be there when you need it." },
			{ 82, "If you never stop trying you will get there eventually." },
			{ 83, "The right decisions at the right time will get you where you want to go." },
			{ 84, "The best solutions may still be the most primitive ones." },
			{ 86, "The world rarely changes when we watch to see it happen." },
			{ 88, "Too much curiosity can get the best of us." },
			{ 91, "Similar problems can have entirely different solutions." },
			{ 92, "A window of oppurtunity can lead to new places if you're willing to take a closer look." },
			{ 93, "Some tasks require a lot of care and observation." },
			{ 96, "The path of least resistance is a valid option." },
			{ 98, "Try hard enough and you will get to where you want to be." },
			{ 99, "Some problems can't be solved until you're more experienced." },
			{ 100, "We move on when there is nothing left to learn." },
			{ 103, "You can't do everything yourself." },
			{ 104, "Solving a problem may require using abilities that we didn't know we had." },
			{ 108, "There's no need to take apart what already works." },
			{ 109, "The end may come before we were ready to get there." },
			{ 110, "A path may not be right or wrong. It may just be different." },
			{ 113, "You can grow a garden anywhere." },
			{ 114, "Going a certain way may require building your own path." },
			{ 115, "Some paths are clearer than others." },
			{ 116, "Mastering a skill requires practice." },
			{ 119, "We can appreciate the entire journey by looking back at how far we have come." },
			{ 121, "Obscure problems may require unusual solutions." },
			{ 122, "Taking one path often means missing out on another." },
			{ 123, "When you return to where you have been, things aren't always as remembered." },
			{ 124, "Some choices can leave us running around in circles." },
			{ 125, "Raw persistance may be the only option other than giving up entirely." },
			{ 128, "Sometimes you need to be carried." },
			{ 129, "Life isn't about getting to the end." },
			{ 130, "A choice may be as simple as going left or going right." },
			{ 131, "Rushing through a problem won't always give the right results." },
			{ 133, "The more we complete, the harder it gets to find what we missed." },
			{ 136, "Solving a problem may require approaching it from a different angle." },
			{ 140, "Connecting the pieces can solve a puzzle." },
			{ 144, "When you have enough resources, you can start growing more." },
			{ 146, "Some events happen whether we want them to or not." },
			{ 157, "The problem may not be where you're going but how to get there." },
			{ 160, "Splitting a problem up may help you find the answer." },
			{ 162, "Attention to detail can lead to very rewarding outcomes." },
			{ 163, "If you aren't paying attention, you will miss everything around you." },
			{ 164, "Some challenges are far harder than they first appear." },
			{ 165, "Throwing yourself into things can take you to new heights." },
			{ 166, "To get past a problem, you may just need to keep pushing through it." },
			{ 167, "Getting where we want may require jumping through some hoops." },
			{ 168, "Every journey comes to an end." },
			{ 169, "If you don't like where you've ended up try doing something else." }
		};

		static Trigger()
		{
			// Add descriptions for duplicate signs to the main dictionary
			foreach (Tuple<string, int[]> duplicateTuple in Duplicates)
				foreach (int num in duplicateTuple.Item2)
					Signs.Add(num, duplicateTuple.Item1);
		}

		public static int CountDuplicates(List<int> signNums)
		{
			int total = 0;

			foreach (Tuple<string, int[]> duplicateTuple in Duplicates)
			{
				int count = signNums.Count(x => duplicateTuple.Item2.Contains(x));

				if (count > 1)
					total += count - 1;
			}

			return total;
		}

		public string FullName { get; private set; }
		public int SignNum { get; private set; }
		public string SignText { get; private set; }

		public Trigger(string name)
		{
			FullName = name;

			SignNum = 0;
			SignText = "<Unknown>";

			// Check that the map (very first part of the trigger name) is HazardIGFChinaSplit
			string[] split = name.Split('.');
			if (split.Length == 3 && split[0] == "HazardIGFChinaSplit")
			{
				// Number at the very end of the trigger name
				string num = split[2].Substring(split[2].IndexOf('_') + 1);

				int n;
				bool success = int.TryParse(num, out n);
				if (success && Signs.ContainsKey(n))
				{
					SignNum = n;
					SignText = Signs[n];
				}
			}
		}
	}
}
