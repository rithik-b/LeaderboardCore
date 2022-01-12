using HarmonyLib;
using IPA.Loader;
using System;
using System.Reflection;

namespace LeaderboardCore.HarmonyPatches
{
	[HarmonyPatch]
    internal class PanelView_SetIsLoaded
    {
		public static event Action<bool> IsLoadedChanged;
		private static MethodBase TargetMethod()
		{
			PluginMetadata pluginFromId = PluginManager.GetPluginFromId("ScoreSaber");
			if (pluginFromId != null)
			{
				return pluginFromId.Assembly.GetType("ScoreSaber.UI.ViewControllers.PanelView").GetMethod("set_isLoaded", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField);
			}
			return null;
		}

		private static void Postfix(bool ____isLoaded)
        {
			IsLoadedChanged?.Invoke(____isLoaded);
        }
	}
}
