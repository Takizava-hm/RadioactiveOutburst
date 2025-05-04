using Exiled.API.Enums;
using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace Plugin
{
    public class Config : IConfig
    {
        [Description("Whether the plugin is enabled or disabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Interval between radioactive outbursts in seconds.")]
        public float OutburstInterval { get; set; } = 300f;

        [Description("Duration of radioactive outburst in seconds.")]
        public float OutburstDuration { get; set; } = 60f;

        [Description("Damage dealt to players per tick during outburst.")]
        public float DamagePerTick { get; set; } = 5f;

        [Description("Message displayed when radioactive outburst begins.")]
        public string OutburstStartMessage { get; set; } = "<color=red>Radioactive outburst has begun!</color>";

        [Description("Message displayed when radioactive outburst ends.")]
        public string OutburstEndMessage { get; set; } = "<color=green>Radioactive outburst has ended.</color>";

        [Description("List of safe zones where players don't take damage during outburst.")]
        public List<ZoneType> SafeZones { get; set; } = new List<ZoneType> { ZoneType.LightContainment, ZoneType.Entrance, ZoneType.Surface };

        [Description("Whether debug mode is enabled for additional output information.")]
        public bool Debug { get; set; } = false;
    }
}