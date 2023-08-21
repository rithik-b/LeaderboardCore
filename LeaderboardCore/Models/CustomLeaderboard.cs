using System;
using System.Collections;
using BeatSaberMarkupLanguage.FloatingScreen;
using HMUI;
using LeaderboardCore.Utilities;
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
        
        /// <summary>
        /// The ID for the leaderboard.
        /// Must be a unique string if the mod has multiple leaderboards.
        /// </summary>
        protected virtual string leaderboardId { get; } = "";
        
        internal string pluginId;
        
        internal string LeaderboardId => $"{pluginId}{leaderboardId}";

        internal void Show(FloatingScreen panelScreen, Vector3 leaderboardPosition, PlatformLeaderboardViewController platformLeaderboardViewController)
        {
            panelScreen.gameObject.SetActive(true);
            
            if (!panelScreen.isActiveAndEnabled)
            {
                SharedCoroutineStarter.Instance.StartCoroutine(WaitForScreen(panelScreen, leaderboardPosition,
                    platformLeaderboardViewController));
                return;
            }
            
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
        
        private IEnumerator WaitForScreen(FloatingScreen panelScreen, Vector3 leaderboardPosition,
            PlatformLeaderboardViewController platformLeaderboardViewController)
        {
            while (!panelScreen.isActiveAndEnabled)
            {
                yield return null;
            }
            Show(panelScreen, leaderboardPosition, platformLeaderboardViewController);
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

            if (leaderboardViewController != null && leaderboardViewController.isActivated)
            {
                leaderboardViewController.__Deactivate(false, true, false);
            }
        }
    }
}
