using System;
using System.Collections.Generic;
using Zenject;
using LeaderboardCore.Interfaces;
using LeaderboardCore.HarmonyPatches;

namespace LeaderboardCore.Managers
{
    internal class LeaderboardCoreManager : IInitializable, IDisposable
    {
        private readonly List<INotifyScoreSaberActivate> notifyScoreSaberActivates;
        private readonly List<INotifyLeaderboardLoad> notifyLeaderboardLoads;
        private readonly List<INotifyScoreUpload> notifyScoreUploads;

        public LeaderboardCoreManager(List<INotifyScoreSaberActivate> notifyScoreSaberActivates,
            List<INotifyLeaderboardLoad> notifyLeaderboardLoads, List<INotifyScoreUpload> notifyScoreUploads)
        {
            this.notifyScoreSaberActivates = notifyScoreSaberActivates;
            this.notifyLeaderboardLoads = notifyLeaderboardLoads;
            this.notifyScoreUploads = notifyScoreUploads;
        }

        public void Initialize()
        {
            PanelView_Show.ViewActivated += LeaderboardActivated;
            PanelView_SetIsLoaded.IsLoadedChanged += PanelViewLoadingChanged;
            PanelView_SetPrompt.ScoreUploaded += ScoreUploaded;
        }

        public void Dispose()
        {
            PanelView_Show.ViewActivated -= LeaderboardActivated;
            PanelView_SetIsLoaded.IsLoadedChanged -= PanelViewLoadingChanged;
            PanelView_SetPrompt.ScoreUploaded -= ScoreUploaded;
        }

        private void LeaderboardActivated()
        {
            foreach (var notifyScoreSaberActivate in notifyScoreSaberActivates)
            {
                notifyScoreSaberActivate.OnScoreSaberActivated();
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
