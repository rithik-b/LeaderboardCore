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

        public void Hide(FloatingScreen panelScreen)
        {
            if (panelScreen.isActiveAndEnabled)
            {
                panelScreen.SetRootViewController(null, ViewController.AnimationType.None);
            }
            else
            {
                panelViewController.gameObject.SetActive(false);
            }

            if (leaderboardViewController != null)
            {
                leaderboardViewController.__Deactivate(false, true, false);
            }
        }
    }
}
