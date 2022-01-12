using System;
using HarmonyLib;

namespace LeaderboardCore.HarmonyPatches
{
    [HarmonyPatch(typeof(PlatformLeaderboardViewController), nameof(PlatformLeaderboardViewController.SetData))]
    internal class PlatformLeaderboardViewController_SetData
    {
        public static event Action<IDifficultyBeatmap> LeaderboardSet;
        private static void Postfix(IDifficultyBeatmap difficultyBeatmap)
        {
            LeaderboardSet?.Invoke(difficultyBeatmap);
        }
    }
}
