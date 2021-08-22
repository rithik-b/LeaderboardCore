using HarmonyLib;
using IPA.Loader;
using System;
using System.Reflection;

namespace LeaderboardCore.HarmonyPatches
{
    [HarmonyPatch]
    internal class PanelView_Show
    {
		public static event Action ViewActivated;
		private static MethodBase TargetMethod()
		{
			PluginMetadata pluginFromId = PluginManager.GetPluginFromId("ScoreSaber");
			if (pluginFromId != null && pluginFromId.Assembly.GetName().Version >= new Version("3.1.8"))
			{
				return pluginFromId.Assembly.GetType("ScoreSaber.UI.ViewControllers.PanelView").GetMethod("Show", BindingFlags.Instance | BindingFlags.Public);
			}
			return null;
		}

		private static void Postfix()
		{
			ViewActivated?.Invoke();
		}
	}
}
