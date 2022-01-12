namespace LeaderboardCore.Interfaces
{
    /// <summary>
    /// Implement this interface to be notified when a map is selected.
    /// </summary>
    public interface INotifyLeaderboardSet
    {
        /// <summary>
        /// Called when a map has been selected on the leaderboard.
        /// <param name="difficultyBeatmap"></param>
        /// </summary>
        public void OnLeaderboardSet(IDifficultyBeatmap difficultyBeatmap);
    }
}
