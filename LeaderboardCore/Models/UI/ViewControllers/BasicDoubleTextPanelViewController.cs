using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LeaderboardCore.Models.UI.ViewControllers
{
    public abstract class BasicDoubleTextPanelViewController : BasicPanelViewController
    {
        protected string ExtraResourceName { get => "LeaderboardCore.Models.UI.Views.basic-panel-double-text.bsml"; }
        protected override string customBSML { get => Utilities.GetResourceContent(Assembly.GetAssembly(typeof(BasicDoubleTextPanelViewController)), ExtraResourceName); }
        protected override object customHost => this;

        #region Text
        protected virtual bool IsTopTextClickable { get => false; }
        protected virtual string TopHoverHint { get => ""; }
        protected virtual void OnTopClicked() { }

        protected virtual bool IsBottomTextClickable { get => false; }
        protected virtual string BottomHoverHint { get => ""; }
        protected virtual void OnBottomClicked() { }

        #region toptext
        [UIValue("is-clickable-top")]
        protected bool isClickableTop { get => IsTopTextClickable; }
        [UIValue("not-is-clickable-top")]
        protected bool notIsClickableTop { get => !isClickableTop; }

        [UIAction("clicked-top")]
        protected void ClickedTop() { OnTopClicked(); }

        private string _topText;
        [UIValue("top-text")]
        protected string topText
        {   get => _topText;
            set
            {
                _topText = value;
                base.NotifyPropertyChanged("top-text");
            }
        }

        [UIValue("top-hover-hint")]
        protected string topHoverHint {get => TopHoverHint; }
        #endregion
        #region bottomtext
        [UIValue("is-clickable-bottom")]
        protected bool isClickableBottom { get => IsBottomTextClickable; }
        [UIValue("not-is-clickable-bottom")]
        protected bool notIsClickableBottom { get => !isClickableBottom; }

        [UIAction("clicked-bottom")]
        protected void ClickedBottom() { OnBottomClicked(); }

        private string _bottomText;
        [UIValue("bottom-text")]
        protected string bottomText
        {
            get => _bottomText;
            set
            {
                _bottomText = value;
                base.NotifyPropertyChanged("bottom-text");
            }
        }

        [UIValue("bottom-hover-hint")]
        protected string bottomHoverHint { get => BottomHoverHint; }
        #endregion
        #endregion
    }
}
