namespace Content.Server.Cinka.Vampire.Components;

[RegisterComponent]
public sealed class VampireComponent : Component
{
    [ViewVariables]
    [DataField("BloodType")]
    public string BloodType = "Blood";

    [DataField("BloodSuckedCount")]
    public float BloodSuckedCount = 0f;
}
