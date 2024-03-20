using System;
using System.Collections.Generic;
using HarmonyLib;
using LeaderboardCore.Interfaces;
using SiraUtil.Affinity;

namespace LeaderboardCore.AffinityPatches
{
    internal class LeaderboardSetDataPatch : IAffinity
    {
        private readonly List<INotifyLeaderboardSet> notifyLeaderboardSets;
        
        public LeaderboardSetDataPatch(List<INotifyLeaderboardSet> notifyLeaderboardSets)
        {
            this.notifyLeaderboardSets = notifyLeaderboardSets;
        }

        [AffinityPatch(typeof(PlatformLeaderboardViewController), nameof(PlatformLeaderboardViewController.SetData))]
        private void Patch(BeatmapKey beatmapKey)
        {
            foreach (var notifyLeaderboardSet in notifyLeaderboardSets)
            {
                notifyLeaderboardSet.OnLeaderboardSet(beatmapKey);
            }
        }
    }
}
