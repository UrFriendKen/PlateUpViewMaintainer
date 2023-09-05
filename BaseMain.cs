using HarmonyLib;
using KitchenMods;
using System.Reflection;
using UnityEngine;

namespace KitchenViewMaintainer
{
    public abstract class BaseMain : IModInitializer
    {
        private readonly string ModGuid;
        private readonly string ModName;
        private readonly string ModVersion;

        Harmony _harmony;

        private bool _isPostActivating = false;

        public BaseMain(string modGuid, string modName, string modVersion, Assembly assembly)
        {
            ModGuid = modGuid;
            ModName = modName;
            ModVersion = modVersion;

            _harmony = new Harmony(modGuid);
            _harmony.PatchAll(assembly);
        }

        public void PostActivate(Mod mod)
        {
            _isPostActivating = true;
            Debug.LogWarning($"{ModGuid} v{ModVersion} in use!");
            OnPostActivate(mod);
            _isPostActivating = false;
        }

        public abstract void OnPostActivate(Mod mod);

        public virtual void PreInject()
        {
        }

        public virtual void PostInject()
        {
        }

        #region Logging
        private void LogInfo(string _log) { Debug.Log($"[{ModName}] " + _log); }
        private void LogWarning(string _log) { Debug.LogWarning($"[{ModName}] " + _log); }
        private void LogError(string _log) { Debug.LogError($"[{ModName}] " + _log); }
        private void LogInfo(object _log) { LogInfo(_log.ToString()); }
        private void LogWarning(object _log) { LogWarning(_log.ToString()); }
        private void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
