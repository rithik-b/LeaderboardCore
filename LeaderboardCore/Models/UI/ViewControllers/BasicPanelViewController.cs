using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using IPA.Utilities;
using System.Reflection;
using UnityEngine;

namespace LeaderboardCore.Models.UI.ViewControllers
{
    public abstract class BasicPanelViewController : BSMLResourceViewController
    {
        public override string ResourceName { get => "LeaderboardCore.Models.UI.Views.basic-panel.bsml"; }

        public override string Content
        {
            get => Utilities.GetResourceContent(Assembly.GetAssembly(typeof(BasicPanelViewController)), ResourceName);
        }

        #region UI Components
        [UIComponent("outer")]
        protected Backgroundable outer;

        [UIComponent("separator")]
        protected ImageView separator;

        [UIComponent("clickable-logo")]
        protected ClickableImage clickableLogo;

        [UIComponent("plain-logo")]
        protected ImageView plainLogo;
        #endregion

        #region Logo
        protected abstract bool IsLogoClickable { get; }
        protected abstract string LogoSource { get; }
        protected abstract string LogoHoverHint { get; }

        [UIValue("is-logo-clickable")]
        protected bool isLogoClickable { get => IsLogoClickable; }

        [UIValue("is-logo-plain")]
        protected bool isLogoNonClickable { get => !isLogoClickable; }

        [UIValue("logo-source")]
        protected string logoSource { get => LogoSource; }

        [UIValue("logo-hoverhint")]
        protected string logoHoverHint { get => LogoHoverHint; }

        [UIAction("clicked-logo")]
        protected void ClickedLogo() { }
        #endregion

        #region Loading
        private bool _isLoaded = false;

        [UIValue("is-loaded")]
        protected bool isLoaded
        {
            get => _isLoaded;
            set
            {
                _isLoaded = value;
                base.NotifyPropertyChanged("is-loaded");
                base.NotifyPropertyChanged("is-loading");
            }
        }

        [UIValue("is-loading")]
        protected bool isLoading { get => !isLoaded; }
        #endregion

        #region RightSide
        protected abstract string rightSideBSML { get; }

        [UIObject("panelRightSide")]
        private GameObject panelRightSide;
        #endregion

        #region Background
        private Color _backgroundColor = Color.black;
        protected Color backgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                if (_background)
                {
                    _background.color = _backgroundColor;
                }
            }
        }
        private ImageView _background;
        #endregion

        [UIAction("#post-parse")]
        public void Parsed()
        {
            if (!panelRightSide)
            {
                Plugin.Log.Debug("hmmm");
            }
            BSMLParser.instance.Parse(rightSideBSML, panelRightSide);

            _background = outer.background as ImageView;

            // show some actual color
            _background.material = Utilities.ImageResources.NoGlowMat;
            _background.color = _backgroundColor;

            // setup gradient
            _background.color0 = Color.white;
            _background.color1 = new Color(1, 1, 1, 0);
            _background.SetField<ImageView, bool>("_gradient", true);

            // skew everything
            _background.SetField<ImageView, float>("_skew", 0.18f);
            separator.SetField<ImageView, float>("_skew", 0.18f);
            clickableLogo.SetField<ImageView, float>("_skew", 0.18f);
            plainLogo.SetField<ImageView, float>("_skew", 0.18f);
        }
    }
}
