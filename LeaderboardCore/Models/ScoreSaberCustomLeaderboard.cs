using System.Reflection;
using LeaderboardCore.Interfaces;
using UnityEngine;
using Zenject;

namespace LeaderboardCore.Models
{
    internal class ScoreSaberCustomLeaderboard :  INotifyScoreSaberActivate
    {
        [Inject]
        private readonly PlatformLeaderboardViewController platformLeaderboardViewController = null!;

        private Transform ssLeaderboardElementsTransform;
        private Vector3 ssLeaderboardElementsPosition;

        private Transform ssPanelScreenTransform;
        private Vector3 ssPanelScreenPosition;
        
        public void OnScoreSaberActivated()
        {
            if (ssLeaderboardElementsTransform == null)
            {
                ssLeaderboardElementsTransform = platformLeaderboardViewController.transform.Find("ScoreSaberLeaderboardElements");
                ssLeaderboardElementsPosition = ssLeaderboardElementsTransform.localPosition;
            }

            if (ssPanelScreenTransform == null)
            {
                ssPanelScreenTransform = platformLeaderboardViewController.transform.Find("ScoreSaberPanelScreen");
                ssPanelScreenPosition = ssPanelScreenTransform.localPosition;
            }
        }
        
        public void YeetSS()
        {
            if (ssLeaderboardElementsTransform != null && ssPanelScreenTransform != null)
            {
                ssLeaderboardElementsTransform.localPosition = new Vector3(-999, -999);
                ssPanelScreenTransform.localPosition = new Vector3(-999, -999);
            }
        }

        public void UnYeetSS()
        {
            if (ssLeaderboardElementsTransform != null && ssPanelScreenTransform != null)
            {
                ssLeaderboardElementsTransform.localPosition = ssLeaderboardElementsPosition;
                ssPanelScreenTransform.localPosition = ssPanelScreenPosition;
            }
        }
    }
}