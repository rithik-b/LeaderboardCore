using System;
using System.Collections.Generic;
using Zenject;
using LeaderboardCore.Interfaces;
using LeaderboardCore.HarmonyPatches;

namespace LeaderboardCore.Managers
{
    internal class LeaderboardCoreManager : IInitializable, IDisposable
    {
        private readonly LevelCollectionViewController levelCollectionViewController;
        private readonly List<IPreviewBeatmapLevelUpdater> previewBeatmapLevelUpdaters;
        private readonly List<INotifyLeaderboardActivate> notifyLeaderboardActivates;
        private readonly List<INotifyLeaderboardLoad> notifyLeaderboardLoads;
        private readonly List<INotifyScoreUpload> notifyScoreUploads;

        public LeaderboardCoreManager(LevelCollectionViewController levelCollectionViewController, List<IPreviewBeatmapLevelUpdater> previewBeatmapLevelUpdaters,
            List<INotifyLeaderboardActivate> notifyLeaderboardActivates, List<INotifyLeaderboardLoad> notifyLeaderboardLoads, List<INotifyScoreUpload> notifyScoreUploads)
        {
            this.levelCollectionViewController = levelCollectionViewController;
            this.previewBeatmapLevelUpdaters = previewBeatmapLevelUpdaters;
            this.notifyLeaderboardActivates = notifyLeaderboardActivates;
            this.notifyLeaderboardLoads = notifyLeaderboardLoads;
            this.notifyScoreUploads = notifyScoreUploads;
        }

        public void Initialize()
        {
            levelCollectionViewController.didSelectLevelEvent += LevelCollectionViewController_didSelectLevelEvent;
            PanelView_Show.ViewActivated += PlatformLeaderboardViewController_didActivateEvent;
            PanelView_SetIsLoaded.IsLoadedChanged += PanelView_SetIsLoaded_IsLoadedChanged;
            PanelView_SetPrompt.ScoreUploaded += PanelView_SetPrompt_ScoreUploaded;
        }

        public void Dispose()
        {
            levelCollectionViewController.didSelectLevelEvent -= LevelCollectionViewController_didSelectLevelEvent;
            PanelView_Show.ViewActivated -= PlatformLeaderboardViewController_didActivateEvent;
            PanelView_SetIsLoaded.IsLoadedChanged -= PanelView_SetIsLoaded_IsLoadedChanged;
            PanelView_SetPrompt.ScoreUploaded -= PanelView_SetPrompt_ScoreUploaded;
        }


        private void LevelCollectionViewController_didSelectLevelEvent(LevelCollectionViewController levelCollectionViewController, IPreviewBeatmapLevel level)
        {
            foreach (var previewBeatmapLevelUpdater in previewBeatmapLevelUpdaters)
            {
                previewBeatmapLevelUpdater.PreviewBeatmapLevelUpdated(level);
            }
        }

        private void PlatformLeaderboardViewController_didActivateEvent()
        {
            foreach (var notifyLeaderboardActivate in notifyLeaderboardActivates)
            {
                notifyLeaderboardActivate.OnLeaderboardActivated();
            }
        }

        private void PanelView_SetIsLoaded_IsLoadedChanged(bool loaded)
        {
            foreach (var notifyLeaderboardLoad in notifyLeaderboardLoads)
            {
                notifyLeaderboardLoad.OnLeaderboardLoaded(loaded);
            }
        }

        private void PanelView_SetPrompt_ScoreUploaded()
        {
            foreach (var notifyScoreUpload in notifyScoreUploads)
            {
                notifyScoreUpload.OnScoreUploaded();
            }
        }
    }
}
