using LeaderboardCore.Interfaces;
using LeaderboardCore.Models;
using System.Collections.Generic;

namespace LeaderboardCore.Managers
{
    public class CustomLeaderboardManager
    {
        public static CustomLeaderboardManager instance;
        private readonly List<INotifyCustomLeaderboardsChange> notifyCustomLeaderboardsChanges;
        internal readonly List<CustomLeaderboard> customLeaderboards;

        internal CustomLeaderboardManager(List<INotifyCustomLeaderboardsChange> notifyCustomLeaderboardsChanges)
        {
            this.notifyCustomLeaderboardsChanges = notifyCustomLeaderboardsChanges;
            customLeaderboards = new List<CustomLeaderboard>();
        }

        public void Register(CustomLeaderboard customLeaderboard)
        {
            customLeaderboards.Add(customLeaderboard);
            OnLeaderboardsChanged();
        }

        public void Unregister(CustomLeaderboard customLeaderboard)
        {
            customLeaderboards.Remove(customLeaderboard);
            OnLeaderboardsChanged();
        }

        private void OnLeaderboardsChanged()
        {
            foreach (var notifyCustomLeaderboardsChange in notifyCustomLeaderboardsChanges)
            {
                notifyCustomLeaderboardsChange.OnLeaderboardsChanged(customLeaderboards);
            }
        }
    }
}
