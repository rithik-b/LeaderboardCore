using IPA.Loader;
using LeaderboardCore.AffinityPatches;
using LeaderboardCore.Configuration;
using LeaderboardCore.UI.ViewControllers;
using Zenject;
using LeaderboardCore.Managers;
using LeaderboardCore.Models;

namespace LeaderboardCore.Installers
{
    internal class LeaderboardCoreMenuInstaller : Installer
    {
        private readonly PluginConfig pluginConfig;

        public LeaderboardCoreMenuInstaller(PluginConfig pluginConfig)
        {
            this.pluginConfig = pluginConfig;
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PluginConfig>().FromInstance(pluginConfig);
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
