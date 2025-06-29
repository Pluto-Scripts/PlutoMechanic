using Rocket.API;
using System.Collections.Generic;

namespace PlutoScripts.PlutoMechanic
{
    public class PlutoMechanicConfiguration : IRocketPluginConfiguration
    {
        public bool EnableRepairItem;
        public ushort RepairItemId;
        public uint RepairPayout;
        public string MechanicPermission;
        public string AdminPermission;
        public bool CommandEnabled;
        public float RepairDurationSeconds;

        public void LoadDefaults()
        {
            EnableRepairItem = true;
            RepairItemId = 76;
            RepairPayout = 100;
            MechanicPermission = "plutomechanic.mechanic";
            AdminPermission = "plutomechanic.admin";
            CommandEnabled = true;
            RepairDurationSeconds = 5.0f;
        }
    }
}