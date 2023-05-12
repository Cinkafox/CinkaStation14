using Robust.Shared.Prototypes;

namespace Content.Server.Cinka.RaceJobWhitelist;

[Prototype("racejobwhitelist")]
public sealed class RaceJobWhiteListPrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; } = default!;

    //list of races that are allowed to work at this job
    [DataField("values")] public IReadOnlyList<string> Values { get; } = new List<string>();
}
