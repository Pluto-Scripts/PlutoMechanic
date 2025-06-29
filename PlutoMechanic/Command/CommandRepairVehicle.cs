using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace PlutoScripts.PlutoMechanic.Commands
{
    public class CommandRepairVehicle : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "repairvehicle";
        public string Help => "Repairs a disabled vehicle if you are a mechanic.";
        public string Syntax => "";
        public List<string> Aliases => new List<string> { "rv" };
        public List<string> Permissions => new List<string> { PlutoMechanicPlugin.Instance.Configuration.Instance.MechanicPermission };
        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            if (!PlutoMechanicPlugin.Instance.Configuration.Instance.CommandEnabled)
            {
                UnturnedChat.Say(player, PlutoMechanicPlugin.Instance.Translate("command_disabled"), Color.red);
                return;
            }
            InteractableVehicle vehicle = GetVehiclePlayerIsLookingAt(player.Player);
            if (vehicle == null)
            {
                UnturnedChat.Say(player, PlutoMechanicPlugin.Instance.Translate("no_vehicle"), Color.red);
                return;
            }
            if (!(vehicle.isLocked || vehicle.health <= 1))
            {
                UnturnedChat.Say(player, PlutoMechanicPlugin.Instance.Translate("vehicle_not_disabled"), Color.red);
                return;
            }
            player.Player.StartCoroutine(RepairVehicleWithDelay(player, vehicle));
        }
        private IEnumerator RepairVehicleWithDelay(UnturnedPlayer player, InteractableVehicle vehicle)
        {
            UnturnedChat.Say(player, PlutoMechanicPlugin.Instance.Translate("repair_in_progress", vehicle.asset.vehicleName, PlutoMechanicPlugin.Instance.Configuration.Instance.RepairDurationSeconds), Color.yellow);
            yield return new WaitForSeconds(PlutoMechanicPlugin.Instance.Configuration.Instance.RepairDurationSeconds);
            if (vehicle == null || vehicle.isDead)
            {
                UnturnedChat.Say(player, PlutoMechanicPlugin.Instance.Translate("vehicle_invalid"), Color.red);
                yield break;
            }
            vehicle.askRepair(vehicle.asset.health);
            UnturnedChat.Say(player, PlutoMechanicPlugin.Instance.Translate("repair_success"), Color.green);
            player.Player.skills.askAward(PlutoMechanicPlugin.Instance.Configuration.Instance.RepairPayout);
            UnturnedChat.Say(player, PlutoMechanicPlugin.Instance.Translate("repair_payout", PlutoMechanicPlugin.Instance.Configuration.Instance.RepairPayout), Color.green);
        }
        private InteractableVehicle GetVehiclePlayerIsLookingAt(Player player)
        {
            if (Physics.Raycast(player.look.aim.position, player.look.aim.forward, out RaycastHit hit, 10f, RayMasks.VEHICLE))
            {
                return hit.transform.GetComponent<InteractableVehicle>();
            }
            return null;
        }
    }
}