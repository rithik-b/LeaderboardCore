using LeaderboardCore.UI.ViewControllers;
using Zenject;
using SiraUtil;
using LeaderboardCore.Managers;

namespace LeaderboardCore.Installers
{
    internal class LeaderboardCoreMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LeaderboardNavigationButtonsController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesTo<LeaderboardCoreManager>().AsSingle();
        }
    }
}
