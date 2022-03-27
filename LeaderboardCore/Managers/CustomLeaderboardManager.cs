using LeaderboardCore.Interfaces;
using LeaderboardCore.Models;
using System.Collections.Generic;

namespace LeaderboardCore.Managers
{
    /// <summary>
    /// Class for the Leaderboard Manager. Use it to register and unregister yourself. Requires Zenject to recieve the instance (installed in menu scene).
    /// </summary>
    public class CustomLeaderboardManager
    {
        private readonly List<INotifyCustomLeaderboardsChange> notifyCustomLeaderboardsChanges;
        private readonly List<CustomLeaderboard> customLeaderboards;

        internal CustomLeaderboardManager(List<INotifyCustomLeaderboardsChange> notifyCustomLeaderboardsChanges)
        {
            this.notifyCustomLeaderboardsChanges = notifyCustomLeaderboardsChanges;
            customLeaderboards = new List<CustomLeaderboard>();
        }

        /// <summary>
        /// Registers a <see cref="CustomLeaderboard"/> to the manager.
        /// <param name="customLeaderboard"></param>
        /// </summary>
        public void Register(CustomLeaderboard customLeaderboard)
        {
            if (!customLeaderboards.Contains(customLeaderboard))
            {
                customLeaderboards.Add(customLeaderboard);
                OnLeaderboardsChanged();
            }
        }

        /// <summary>
        /// Removes a <see cref="CustomLeaderboard"/> from the manager.
        /// <param name="customLeaderboard"></param>
        /// </summary>
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
