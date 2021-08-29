using BeatSaberMarkupLanguage.FloatingScreen;
using HMUI;
using UnityEngine;

namespace LeaderboardCore.Models
{
    public abstract class CustomLeaderboard
    {
        protected abstract ViewController panelViewController { get; }
        protected abstract ViewController leaderboardViewController { get; }

        public void Show(FloatingScreen panelScreen, Vector3 leaderboardPosition, PlatformLeaderboardViewController platformLeaderboardViewController)
        {
            panelScreen.SetRootViewController(panelViewController, ViewController.AnimationType.None);
            leaderboardViewController.transform.SetParent(platformLeaderboardViewController.transform);
            leaderboardViewController.transform.localPosition = leaderboardPosition;
            leaderboardViewController.transform.localScale = Vector3.one;
            leaderboardViewController.__Activate(false, false);
        }

        public void Hide(FloatingScreen panelScreen)
        {
            panelScreen.SetRootViewController(null, ViewController.AnimationType.None);
            leaderboardViewController.__Deactivate(false, true, false);
        }
    }
}
