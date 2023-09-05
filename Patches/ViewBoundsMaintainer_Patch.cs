using HarmonyLib;
using Kitchen;
using System;
using System.Reflection;

namespace KitchenViewMaintainer.Patches
{
    public enum PlayerGroupOptions
    {
        All,
        Local,
        Remote,
        None
    }

    [HarmonyPatch]
    static class ViewBoundsMaintainer_Patch
    {
        static FieldInfo f_IsMyPlayer = typeof(PlayerView).GetField("IsMyPlayer", BindingFlags.NonPublic | BindingFlags.Instance);

        [HarmonyPatch(typeof(ViewBoundsMaintainer), nameof(ViewBoundsMaintainer.Update))]
        [HarmonyPrefix]
        static void Update_Prefix(IObjectView view, ref MaintainInViewData mvd)
        {
            if (view is PlayerView playerView && Enum.TryParse(Main.PrefManager.Get<string>(Main.PLAYERS_IN_VIEW_ID), out PlayerGroupOptions playerGroupOptions))
            {
                bool preventMaintain = false;
                object obj = f_IsMyPlayer?.GetValue(playerView);
                switch (playerGroupOptions)
                {
                    case PlayerGroupOptions.Local:
                        if (obj != null)
                        {
                            bool isMyPlayer = (bool)obj;
                            preventMaintain = !isMyPlayer;
                        }
                        break;
                    case PlayerGroupOptions.Remote:
                        if (obj != null)
                        {
                            bool isMyPlayer = (bool)obj;
                            preventMaintain = isMyPlayer;
                        }
                        break;
                    case PlayerGroupOptions.None:
                        preventMaintain = true;
                        break;
                    case PlayerGroupOptions.All:
                    default:
                        break;
                }

                if (preventMaintain)
                {
                    mvd.ShouldMaintain = false;
                }
            }
        }
    }
}
