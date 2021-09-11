using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
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
    public abstract class BasicLeaderboardViewController : BSMLResourceViewController, INotifyPropertyChanged
    {
        [Inject]
        PlatformLeaderboardViewController _platformLeaderboardViewController;

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
        protected virtual bool useAroundPlayer { get => true; }
        protected virtual bool useFriends { get => false; }

        protected abstract void OnUpClicked();
        protected abstract void OnDownClicked();

        protected bool __upEnabled = true;
        protected bool __downEnabled = true;

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

        public event Action<LeaderboardScope> didSelectLeaderboardScopeEvent;

        [UIAction("cell-selected")]
        protected void OnSelectCell(SegmentedControl segmentedControl, int index)
        {
            LeaderboardScope scope = (LeaderboardScope) (index + (useAroundPlayer ? 0 : 1));
            didSelectLeaderboardScopeEvent?.Invoke(scope);
        }
        #endregion

        #region Loading
        protected bool __isLoaded = false;

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
