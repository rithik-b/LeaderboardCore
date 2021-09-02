using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using LeaderboardCore.Interfaces;
using LeaderboardCore.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LeaderboardCore.UI.ViewControllers
{
    [HotReload(RelativePathToLayout = @"..\Views\LeaderboardNavigationButtons.bsml")]
    [ViewDefinition("LeaderboardCore.UI.Views.LeaderboardNavigationButtons.bsml")]
    internal class LeaderboardNavigationButtonsController : BSMLAutomaticViewController, IInitializable, IDisposable, IPreviewBeatmapLevelUpdater, INotifyLeaderboardActivate, INotifyLeaderboardLoad, INotifyCustomLeaderboardsChange
    {
        private PlatformLeaderboardViewController platformLeaderboardViewController;
        private FloatingScreen buttonsFloatingScreen;
        private FloatingScreen customPanelFloatingScreen;

        private bool leaderboardLoaded = false;
        private IPreviewBeatmapLevel selectedLevel;
        private List<CustomLeaderboard> customLeaderboards;
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
            buttonsFloatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(115f, 25f), false, Vector3.zero, Quaternion.identity);
            buttonsFloatingScreen.transform.SetParent(platformLeaderboardViewController.transform);
            buttonsFloatingScreen.transform.localPosition = new Vector3(3f, 50f);
            buttonsFloatingScreen.transform.localScale = Vector3.one;
            buttonsFloatingScreen.gameObject.SetActive(false);
            buttonsFloatingScreen.gameObject.SetActive(true);
            buttonsFloatingScreen.gameObject.name = "LeaderboardNavigationButtonsPanel";

            customPanelFloatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(100f, 25f), false, Vector3.zero, Quaternion.identity);
            customPanelFloatingScreen.transform.SetParent(platformLeaderboardViewController.transform);
            customPanelFloatingScreen.transform.localPosition = new Vector3(3f, 50f);
            customPanelFloatingScreen.transform.localScale = Vector3.one;
            customPanelFloatingScreen.gameObject.SetActive(false);
            customPanelFloatingScreen.gameObject.SetActive(true);
            customPanelFloatingScreen.gameObject.name = "CustomLeaderboardPanel";
        }

        public void Dispose()
        {
            if (buttonsFloatingScreen != null && buttonsFloatingScreen.gameObject != null)
            {
                GameObject.Destroy(buttonsFloatingScreen.gameObject);
            }
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

            OnLeaderboardLoaded(true);
        }

        public void OnLeaderboardLoaded(bool loaded)
        {
            leaderboardLoaded = loaded;

            if (loaded && selectedLevel != null && selectedLevel is CustomPreviewBeatmapLevel)
            {
                buttonsFloatingScreen.SetRootViewController(this, AnimationType.None);
            }
            else
            {
                if (currentIndex != 0)
                {
                    customLeaderboards[currentIndex - 1].Hide(customPanelFloatingScreen);
                    currentIndex = 0;
                    UnYeetSS();
                    NotifyPropertyChanged(nameof(LeftButtonActive));
                    NotifyPropertyChanged(nameof(RightButtonActive));
                }
                buttonsFloatingScreen.SetRootViewController(null, AnimationType.None);
            }
        }

        private void YeetSS()
        {
            if (containerTransform != null && ssLeaderboardElementsTransform != null && ssPanelScreenTransform != null)
            {
                containerTransform.localPosition = new Vector3(-999, -999);
                ssLeaderboardElementsTransform.localPosition = new Vector3(-999, -999);
                ssPanelScreenTransform.localPosition = new Vector3(-999, -999);
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
            customLeaderboards[currentIndex - 1].Hide(customPanelFloatingScreen);
            currentIndex--;

            if (currentIndex == 0)
            {
                UnYeetSS();
            }
            else
            {
                customLeaderboards[currentIndex - 1].Show(customPanelFloatingScreen, containerPosition, platformLeaderboardViewController);
            }

            NotifyPropertyChanged(nameof(LeftButtonActive));
            NotifyPropertyChanged(nameof(RightButtonActive));
        }

        [UIAction("right-button-click")]
        private void RightButtonClick()
        {
            if (currentIndex == 0)
            {
                YeetSS();
            }
            else
            {
                customLeaderboards[currentIndex - 1].Hide(customPanelFloatingScreen);
            }

            currentIndex++;
            customLeaderboards[currentIndex - 1].Show(customPanelFloatingScreen, containerPosition, platformLeaderboardViewController);

            NotifyPropertyChanged(nameof(LeftButtonActive));
            NotifyPropertyChanged(nameof(RightButtonActive));
        }

        public void OnLeaderboardsChanged(List<CustomLeaderboard> customLeaderboards)
        {
            if (currentIndex != 0)
            {
                this.customLeaderboards[currentIndex - 1].Hide(customPanelFloatingScreen);
                currentIndex = 0;
                UnYeetSS();
            }
            this.customLeaderboards = customLeaderboards;
            NotifyPropertyChanged(nameof(LeftButtonActive));
            NotifyPropertyChanged(nameof(RightButtonActive));
        }

        [UIValue("left-button-active")]
        private bool LeftButtonActive => currentIndex > 0;

        [UIValue("right-button-active")]
        private bool RightButtonActive => currentIndex < customLeaderboards?.Count;
    }
}
