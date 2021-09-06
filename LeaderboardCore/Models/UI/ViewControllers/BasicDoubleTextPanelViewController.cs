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
        public override string ResourceName { get => "LeaderboardCore.Models.UI.Views.basic-panel-right.bsml"; }
        protected override string rightSideBSML { get => Utilities.GetResourceContent(Assembly.GetAssembly(typeof(BasicDoubleTextPanelViewController)), ResourceName); }

        #region Text
        protected abstract bool IsTopTextClickable { get; }
        protected abstract string TopHoverHint { get; }
        protected abstract bool IsBottomTextClickable { get; }
        protected abstract string BottomHoverHint { get; }

        #region toptext
        [UIValue("is-clickable-top")]
        protected bool isClickableTop { get => IsTopTextClickable; }
        [UIValue("not-is-clickable-top")]
        protected bool notIsClickableTop { get => !isClickableTop; }

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
