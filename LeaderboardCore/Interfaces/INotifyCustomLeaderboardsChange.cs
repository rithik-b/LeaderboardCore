using LeaderboardCore.Models;
using System.Collections.Generic;

namespace LeaderboardCore.Interfaces
{
    interface INotifyCustomLeaderboardsChange
    {
        public void OnLeaderboardsChanged(List<CustomLeaderboard> customLeaderboards);

    }
}
