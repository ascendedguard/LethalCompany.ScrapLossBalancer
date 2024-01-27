using System.ComponentModel;

namespace AscendTV.LethalCompany.ScrapLossBalancer
{
    public enum ScrapCalculationType
    {
        /// <summary>
        /// Penalty percent is multiplied by the number of players.
        /// </summary>
        [Description("Per Player")]
        PerPlayer = 0,

        /// <summary>
        /// Penalty percent is static, regardless of how many players there are.
        /// </summary>
        [Description("Total Percent")]
        TotalPercent
    }
}
