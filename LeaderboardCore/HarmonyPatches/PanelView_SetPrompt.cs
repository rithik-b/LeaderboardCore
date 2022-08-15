using HarmonyLib;
using HMUI;
using System;
using System.Reflection;

namespace LeaderboardCore.HarmonyPatches
{
    [HarmonyPatch]
    internal class PanelView_SetPrompt
    {
		public static event Action ScoreUploaded;
		private static MethodBase TargetMethod() => 
			Plugin.Instance.scoreSaber.Assembly.GetType("ScoreSaber.UI.Leaderboard.PanelView").GetMethod("SetPrompt", BindingFlags.Instance | BindingFlags.Public);

		private static void Postfix(ref CurvedTextMeshPro ____promptText)
		{
			if (____promptText == null || ____promptText.text.Contains("Score uploaded"))
				return;

			ScoreUploaded?.Invoke();
		}
	}
}
