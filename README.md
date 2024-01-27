# Scrap Loss Balancer

A host-only mod to balance the amount of scrap you lose after all players die. The intention is to make it feasible to continue playing after death while still having death be meaningful, unlike mods that disable scrap loss entirely.

By default, it scales by the number of players in the game, with 12.5% value lost per player.

The idea is that the more players you have, the easier survival is, and it should be less likely that everyone dies. This attempts to balance that so a single team death doesn't make the quota automatically impossible and force a `/restart`.

This should scale fine with mods such as MoreCompany, however I haven't tested it.

## Features

* Host-only. Clients can have the mod installed, it just won't do anything.
* If you install a config mod like `LethalConfig`, configuration can be changed while playing. Changes take effect the next time everyone dies.
* Configurable settings:
    * Enabled - To allow you to temporarily disable the mod for some games.
    * Scrap Penalty (%) - The amount of scrap value lost, multiplied based on the calculation type.
    * Calculation Type - Either `PerPlayer` or `TotalPercent`

Everyone will still see the "All Scrap Lost" message on death, even though it's not.

## Explanation

If calculating `PerPlayer`, then when everyone dies, the mod will multiply the Scrap Penalty % by the number of players in the game, and then remove at least that much scrap.

For example, if you have a 4 player game, with the default `12.5%` penalty, then when everyone dies it will try to remove 50% of scrap value from the ship. If you had 2000 scrap, it will remove *at least* 1000 scrap. It basically takes the largest scrap until it's close, then fills in the rest with the smallest pieces until it's over 1000. It's common you may end up a little extra if it doesn't add up exactly. So in this case, the team may end up with 995 scrap remaining on the ship.

If calculating `TotalPercent`, then the scrap penalty % is used as-is, and the number of players don't matter. In the same scenario above, the team would end up with 1750 scrap left, or a little less.

If you set the Scrap % to 0, it'll be the same as mods that disable scrap loss on death. Similarly, if the Scrap % 