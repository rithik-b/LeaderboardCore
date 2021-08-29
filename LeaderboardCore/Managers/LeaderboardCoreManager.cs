using System;
using System.Collections.Generic;
using Zenject;
using LeaderboardCore.Interfaces;
using LeaderboardCore.HarmonyPatches;

namespace LeaderboardCore.Managers
{
    internal class LeaderboardCoreManager : IInitializable, IDisposable
    {
        private readonly CustomLeaderboardManager customLeaderboardManager;
        private readonly LevelCollectionViewController levelCollectionViewController;
        private readonly List<IPreviewBeatmapLevelUpdater> previewBeatmapLevelUpdaters;
        private readonly List<INotifyLeaderboardActivate> notifyLeaderboardActivates;
        private readonly List<INotifyLeaderboardLoad> notifyLeaderboardLoads;

        public LeaderboardCoreManager(CustomLeaderboardManager customLeaderboardManager, LevelCollectionViewController levelCollectionViewController,
            List<IPreviewBeatmapLevelUpdater> previewBeatmapLevelUpdaters, List<INotifyLeaderboardActivate> notifyLeaderboardActivates, List<INotifyLeaderboardLoad> notifyLeaderboardLoads)
        {
            this.customLeaderboardManager = customLeaderboardManager;
            this.levelCollectionViewController = levelCollectionViewController;
            this.previewBeatmapLevelUpdaters = previewBeatmapLevelUpdaters;
            this.notifyLeaderboardActivates = notifyLeaderboardActivates;
            this.notifyLeaderboardLoads = notifyLeaderboardLoads;
        }

        public void Initialize()
        {
            CustomLeaderboardManager.instance = customLeaderboardManager;

            levelCollectionViewController.didSelectLevelEvent += LevelCollectionViewController_didSelectLevelEvent;
            PanelView_Show.ViewActivated += PlatformLeaderboardViewController_didActivateEvent;
            PanelView_SetIsLoaded.IsLoadedChanged += PanelView_SetIsLoaded_IsLoadedChanged;
        }

        public void Dispose()
        {
            CustomLeaderboardManager.instance = null;

            levelCollectionViewController.didSelectLevelEvent -= LevelCollectionViewController_didSelectLevelEvent;
            PanelView_Show.ViewActivated -= PlatformLeaderboardViewController_didActivateEvent;
            PanelView_SetIsLoaded.IsLoadedChanged -= PanelView_SetIsLoaded_IsLoadedChanged;
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
    }
}
