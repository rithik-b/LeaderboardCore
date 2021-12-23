using System;
using System.Collections.Generic;
using Zenject;
using LeaderboardCore.Interfaces;
using LeaderboardCore.HarmonyPatches;

namespace LeaderboardCore.Managers
{
    internal class LeaderboardCoreManager : IInitializable, IDisposable
    {
        private readonly List<INotifyLeaderboardActivate> notifyLeaderboardActivates;
        private readonly List<INotifyLeaderboardSet> notifyLeaderboardSets;
        private readonly List<INotifyLeaderboardLoad> notifyLeaderboardLoads;
        private readonly List<INotifyScoreUpload> notifyScoreUploads;

        public LeaderboardCoreManager(List<INotifyLeaderboardActivate> notifyLeaderboardActivates, List<INotifyLeaderboardSet> notifyLeaderboardSets,
            List<INotifyLeaderboardLoad> notifyLeaderboardLoads, List<INotifyScoreUpload> notifyScoreUploads)
        {
            this.notifyLeaderboardActivates = notifyLeaderboardActivates;
            this.notifyLeaderboardSets = notifyLeaderboardSets;
            this.notifyLeaderboardLoads = notifyLeaderboardLoads;
            this.notifyScoreUploads = notifyScoreUploads;
        }

        public void Initialize()
        {
            PanelView_Show.ViewActivated += LeaderboardActivated;
            PlatformLeaderboardViewController_SetData.LeaderboardSet += LeaderboardSet;
            PanelView_SetIsLoaded.IsLoadedChanged += PanelViewLoadingChanged;
            PanelView_SetPrompt.ScoreUploaded += ScoreUploaded;
        }

        public void Dispose()
        {
            PanelView_Show.ViewActivated -= LeaderboardActivated;
            PlatformLeaderboardViewController_SetData.LeaderboardSet -= LeaderboardSet;
            PanelView_SetIsLoaded.IsLoadedChanged -= PanelViewLoadingChanged;
            PanelView_SetPrompt.ScoreUploaded -= ScoreUploaded;
        }

        private void LeaderboardActivated()
        {
            foreach (var notifyLeaderboardActivate in notifyLeaderboardActivates)
            {
                notifyLeaderboardActivate.OnLeaderboardActivated();
            }
        }

        private void LeaderboardSet(IDifficultyBeatmap difficultyBeatmap)
        {
            foreach (var notifyLeaderboardSet in notifyLeaderboardSets)
            {
                notifyLeaderboardSet.OnLeaderboardSet(difficultyBeatmap);
            }
        }

        private void PanelViewLoadingChanged(bool loaded)
        {
            foreach (var notifyLeaderboardLoad in notifyLeaderboardLoads)
            {
                notifyLeaderboardLoad.OnLeaderboardLoaded(loaded);
            }
        }

        private void ScoreUploaded()
        {
            foreach (var notifyScoreUpload in notifyScoreUploads)
            {
                notifyScoreUpload.OnScoreUploaded();
            }
        }
    }
}
