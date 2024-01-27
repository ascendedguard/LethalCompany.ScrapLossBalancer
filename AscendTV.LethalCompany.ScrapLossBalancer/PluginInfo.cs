namespace AscendTV.LethalCompany.ScrapLossBalancer
{
    public class PluginInfo
    {
        /// <summary>
        /// Unique identifier for the mod. Must be unique ACROSS ALL MODS,
        /// and should never be changed, otherwise you'll lose config.
        /// </summary>
        public const string GUID = "ascendtv.lethal.ScrapLossBalancer";

        /// <summary>
        /// Readable Mod Name for BepInEx, r2modman, and in-game.
        /// </summary>
        public const string Name = "Scrap Loss Balancer";

        /// <summary>
        /// 3-digit version number for Thunderstore.io and this assembly.
        /// </summary>
        public const string ShortVersion = "1.0.0";

        /// <summary>
        /// Longer version number with the build number, for BepInEx and this assembly.
        /// Generally fine to just leave this zero and only update the ShortVersion.
        /// </summary>
        public const string Version = ShortVersion + ".0";

        /// <summary>
        /// Short name for Thunderstore.io - Usually the mod name with no spaces.
        /// </summary>
        public const string ShortName = "ScrapLossBalancer";

        /// <summary>
        /// Mod description, for Thunderstore.io.
        /// </summary>
        public const string Description = "Change team death to lose a percentage of scrap, scaled by number of players.";
    }
}
