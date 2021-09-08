using BeatSaberMarkupLanguage.FloatingScreen;
using HMUI;
using UnityEngine;

namespace LeaderboardCore.Models
{
    /// <summary>
    /// Abstract class for the Custom Leaderboard. Must provide the panel and leaderboard views.
    /// </summary>
    public abstract class CustomLeaderboard
    {
        /// <summary>
        /// The ViewController for the leaderboard's panel.
        /// </summary>
        protected abstract ViewController panelViewController { get; }

        /// <summary>
        /// The ViewController for the leaderboard itself.
        /// </summary>
        protected abstract ViewController leaderboardViewController { get; }

        internal void Show(FloatingScreen panelScreen, Vector3 leaderboardPosition, PlatformLeaderboardViewController platformLeaderboardViewController)
        {
            if (panelViewController != null)
            {
                panelScreen.SetRootViewController(panelViewController, ViewController.AnimationType.None);
                if (!panelViewController.isActiveAndEnabled)
                {
                    panelViewController.gameObject.SetActive(true);
                }
            }
            else
            {
                panelScreen.SetRootViewController(null, ViewController.AnimationType.None);
            }

            if (leaderboardViewController != null)
            {
                leaderboardViewController.transform.localPosition = leaderboardPosition;

                if (leaderboardViewController.screen == null)
                {
                    leaderboardViewController.__Init(platformLeaderboardViewController.screen, platformLeaderboardViewController, null);
                }

                leaderboardViewController.__Activate(false, false);
                leaderboardViewController.transform.SetParent(platformLeaderboardViewController.transform);
            }
        }

        internal void Hide(FloatingScreen panelScreen)
        {
            if (panelScreen != null)
            {
                if (panelScreen.isActiveAndEnabled)
                {
                    panelScreen.SetRootViewController(null, ViewController.AnimationType.None);
                }
                else
                {
                    panelViewController.gameObject.SetActive(false);
                }
            }

            if (leaderboardViewController != null)
            {
                leaderboardViewController.__Deactivate(false, true, false);
            }
        }
    }
}
