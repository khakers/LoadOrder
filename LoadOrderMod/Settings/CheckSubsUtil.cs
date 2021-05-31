namespace LoadOrderMod.Settings {
    extern alias Injections;
    using Inject = Injections.LoadOrderInjections.Injections;
    using SteamUtilities = Injections.LoadOrderInjections.SteamUtilities;
    using ColossalFramework.PlatformServices;
    using KianCommons;

    public static class CheckSubsUtil {
        static void RegisterEvent() {
            PlatformService.workshop.eventUGCRequestUGCDetailsCompleted -= OnUGCRequestUGCDetailsCompleted;
            PlatformService.workshop.eventUGCRequestUGCDetailsCompleted += OnUGCRequestUGCDetailsCompleted;
        }

        public static void EnsureAll() {
            RegisterEvent();
            Log.Info("EnsureAll called ...", true);
            SteamUtilities.EnsureAll();
        }

        public static void OnUGCRequestUGCDetailsCompleted(UGCDetails result, bool ioError) {
            // called after RequestItemDetails
            //Log.Debug($"OnUGCRequestUGCDetailsCompleted(" +
            //    $"result:{result.ToSTR2()}, " +
            //    $"ioError:{ioError})");
            bool good = SteamUtilities.IsUGCUpToDate(result, out string reason);
            if(!good) {
                Log.Info($"[WARNING!] subscribed item not installed properly:" +
                    $"{result.publishedFileId} {result.title}\n" +
                    $"reason={reason}. " +
                    $"try reinstalling the item.", true);
            } else {
                Log.Debug($"subscribed item is good:{result.publishedFileId} {result.title}", false);
            }
        }
    }
}