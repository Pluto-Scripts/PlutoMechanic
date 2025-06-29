using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Rocket.API.Collections;
using Rocket.Unturned.Chat;
using Rocket.API;
using System.Collections;
using System.Collections.Generic;
using Logger = Rocket.Core.Logging.Logger;
using System.Numerics;

namespace PlutoScripts.PlutoMechanic
{
    public class PlutoMechanicPlugin : RocketPlugin<PlutoMechanicConfiguration>
    {
        public static PlutoMechanicPlugin Instance { get; private set; }
        private HashSet<InteractableVehicle> vehiclesBeingRepaired = new HashSet<InteractableVehicle>();
        protected override void Load()
        {
            Instance = this;
            Logger.Log($"{Name} {Assembly.GetName().Version.ToString(3)} has been loaded!");
            Logger.Log("Made By PlutoScripts");
            VehicleManager.onDamageVehicleRequested += OnDamageVehicleRequested;
            VehicleManager.onRepairVehicleRequested += OnRepairVehicleRequested;
            VehicleManager.onEnterVehicleRequested += OnEnterVehicleRequested;
        }
        protected override void Unload()
        {
            Logger.Log($"{Name} has been unloaded!");
            Logger.Log("Made By PlutoScripts");
            VehicleManager.onDamageVehicleRequested -= OnDamageVehicleRequested;
            VehicleManager.onRepairVehicleRequested -= OnRepairVehicleRequested;
            VehicleManager.onEnterVehicleRequested -= OnEnterVehicleRequested;
            vehiclesBeingRepaired.Clear();
        }
        private void OnDamageVehicleRequested(CSteamID instigator, InteractableVehicle vehicle, ref ushort pendingTotalDamage, ref bool canRepair, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            if (vehicle.health <= pendingTotalDamage)
            {
                UnturnedPlayer player = UnturnedPlayer.FromCSteamID(instigator);
                if (player != null)
                    UnturnedChat.Say(player, Translate("vehicle_disabled_broadcast", vehicle.asset.vehicleName), Color.red);
                if (vehicle.health != 1)
                    vehicle.health = 1;
                shouldAllow = false;
            }
        }
        private void OnEnterVehicleRequested(Player player, InteractableVehicle vehicle, ref bool shouldAllow)
        {
            if (vehicle.health <= 1)
            {
                UnturnedPlayer uplayer = UnturnedPlayer.FromPlayer(player);
                UnturnedChat.Say(uplayer, Translate("vehicle_disabled"), Color.red);
                shouldAllow = false;
            }
        }
        private void OnRepairVehicleRequested(CSteamID instigatorSteamID, InteractableVehicle vehicle, ref ushort pendingTotalHealing, ref bool shouldAllow)
        {
            var unturnedPlayer = UnturnedPlayer.FromCSteamID(instigatorSteamID);
            if (unturnedPlayer == null) return;
            var equippedItemAsset = unturnedPlayer.Player.equipment.asset;
            if (!Configuration.Instance.EnableRepairItem || equippedItemAsset == null || equippedItemAsset.id != Configuration.Instance.RepairItemId)
            {
                UnturnedChat.Say(unturnedPlayer, Translate("invalid_repair_item"), Color.red);
                shouldAllow = false;
                return;
            }
            if (vehicle == null || vehicle.isDead || !(vehicle.isLocked || vehicle.health <= 1))
            {
                UnturnedChat.Say(unturnedPlayer, Translate("vehicle_not_disabled"), Color.red);
                shouldAllow = false;
                return;
            }
            if (!unturnedPlayer.HasPermission(Configuration.Instance.MechanicPermission))
            {
                UnturnedChat.Say(unturnedPlayer, Translate("no_permission_item"), Color.red);
                shouldAllow = false;
                return;
            }
            if (vehiclesBeingRepaired.Contains(vehicle))
            {
                UnturnedChat.Say(unturnedPlayer, Translate("vehicle_already_being_repaired"), Color.red);
                shouldAllow = false;
                return;
            }
            shouldAllow = false;
            unturnedPlayer.Player.StartCoroutine(RepairVehicleWithDelay(unturnedPlayer, vehicle));
        }
        private IEnumerator RepairVehicleWithDelay(UnturnedPlayer player, InteractableVehicle vehicle)
        {
            vehiclesBeingRepaired.Add(vehicle);
            UnturnedChat.Say(player, Translate("repair_in_progress", vehicle.asset.vehicleName, Configuration.Instance.RepairDurationSeconds), Color.yellow);
            yield return new WaitForSeconds(Configuration.Instance.RepairDurationSeconds);
            if (vehicle == null || vehicle.isDead)
            {
                UnturnedChat.Say(player, Translate("vehicle_invalid"), Color.red);
                vehiclesBeingRepaired.Remove(vehicle);
                yield break;
            }
            vehicle.askRepair(vehicle.asset.health);
            UnturnedChat.Say(player, Translate("repair_success"), Color.green);
            player.Player.skills.askAward(Configuration.Instance.RepairPayout);
            UnturnedChat.Say(player, Translate("repair_payout", Configuration.Instance.RepairPayout), Color.green);
            vehiclesBeingRepaired.Remove(vehicle);
        }
        public override TranslationList DefaultTranslations => new TranslationList
        {
            { "no_permission", "You do not have permission to use this command!" },
            { "no_permission_item", "You do not have permission to use this item!" },
            { "no_vehicle", "You must be looking at a vehicle!" },
            { "vehicle_not_disabled", "This vehicle is not disabled!" },
            { "command_disabled", "Repair Command is currently not enabled!" },
            { "repair_success", "Vehicle repaired successfully!" },
            { "vehicle_disabled", "This vehicle is disabled and cannot be used until repaired!" },
            { "vehicle_disabled_broadcast", "Vehicle {0} is disabled and requires repair by a mechanic!" },
            { "repair_in_progress", "Repairing {0}, please wait {1} seconds..." },
            { "vehicle_invalid", "The vehicle is no longer valid!" },
            { "repair_payout", "You earned {0} for the repair job!" },
            { "invalid_repair_item", "You must use the correct repair tool to repair vehicles!" },
            { "vehicle_already_being_repaired", "This vehicle is already being repaired!" }
        };
    }
}