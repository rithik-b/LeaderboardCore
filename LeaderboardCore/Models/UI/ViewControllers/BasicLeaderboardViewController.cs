using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using IPA.Utilities;
using Polyglot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace LeaderboardCore.Models.UI.ViewControllers
{
    /// <summary>
    /// Abstract class for a basic leaderboard view.
    /// </summary>
    public abstract class BasicLeaderboardViewController : BSMLResourceViewController, INotifyPropertyChanged
    {
        [Inject]
        PlatformLeaderboardViewController _platformLeaderboardViewController;

        /// <summary>
        /// The side buttons next to the leaderboard, typically meant for controlling what data is displayed.
        /// </summary>
        #region enums
        public enum LeaderboardScope
        {
            Global,
            AroundPlayer,
            Friends
        }
        #endregion

        public override string ResourceName { get => "LeaderboardCore.Models.UI.Views.basic-leaderboard.bsml"; }

        public override string Content
        {
            get => Utilities.GetResourceContent(Assembly.GetAssembly(typeof(BasicLeaderboardViewController)), ResourceName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [UIComponent("leaderboard")]
        protected LeaderboardTableView table;

        #region Segmented icon control
        /// <summary>
        /// Whether or not the "Around Player" <see cref="LeaderboardScope"/> is enabled.
        /// </summary>
        protected virtual bool useAroundPlayer { get => true; }

        /// <summary>
        /// Whether or not the "Friends" <see cref="LeaderboardScope"/> is enabled.
        /// </summary>
        protected virtual bool useFriends { get => false; }

        /// <summary>
        /// Called when the up arrow next to the leaderboard is clicked.
        /// </summary>
        protected abstract void OnUpClicked();

        /// <summary>
        /// Called when the down arrow next to the leaderboard is clicked.
        /// </summary>
        protected abstract void OnDownClicked();

        protected bool __upEnabled = true;
        protected bool __downEnabled = true;

        /// <summary>
        /// Set this to change whether or not the up arrow is interactable.
        /// </summary>
        [UIValue("up-enabled")]
        protected bool upEnabled
        {
            get => __upEnabled;
            set
            {
                __upEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(upEnabled)));
            }
        }

        [UIAction("up-clicked")]
        protected void UpClicked() { OnUpClicked();  }


        /// <summary>
        /// Set this to change whether or not the down arrow is interactable.
        /// </summary>
        [UIValue("down-enabled")]
        protected bool downEnabled
        {
            get => __downEnabled;
            set
            {
                __downEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(downEnabled)));
            }
        }

        [UIAction("down-clicked")]
        protected void DownClicked() { OnDownClicked(); }

        [UIValue("cell-data")]
        protected List<IconSegmentedControl.DataItem> cellData = new List<IconSegmentedControl.DataItem>();

        /// <summary>
        /// Subscribe to this event to get notified when a <see cref="LeaderboardScope"/> is interacted with.
        /// </summary>
        public event Action<LeaderboardScope> didSelectLeaderboardScopeEvent;

        [UIAction("cell-selected")]
        protected void OnSelectCell(SegmentedControl segmentedControl, int index)
        {
            var scope = (LeaderboardScope) (index + (useAroundPlayer ? 0 : 1));
            didSelectLeaderboardScopeEvent?.Invoke(scope);
        }
        #endregion

        #region Loading
        protected bool __isLoaded = false;

        /// <summary>
        /// Set this to indicate whether or not the content is currently loaded. When false, a loading icon will display on the leaderboard.
        /// </summary>
        [UIValue("is-loaded")]
        protected virtual bool isLoaded
        {
            get => __isLoaded;
            set
            {
                __isLoaded = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(isLoaded)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(isLoading)));
            }
        }

        [UIValue("is-loading")]
        protected bool isLoading { get => !isLoaded; }
        #endregion

        public virtual void Awake()
        {
            cellData.Add(new IconSegmentedControl.DataItem(_platformLeaderboardViewController.GetField<Sprite, PlatformLeaderboardViewController>("_globalLeaderboardIcon"), Localization.Get("BUTTON_HIGHSCORES_GLOBAL")));
            if (useAroundPlayer)
            {
                cellData.Add(new IconSegmentedControl.DataItem(_platformLeaderboardViewController.GetField<Sprite, PlatformLeaderboardViewController>("_aroundPlayerLeaderboardIcon"), Localization.Get("BUTTON_HIGHSCORES_AROUND_YOU")));
            }
            if (useFriends)
            {
                cellData.Add(new IconSegmentedControl.DataItem(_platformLeaderboardViewController.GetField<Sprite, PlatformLeaderboardViewController>("_friendsLeaderboardIcon"), Localization.Get("BUTTON_HIGHSCORES_FRIENDS")));
            }
            isLoaded = false;
        }

        /// <summary>
        /// Changes the data displayed on the leaderboard to the given scores.
        /// </summary>
        /// <param name="scores">A list of ScoreDatas to populate the leaderboard with. Behavior is undefined if more than 10 scores are provided. /></param>
        /// <param name="specialScorePos">The score position (0-indexed in order of the given scores) to highlight on the leaderboard. Set to -1 if you wish for no score to be highlighted.</param>
        public void SetScores(List<LeaderboardTableView.ScoreData> scores, int specialScorePos)
        {
            if (table)
            {
                table.SetScores(scores, specialScorePos);
                isLoaded = true;
            }
            else
            {
                Plugin.Log.Warn("Tried to set scores when there was no table!");
            }
        }
    }
}
