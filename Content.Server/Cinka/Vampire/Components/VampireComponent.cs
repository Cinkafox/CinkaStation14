namespace Content.Server.Cinka.Vampire.Components;

[RegisterComponent]
public sealed class VampireComponent : Component
{
    [ViewVariables]
    [DataField("Bloodtype")]
    public string Bloodtype = "Blood";
}
