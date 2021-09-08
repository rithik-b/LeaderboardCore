namespace LeaderboardCore.Interfaces
{
    /// <summary>
    /// Implement this interface to be notified of ScoreSaber's leaderboard getting loaded/failing to load.
    /// </summary>
    public interface INotifyLeaderboardLoad
    {
        /// <summary>
        /// Called when ScoreSaber's loaded state changes.
        /// <param name="loaded"></param>
        /// </summary>
        public void OnLeaderboardLoaded(bool loaded);
    }
}
