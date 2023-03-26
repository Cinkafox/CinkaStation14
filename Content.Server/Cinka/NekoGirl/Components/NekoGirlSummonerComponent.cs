using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Cinka.NekoGirl.Components;

[RegisterComponent]
public sealed class NekoGirlSummonerComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("UseCount")]
    public int UseCount = 1;

    [ViewVariables]
    [DataField("prototype", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string Prototype = "RandomHumanoidSpawnerNekoGirl";

}
