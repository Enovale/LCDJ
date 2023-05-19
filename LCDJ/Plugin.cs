using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

namespace LCDJ
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        public static ManualLogSource PluginLog;

        public Plugin()
        {
            PluginLog = Log;
        }

        public override void Load()
        {
            // Plugin startup logic
            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

            Initialize();

            //var harmony = new Harmony($"com.enovale.{MyPluginInfo.PLUGIN_GUID}");
        }

        private void Initialize()
        {
            PluginBootstrap.Setup();
        }
    }
}