using Robust.Shared.Prototypes;

namespace Content.Shared.Cinka.SpeciesJobWhitelist;

[Prototype("speciesJobWhiteList")]
public sealed class SpeciesJobWhiteListPrototype : IPrototype
{

    [ViewVariables]
    [IdDataField]
    public string ID { get; } = default!;

    //list of races that are allowed to work at this job
    [DataField("values")] public IReadOnlyList<string> Values { get; } = new List<string>();
}
