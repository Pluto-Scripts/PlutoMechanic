using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace PlutoScripts.PlutoMechanic.Commands
{
    public class CommandAdminRepair : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "adminrepair";
        public string Help => "Repairs any vehicle instantly (admin only).";
        public string Syntax => "";
        public List<string> Aliases => new List<string> { "ar" };
        public List<string> Permissions => new List<string> { PlutoMechanicPlugin.Instance.Configuration.Instance.AdminPermission };
        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            InteractableVehicle vehicle = GetVehiclePlayerIsLookingAt(player.Player);
            if (vehicle == null)
            {
                UnturnedChat.Say(player, PlutoMechanicPlugin.Instance.Translate("no_vehicle"), Color.red);
                return;
            }
            vehicle.askRepair(vehicle.asset.health);
            UnturnedChat.Say(player, PlutoMechanicPlugin.Instance.Translate("repair_success"), Color.green);
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