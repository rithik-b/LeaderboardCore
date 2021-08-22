using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using LeaderboardCore.Interfaces;
using System;
using UnityEngine;
using Zenject;

namespace LeaderboardCore.UI.ViewControllers
{
    [HotReload(RelativePathToLayout = @"..\Views\LeaderboardNavigationButtons.bsml")]
    [ViewDefinition("LeaderboardCore.UI.Views.LeaderboardNavigationButtons.bsml")]
    internal class LeaderboardNavigationButtonsController : BSMLAutomaticViewController, IInitializable, IDisposable, IPreviewBeatmapLevelUpdater, INotifyLeaderboardActivate, INotifyLeaderboardLoad
    {
        private PlatformLeaderboardViewController platformLeaderboardViewController;
        private FloatingScreen floatingScreen;

        private bool leaderboardLoaded = false;
        private IPreviewBeatmapLevel selectedLevel;
        private int currentIndex = 0;

        private Transform containerTransform;
        private Vector3 containerPosition;

        private Transform ssLeaderboardElementsTransform;
        private Vector3 ssLeaderboardElementsPosition;

        private Transform ssPanelScreenTransform;
        private Vector3 ssPanelScreenPosition;

        [Inject]
        public void Construct(PlatformLeaderboardViewController platformLeaderboardViewController)
        {
            this.platformLeaderboardViewController = platformLeaderboardViewController;
        }

        public void Initialize()
        {
            floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(115f, 25f), false, Vector3.zero, Quaternion.identity);
            floatingScreen.transform.SetParent(platformLeaderboardViewController.transform);
            floatingScreen.transform.localPosition = new Vector3(3f, 50f);
            floatingScreen.transform.localScale = Vector3.one;
            floatingScreen.gameObject.SetActive(false);
            floatingScreen.gameObject.SetActive(true);
            floatingScreen.gameObject.name = "LeaderboardNavigationButtonsPanel";
        }

        public void Dispose()
        {
            GameObject.Destroy(floatingScreen.gameObject);
        }

        public void PreviewBeatmapLevelUpdated(IPreviewBeatmapLevel beatmapLevel)
        {
            selectedLevel = beatmapLevel;
            OnLeaderboardLoaded(leaderboardLoaded);
        }

        public void OnLeaderboardActivated()
        {
            if (containerTransform == null)
            {
                containerTransform = platformLeaderboardViewController.transform.Find("Container");
                containerPosition = containerTransform.localPosition;
            }

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

            OnLeaderboardLoaded(leaderboardLoaded);
        }

        public void OnLeaderboardLoaded(bool loaded)
        {
            leaderboardLoaded = loaded;

            if (loaded && selectedLevel != null && selectedLevel is CustomPreviewBeatmapLevel)
            {
                floatingScreen.SetRootViewController(this, AnimationType.In);
            }
            else
            {
                floatingScreen.SetRootViewController(null, AnimationType.None);
            }

            currentIndex = 1;
            LeftButtonClick();
        }

        private void YeetSS()
        {
            if (containerTransform != null && ssLeaderboardElementsTransform != null && ssPanelScreenTransform != null)
            {
                containerTransform.localPosition = new Vector3(999, 999);
                ssLeaderboardElementsTransform.localPosition = new Vector3(999, 999);
                ssPanelScreenTransform.localPosition = new Vector3(999, 999);
            }
        }

        private void UnYeetSS()
        {
            if (containerTransform != null && ssLeaderboardElementsTransform != null && ssPanelScreenTransform != null)
            {
                containerTransform.localPosition = containerPosition;
                ssLeaderboardElementsTransform.localPosition = ssLeaderboardElementsPosition;
                ssPanelScreenTransform.localPosition = ssPanelScreenPosition;
            }
        }

        [UIAction("left-button-click")]
        private void LeftButtonClick()
        {
            currentIndex--;

            if (currentIndex == 0)
            {
                UnYeetSS();
            }

            NotifyPropertyChanged(nameof(LeftButtonActive));
            NotifyPropertyChanged(nameof(RightButtonActive));
        }

        [UIAction("right-button-click")]
        private void RightButtonClick()
        {
            currentIndex++;
            YeetSS();

            NotifyPropertyChanged(nameof(LeftButtonActive));
            NotifyPropertyChanged(nameof(RightButtonActive));
        }

        [UIValue("left-button-active")]
        private bool LeftButtonActive => currentIndex > 0;

        [UIValue("right-button-active")]
        private bool RightButtonActive => currentIndex < 1;
    }
}
