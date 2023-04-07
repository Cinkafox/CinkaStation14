using Content.Shared.Cinka.GunSilencerByCat.Components;
using Content.Shared.Containers.ItemSlots;
using Robust.Shared.GameStates;

namespace Content.Server.Cinka.GunSilencerByCat.Components;

[NetworkedComponent,RegisterComponent]
public sealed class GunSilencerByCatComponent : SharedGunSilencerByCatComponent
{
    [DataField("GunUid")]
    public EntityUid GunUid;

    [DataField("CatUid")]
    public EntityUid CatUid;

    public const string CatSlotId = "cat_slot";

    [DataField("cat_slot")]
    public ItemSlot CatSlot = new();
}
