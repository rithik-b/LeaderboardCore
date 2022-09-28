using LeaderboardCore.Models;
using System.Collections.Generic;

namespace LeaderboardCore.Interfaces
{
    internal interface INotifyCustomLeaderboardsChange
    {
        public void OnLeaderboardsChanged(IEnumerable<CustomLeaderboard> orderedCustomLeaderboards, Dictionary<string, CustomLeaderboard> customLeaderboardsById);

    }
}
