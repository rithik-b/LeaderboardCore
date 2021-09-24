using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace LeaderboardCore.Models.UI.ViewControllers
{
    /// <summary>
    /// Absract class for a <see cref="BasicPanelViewController"/> with both a top text and a bottom text on the right side (the same as what ScoreSaber has).
    /// </summary>
    public abstract class BasicDoubleTextPanelViewController : BasicPanelViewController
    {
        protected string ExtraResourceName { get => "LeaderboardCore.Models.UI.Views.basic-panel-double-text.bsml"; }
        protected override string customBSML { get => Utilities.GetResourceContent(Assembly.GetAssembly(typeof(BasicDoubleTextPanelViewController)), ExtraResourceName); }
        protected override object customHost => this;

        #region Text
        /// <summary>
        /// Whether or not the top text should be clickable.
        /// </summary>
        protected virtual bool IsTopTextClickable { get => false; }

        /// <summary>
        /// What hover hint to display when hovering over the top text.
        /// </summary>
        protected virtual string TopHoverHint { get => ""; }

        /// <summary>
        /// Called when the top text is clicked (if <see cref="IsTopTextClickable"/> is true).
        /// </summary>
        protected virtual void OnTopClicked() { }

        /// <summary>
        /// Whether or not the bottom text should be clickable.
        /// </summary>
        protected virtual bool IsBottomTextClickable { get => false; }

        /// <summary>
        /// What hover hint to display when hovering over the bottom text.
        /// </summary>
        protected virtual string BottomHoverHint { get => ""; }

        /// <summary>
        /// Called when the bottom text is clicked (if <see cref="IsBottomTextClickable"/> is true).
        /// </summary>
        protected virtual void OnBottomClicked() { }

        #region toptext
        [UIValue("is-clickable-top")]
        protected bool isClickableTop { get => IsTopTextClickable || TopHoverHint.Length > 0; }
        [UIValue("not-is-clickable-top")]
        protected bool notIsClickableTop { get => !isClickableTop; }

        [UIAction("clicked-top")]
        protected void ClickedTop() { if (IsTopTextClickable) OnTopClicked(); }

        private string _topText;

        /// <summary>
        /// Set this to change the value of the top text.
        /// </summary>
        [UIValue("top-text")]
        protected string topText
        {   get => _topText;
            set
            {
                _topText = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(topText)));
            }
        }

        [UIValue("top-hover-hint")]
        protected string topHoverHint {get => TopHoverHint; }
        #endregion
        #region bottomtext
        [UIValue("is-clickable-bottom")]
        protected bool isClickableBottom { get => IsBottomTextClickable || BottomHoverHint.Length > 0; }
        [UIValue("not-is-clickable-bottom")]
        protected bool notIsClickableBottom { get => !isClickableBottom; }

        [UIAction("clicked-bottom")]
        protected void ClickedBottom() { if (IsBottomTextClickable) OnBottomClicked(); }

        private string _bottomText;

        /// <summary>
        /// Set this to change the value of the bottom text.
        /// </summary>
        [UIValue("bottom-text")]
        protected string bottomText
        {
            get => _bottomText;
            set
            {
                _bottomText = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(bottomText)));
            }
        }

        [UIValue("bottom-hover-hint")]
        protected string bottomHoverHint { get => BottomHoverHint; }
        #endregion
        #endregion
    }
}
