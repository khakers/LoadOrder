namespace LoadOrderMod
{
    using System;
    using ICities;
    using KianCommons;
    using System.Diagnostics;
    using UnityEngine;
    using ColossalFramework.IO;
    using ColossalFramework.Plugins;
    using ColossalFramework.PlatformServices;
    using System.Linq;
    using System.IO;

    public class LoadOrderMod : IUserMod {
        public static Version ModVersion => typeof(LoadOrderMod).Assembly.GetName().Version;
        public static string VersionString => ModVersion.ToString(2);
        public string Name => "Load Order Mod " + VersionString;
        public string Description => "use LoadOrderTool.exe to manage the order in which mods are loaded.";
        public static string HARMONY_ID = "CS.Kian.LoadOrder";

        //static LoadOrderMod() => Log.Debug("Static Ctor "   + Environment.StackTrace);
        //public LoadOrderMod() => Log.Debug("Instance Ctor " + Environment.StackTrace);

        public void OnEnabled() {
            Log.Debug("Testing StackTrace:\n" + new StackTrace(true).ToString(), copyToGameLog: false);
            //KianCommons.UI.TextureUtil.EmbededResources = false;
            //HelpersExtensions.VERBOSE = false;
            //HarmonyUtil.InstallHarmony(HARMONY_ID);
            //foreach(var p in ColossalFramework.Plugins.PluginManager.instance.GetPluginsInfo()) {
            //    string savedKey = p.name + p.modPath.GetHashCode().ToString() + ".enabled";
            //    Log.Debug($"plugin info: savedKey={savedKey} cachedName={p.name} modPath={p.modPath}");
            //}
            LoadOrderCache data = new LoadOrderCache { GamePath = DataLocation.applicationBase };

            var plugin = PluginManager.instance.GetPluginsInfo()
                 .FirstOrDefault(_p => _p.publishedFileID != PublishedFileId.invalid);
            if (plugin?.modPath is string path) {
                data.WorkShopContentPath = Path.GetDirectoryName(path); // get parent directory.
            }

            data.Serialize(DataLocation.localApplicationData);
        }

        public void OnDisabled() {
            //HarmonyUtil.UninstallHarmony(HARMONY_ID);
        }

        // public void OnSettingsUI(UIHelperBase helper) {
        //    GUI.Settings.OnSettingsUI(helper);
        // }
    }
}
