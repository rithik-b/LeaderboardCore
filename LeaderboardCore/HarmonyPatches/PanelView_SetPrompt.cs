using HarmonyLib;
using HMUI;
using IPA.Loader;
using System;
using System.Reflection;

namespace LeaderboardCore.HarmonyPatches
{
    [HarmonyPatch]
    internal class PanelView_SetPrompt
    {
		public static event Action ScoreUploaded;
		private static MethodBase TargetMethod()
		{
			PluginMetadata pluginFromId = PluginManager.GetPluginFromId("ScoreSaber");
			if (pluginFromId != null)
			{
				return pluginFromId.Assembly.GetType("ScoreSaber.UI.ViewControllers.PanelView").GetMethod("SetPrompt", BindingFlags.Instance | BindingFlags.Public);
			}
			return null;
		}

		private static void Postfix(ref CurvedTextMeshPro ___promptText)
		{
			if (___promptText.text.Contains("Score uploaded"))
            {
				ScoreUploaded?.Invoke();
            }
		}
	}
}
