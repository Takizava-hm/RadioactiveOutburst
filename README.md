# Radioactive Outburst Plugin for SCP:SL

A dynamic plugin that introduces periodic radioactive outbursts to SCP: Secret Laboratory, creating intense survival scenarios during gameplay.

## Features

- ☢️ **Periodic Radiation Events** - Deadly radioactive outbursts occur at configurable intervals
- 🛡️ **Protection Mechanics** - SCP-500 provides temporary radiation immunity
- 🏠 **Safe Zones** - Designated areas protect players from radiation damage
- ⚙️ **Fully Configurable** - Adjust timing, damage, messages, and safe zones
- 📊 **Debug Mode** - Additional logging for troubleshooting

## Configuration

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| IsEnabled | bool | true | Toggles the entire plugin |
| OutburstInterval | float | 300 | Seconds between radiation events |
| OutburstDuration | float | 60 | How long radiation lasts (seconds) |
| DamagePerTick | float | 5 | Radiation damage per second |
| OutburstStartMessage | string | `<color=red>Radioactive outburst has begun!</color>` | Broadcast when event starts |
| OutburstEndMessage | string | `<color=green>Radioactive outburst has ended.</color>` | Broadcast when event ends |
| SafeZones | List<ZoneType> | LightContainment, Entrance, Surface | Areas protected from radiation |
| Debug | bool | false | Enable detailed logging |

## How It Works

1. **Event Cycle**:
   - After round start, the plugin begins counting down to the first outburst
   - When triggered, all players receive warning messages
   - Radiation lasts for the configured duration

2. **Damage System**:
   - Players outside safe zones take periodic damage
   - Damage occurs every second while the event is active
   - Visual hints alert affected players

3. **Protection**:
   - Using SCP-500 grants 30 seconds of radiation immunity
   - Players receive confirmation when protected
   - Safe zones completely prevent damage
