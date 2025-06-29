# PlutoMechanic

# **<span style="color:darkblue">[INFO] Check out our Discord Server: discord.gg/Uu5rNSBpH6</span>**

### **Please leave a review**
**This will be updated sometimes if contacted about issues, but not always.**

## Overview
PlutoMechanic is a free plugin designed to enhance roleplay servers by adding a mechanic job with customizable repair mechanics.

## Commands
- `/repairvehicle` (`/rv`): Repairs a disabled vehicle (configurable permission).
- `/adminrepair` (`/ar`): Instantly repairs any vehicle (admin-only, configurable permission).

## Features
- 🗃️ **Mechanic Job**: Repair vehicles with configurable permissions and payouts.
- 🛠️ **Customizable Repairs**: Use a specific item (e.g., blowtorch) or commands, with configurable repair duration.
- 🛡️ **Vehicle Protection**: Prevents vehicle explosions and disables them until repaired.
- ⚔️ **Repair Protection**: Prevents people from repairing vehicles unless they have mechanic permission.
- 🥋 **Admin Tools**: Dedicated admin repair command.
- ⚙️ **Localization**: All messages customizable via Translation List.
- 🔥 **No loader, fully yours**.
- 💸 **Free**: No cost, fully featured.

## Configuration
- Enable/disable item or command repairs.
- Set repair item ID, payout amount, and repair duration. (Repair item needs to be a melee weapon with `Repair`, like a blowtorch)
- Customize permissions for mechanics and admins.

### Example Configuration
```xml
<?xml version="1.0" encoding="utf-8"?>
<PlutoMechanicConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <EnableRepairItem>true</EnableRepairItem>
  <RepairItemId>76</RepairItemId>
  <RepairPayout>100</RepairPayout>
  <MechanicPermission>plutomechanic.mechanic</MechanicPermission>
  <AdminPermission>plutomechanic.admin</AdminPermission>
  <CommandEnabled>true</CommandEnabled>
  <RepairDurationSeconds>5</RepairDurationSeconds>
</PlutoMechanicConfiguration>
```

### Translations
```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="no_permission" Value="You do not have permission to use this command!" />
  <Translation Id="no_permission_item" Value="You do not have permission to use this item!" />
  <Translation Id="no_vehicle" Value="You must be looking at a vehicle!" />
  <Translation Id="vehicle_not_disabled" Value="This vehicle is not disabled!" />
  <Translation Id="command_disabled" Value="Repair Command is currently not enabled!" />
  <Translation Id="repair_success" Value="Vehicle repaired successfully!" />
  <Translation Id="vehicle_disabled" Value="This vehicle is disabled and cannot be used until repaired!" />
  <Translation Id="vehicle_disabled_broadcast" Value="Vehicle {0} is disabled and requires repair by a mechanic!" />
  <Translation Id="repair_in_progress" Value="Repairing {0}, please wait {1} seconds..." />
  <Translation Id="vehicle_invalid" Value="The vehicle is no longer valid!" />
  <Translation Id="repair_payout" Value="You earned {0} for the repair job!" />
  <Translation Id="invalid_repair_item" Value="You must use the correct repair tool to repair vehicles!" />
  <Translation Id="vehicle_already_being_repaired" Value="This vehicle is already being repaired!" />
</Translations>
```
