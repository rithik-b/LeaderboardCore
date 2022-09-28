using LeaderboardCore.Interfaces;
using LeaderboardCore.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace LeaderboardCore.Managers
{
    /// <summary>
    /// Class for the Leaderboard Manager. Use it to register and unregister yourself. Requires Zenject to recieve the instance (installed in menu scene).
    /// </summary>
    public class CustomLeaderboardManager
    {
        private readonly List<INotifyCustomLeaderboardsChange> notifyCustomLeaderboardsChanges;
        private readonly Dictionary<string, CustomLeaderboard> customLeaderboardsById;

        internal CustomLeaderboardManager(List<INotifyCustomLeaderboardsChange> notifyCustomLeaderboardsChanges)
        {
            this.notifyCustomLeaderboardsChanges = notifyCustomLeaderboardsChanges;
            customLeaderboardsById = new Dictionary<string, CustomLeaderboard>();
        }

        /// <summary>
        /// Registers a <see cref="CustomLeaderboard"/> to the manager.
        /// <param name="customLeaderboard"></param>
        /// </summary>
        public void Register(CustomLeaderboard customLeaderboard)
        {
            if (customLeaderboard.pluginId == null)
            {
                customLeaderboard.pluginId = Assembly.GetCallingAssembly().GetName().Name;
            }
            
            if (!customLeaderboardsById.ContainsKey(customLeaderboard.LeaderboardId))
            {
                customLeaderboardsById[customLeaderboard.LeaderboardId] = customLeaderboard;
                OnLeaderboardsChanged();
            }
        }

        /// <summary>
        /// Removes a <see cref="CustomLeaderboard"/> from the manager.
        /// <param name="customLeaderboard"></param>
        /// </summary>
        public void Unregister(CustomLeaderboard customLeaderboard)
        {
            customLeaderboardsById.Remove(customLeaderboard.LeaderboardId);
            OnLeaderboardsChanged();
        }

        private void OnLeaderboardsChanged()
        {
            foreach (var notifyCustomLeaderboardsChange in notifyCustomLeaderboardsChanges)
            {
                notifyCustomLeaderboardsChange.OnLeaderboardsChanged(customLeaderboardsById.Values, customLeaderboardsById);
            }
        }
    }
}
