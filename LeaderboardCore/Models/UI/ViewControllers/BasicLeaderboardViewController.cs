using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using IPA.Utilities;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace LeaderboardCore.Models.UI.ViewControllers
{
    public abstract class BasicLeaderboardViewController : BSMLResourceViewController
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

        [UIComponent("leaderboard")]
        private LeaderboardTableView table;

        #region Segmented icon control
        protected abstract bool useAroundPlayer { get; }
        protected abstract bool useFriends { get; }

        private bool _upEnabled = true;
        private bool _downEnabled = true;

        [UIValue("up-enabled")]
        protected bool upEnabled
        {
            get => _upEnabled;
            set
            {
                _upEnabled = value;
                base.NotifyPropertyChanged("up-enabled");
            }
        }

        [UIValue("down-enabled")]
        protected bool downEnabled
        {
            get => _downEnabled;
            set
            {
                _downEnabled = value;
                base.NotifyPropertyChanged("down-enabled");
            }
        }

        [UIValue("cell-data")]
        protected List<IconSegmentedControl.DataItem> cellData = new List<IconSegmentedControl.DataItem>();

        public event Action<LeaderboardScope> didSelectLeaderboardScopeEvent;

        [UIAction("select-cell")]
        protected void OnSelectCell(SegmentedControl segmentedControl, int index)
        {
            LeaderboardScope scope = (LeaderboardScope) (index + (useAroundPlayer ? 0 : 1));
            didSelectLeaderboardScopeEvent?.Invoke(scope);
        }
        #endregion

        public void Awake()
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
        }

    public void SetScores(List<LeaderboardTableView.ScoreData> scores, int specialScorePos)
        {
            table.SetScores(scores, specialScorePos);
        }
    }
}
