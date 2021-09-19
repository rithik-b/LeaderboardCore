using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using IPA.Utilities;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace LeaderboardCore.Models.UI.ViewControllers
{
    public abstract class BasicPanelViewController : BSMLResourceViewController, INotifyPropertyChanged
    {
        public override string ResourceName { get => "LeaderboardCore.Models.UI.Views.basic-panel.bsml"; }

        public override string Content
        {
            get => Utilities.GetResourceContent(Assembly.GetAssembly(typeof(BasicPanelViewController)), ResourceName);
        }

        #region events
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }
        #endregion

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
        protected virtual bool IsLogoClickable { get => false; }
        protected abstract string LogoSource { get; }
        protected virtual string LogoHoverHint { get => ""; }
        protected virtual void OnLogoClicked() {}

        [UIValue("is-logo-clickable")]
        protected bool isLogoClickable { get => IsLogoClickable; }

        [UIValue("is-logo-plain")]
        protected bool isLogoNonClickable { get => !isLogoClickable; }

        [UIValue("logo-source")]
        protected string logoSource { get => LogoSource; }

        [UIValue("logo-hoverhint")]
        protected string logoHoverHint { get => LogoHoverHint; }

        [UIAction("clicked-logo")]
        protected void ClickedLogo() { OnLogoClicked(); }
        #endregion

        #region Loading
        protected bool __isLoaded = false;

        [UIValue("is-loaded")]
        protected bool isLoaded
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

        #region RightSide
        private bool _parsedCustomBSML = false;
        protected abstract string customBSML { get; }
        protected abstract object customHost { get; }

        [UIObject("panel-custom")]
        protected GameObject panelCustom;

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
        public virtual void Parsed()
        {
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

            if (!_parsedCustomBSML)
            {
                _parsedCustomBSML = true;
                BSMLParser.instance.Parse(customBSML, panelCustom, customHost);
            }
        }
    }
}
