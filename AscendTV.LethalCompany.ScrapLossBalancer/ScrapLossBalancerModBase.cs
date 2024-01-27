using AscendTV.LethalCompany.ScrapLossBalancer.Patches;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace AscendTV.LethalCompany.ScrapLossBalancer
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class ScrapLossBalancerModBase : BaseUnityPlugin
    {
        private readonly Harmony _harmony = new Harmony(PluginInfo.GUID);

        internal static ManualLogSource? Log;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity Method")]
        private void Awake()
        {
            Configuration.Setup(Config);

            Log = Logger;
            Log.LogInfo($"Scrap Loss Balancer loaded. " +
                $"Penalty: {Configuration.PenaltyScrapPercent:0.##}% " +
                $"{Configuration.ScrapCalculationType}");

            _harmony.PatchAll(typeof(ScrapLossBalancerModBase));
            _harmony.PatchAll(typeof(RoundManagerPatch));
        }
    }
}
