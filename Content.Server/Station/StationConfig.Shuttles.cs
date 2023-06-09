﻿using Robust.Shared.Utility;
using Robust.Shared.Serialization.TypeSerializers.Implementations;

namespace Content.Server.Station;

public sealed partial class StationConfig
{
    /// <summary>
    /// Emergency shuttle map path for this station.
    /// </summary>
    [DataField("emergencyShuttlePath", customTypeSerializer: typeof(ResPathSerializer))]
    public ResPath EmergencyShuttlePath { get; set; } = new("/Maps/Shuttles/emergency.yml");
}
