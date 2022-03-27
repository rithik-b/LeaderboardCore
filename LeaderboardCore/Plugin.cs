using IPA;
using LeaderboardCore.Installers;
using SiraUtil.Zenject;
using System;
using System.Reflection;
using IPA.Loader;
using IPALogger = IPA.Logging.Logger;

namespace LeaderboardCore
{
    /// <summary>
    /// The LeaderboardCore plugin
    /// </summary>
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        private const string kHarmonyID = "com.github.rithik-b.LeaderboardCore";
        private static readonly HarmonyLib.Harmony harmony = new HarmonyLib.Harmony(kHarmonyID);

        internal static Plugin Instance { get; set; }
        internal static IPALogger Log { get; private set; }
        internal readonly PluginMetadata scoreSaber;

        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        [Init]
        public Plugin(IPALogger logger, Zenjector zenjector)
        {
            Instance = this;
            Log = logger;
            zenjector.Install<LeaderboardCoreMenuInstaller>(Location.Menu);

            scoreSaber = PluginManager.GetPluginFromId("ScoreSaber");
            // Since we only harmony patch on enable/disable, ScoreSaber is harmony patched regardless as long as it exists
            scoreSaber ??= PluginManager.GetDisabledPluginFromId("ScoreSaber");
        }


        #region Disableable

        /// <summary>
        /// Called when the plugin is enabled (including when the game starts if the plugin is enabled).
        /// </summary>
        [OnEnable]
        public void OnEnable()
        {
            if (scoreSaber != null)
            {
                ApplyHarmonyPatches();
            }
        }

        /// <summary>
        /// Called when the plugin is disabled and on Beat Saber quit. It is important to clean up any Harmony patches, GameObjects, and Monobehaviours here.
        /// The game should be left in a state as if the plugin was never started.
        /// Methods marked [OnDisable] must return void or Task.
        /// </summary>
        [OnDisable]
        public void OnDisable()
        {
            if (scoreSaber != null)
            {
                RemoveHarmonyPatches();
            }
        }
        
        #endregion

        #region Harmony
        /// <summary>
        /// Attempts to apply all the Harmony patches in this assembly.
        /// </summary>
        private static void ApplyHarmonyPatches()
        {
            try
            {
                Log?.Debug("Applying Harmony patches.");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Log?.Error("Error applying Harmony patches: " + ex.Message);
                Log?.Debug(ex);
            }
        }

        /// <summary>
        /// Attempts to remove all the Harmony patches that used our HarmonyId.
        /// </summary>
        private static void RemoveHarmonyPatches()
        {
            try
            {
                harmony.UnpatchSelf();
            }
            catch (Exception ex)
            {
                Log?.Error("Error removing Harmony patches: " + ex.Message);
                Log?.Debug(ex);
            }
        }
        #endregion
    }
}
