using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace AscendTV.LethalCompany.ScrapLossBalancer.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    public class RoundManagerPatch
    {
        /// <summary>
        /// Prevents the normal behavior of player deaths 
        /// causing all items to unspawn, by temporarily saying
        /// players aren't dead.
        /// </summary>
        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.DespawnPropsAtEndOfRound))]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.Low)]
        public static void DespawnPropsAtEndOfRoundPre(out bool __state)
        {
            // Host-only mod.
            if (!RoundManager.Instance.IsServer || !Configuration.ModEnabled)
            {
                __state = false;
                return;
            }

            __state = StartOfRound.Instance.allPlayersDead;
            StartOfRound.Instance.allPlayersDead = false;
        }

        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.DespawnPropsAtEndOfRound))]
        [HarmonyPostfix]
        [HarmonyPriority(Priority.High)]
        public static void DespawnPropsAtEndOfRoundPost(bool __state)
        {
            // Host-only mod.
            if (!RoundManager.Instance.IsServer || !Configuration.ModEnabled)
                return;

            bool allPlayersDead = __state;
            // Restore the proper dead player state
            StartOfRound.Instance.allPlayersDead = allPlayersDead;

            if (allPlayersDead)
            {
                GameObject _ship = GameObject.Find("/Environment/HangarShip");

                List<GrabbableObject> shipScrap = _ship
                    .GetComponentsInChildren<GrabbableObject>()
                    .Where(s => s.itemProperties.isScrap)
                    .ToList();

                int totalScrap = shipScrap.Sum(s => s.scrapValue);

                float targetPercent = CalculateTargetScrapPercent();
                ScrapLossBalancerModBase.Log?.LogInfo(
                    $"Attempting to remove {targetPercent * 100}% scrap value from {shipScrap.Count} scrap items.");

                int targetScrapValueToRemove = (int)Math.Round(totalScrap * targetPercent);
                List<GrabbableObject> scrapToRemove = GetScrapAtLeastTargetPercent(shipScrap, targetPercent);

                foreach (GrabbableObject scrap in scrapToRemove)
                {
                    DespawnObject(scrap);
                }
            }
        }

        private static float CalculateTargetScrapPercent()
        {
            if (Configuration.ScrapCalculationType == ScrapCalculationType.PerPlayer)
            {
                int numPlayers = StartOfRound.Instance.connectedPlayersAmount + 1;
                return Math.Clamp(numPlayers * Configuration.PenaltyScrapPercent / 100.0f, 0, 1);
            }
            else // TotalPercent
            {
                return Math.Clamp(Configuration.PenaltyScrapPercent / 100.0f, 0, 1);
            }
        }

        private static void DespawnObject(GrabbableObject item)
        {
            item.scrapPersistedThroughRounds = false;

            NetworkObject component = item.gameObject.GetComponent<NetworkObject>();
            if (component != null && component.IsSpawned)
            {
                item.gameObject.GetComponent<NetworkObject>().Despawn();
            }
            else
            {
                UnityEngine.Object.Destroy(item.gameObject);
            }

            ScrapLossBalancerModBase.Log?
                .LogDebug($"Item Destroyed: {item.name}, Value: {item.scrapValue}");
        }

        private static List<GrabbableObject> GetScrapAtLeastTargetPercent(List<GrabbableObject> allScrap, double targetPercent)
        {
            if (allScrap.Count < 2 || targetPercent >= 100.0f)
            {
                return allScrap;
            }

            int totalScrapValue = allScrap.Sum(item => item.scrapValue);
            int targetValue = (int)(totalScrapValue * targetPercent);

            List<GrabbableObject> result = new List<GrabbableObject>();
            int accumulatedValue = 0;

            // Take large items until we're close
            IOrderedEnumerable<GrabbableObject> descScrap = allScrap.OrderByDescending(item => item.scrapValue);
            foreach (GrabbableObject item in descScrap)
            {
                // Stop 1 item early, then take small items to get a little closer
                if (accumulatedValue + item.scrapValue > targetValue)
                    break;

                result.Add(item);
                accumulatedValue += item.scrapValue;
            }

            IOrderedEnumerable<GrabbableObject> ascScrap = allScrap.OrderBy(item => item.scrapValue);
            foreach (GrabbableObject? item in ascScrap)
            {
                // If we found an exact match on the first loop,
                // this will also short-circuit
                if (accumulatedValue >= targetValue)
                    break;

                // It shouldn't be possible to add the same object twice
                // assuming this is a true reverse of the desc sort
                result.Add(item);
                accumulatedValue += item.scrapValue;
            }

            ScrapLossBalancerModBase.Log?
                .LogInfo($"Removing {result.Count} items for penalty. Target value: {targetValue}, Actual: {accumulatedValue}");

            return result;
        }
    }
}
