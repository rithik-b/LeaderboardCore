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
        private readonly List<INotifyLeaderboardLoad> notifyLeaderboardLoads;

        public LeaderboardCoreManager(List<INotifyLeaderboardActivate> notifyLeaderboardActivates, List<INotifyLeaderboardLoad> notifyLeaderboardLoads)
        {
            this.notifyLeaderboardActivates = notifyLeaderboardActivates;
            this.notifyLeaderboardLoads = notifyLeaderboardLoads;
        }

        public void Initialize()
        {
            PanelView_Show.ViewActivated += PlatformLeaderboardViewController_didActivateEvent;
            PanelView_SetIsLoaded.IsLoadedChanged += PanelView_SetIsLoaded_IsLoadedChanged;
        }

        public void Dispose()
        {
            PanelView_Show.ViewActivated -= PlatformLeaderboardViewController_didActivateEvent;
            PanelView_SetIsLoaded.IsLoadedChanged -= PanelView_SetIsLoaded_IsLoadedChanged;
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
