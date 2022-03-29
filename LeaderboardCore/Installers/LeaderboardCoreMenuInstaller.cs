using IPA.Loader;
using LeaderboardCore.AffinityPatches;
using LeaderboardCore.UI.ViewControllers;
using Zenject;
using LeaderboardCore.Managers;
using LeaderboardCore.Models;

namespace LeaderboardCore.Installers
{
    internal class LeaderboardCoreMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LeaderboardNavigationButtonsController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesTo<LeaderboardCoreManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<CustomLeaderboardManager>().AsSingle();

            if (PluginManager.GetPluginFromId("ScoreSaber") != null)
            {
                Container.BindInterfacesAndSelfTo<ScoreSaberCustomLeaderboard>().AsSingle();
            }

            Container.BindInterfacesTo<LeaderboardSetDataPatch>().AsSingle();
        }
    }
}
