namespace LeaderboardCore.Interfaces
{
    /// <summary>
    /// Implement this interface to be notified if ScoreSaber just uploaded a score.
    /// </summary>
    public interface INotifyScoreUpload
    {
        /// <summary>
        /// Called when a score is uploaded.
        /// </summary>
        public void OnScoreUploaded();
    }
}
