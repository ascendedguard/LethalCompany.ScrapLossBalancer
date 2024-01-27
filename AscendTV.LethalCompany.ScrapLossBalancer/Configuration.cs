using BepInEx.Configuration;

namespace AscendTV.LethalCompany.ScrapLossBalancer
{
    public static class Configuration
    {
        private static ConfigEntry<float>? _penaltyScrapPercent;
        private static ConfigEntry<ScrapCalculationType>? _scrapCalculationType;
        private static ConfigEntry<bool>? _modEnabled;

        public static float PenaltyScrapPercent => _penaltyScrapPercent?.Value ?? 12.5f;
        public static ScrapCalculationType ScrapCalculationType => _scrapCalculationType?.Value ?? ScrapCalculationType.PerPlayer;
        public static bool ModEnabled => _modEnabled?.Value ?? false;

        public static void Setup(ConfigFile config)
        {
            _modEnabled = config.Bind(
                "General",
                "Enabled",
                defaultValue: true,
                "Enables the mod. When enabled, reduces scrap loss when all players die.\r\n\r\nNo restart required. Host only."
            );

            _penaltyScrapPercent = config.Bind(
                "General",
                "Scrap Penalty (%)",
                defaultValue: 12.5f,
                new ConfigDescription(
                    "Percent of scrap value to remove from the ship when all players die.\r\n\r\nExample: If you had 2000 scrap on ship with 3 players, 25% per player, death would result in around 500 scrap remaining.\r\n\r\nNo restart required. Takes effect on next full wipe.",
                    new AcceptableValueRange<float>(0.0f, 100.0f)
            ));

            _scrapCalculationType = config.Bind(
                "General",
                "Calculation Type",
                defaultValue: ScrapCalculationType.PerPlayer,
                "Describes how the penalty is calculated, either per player or as a static value.\r\nValid options: PerPlayer, TotalPercent\r\n\r\nNo restart required. Takes effect on next full wipe."
            );
        }
    }
}
