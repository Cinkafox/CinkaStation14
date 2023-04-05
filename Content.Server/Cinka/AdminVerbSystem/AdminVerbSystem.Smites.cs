using Content.Shared.Database;
using Content.Shared.Interaction.Components;
using Content.Shared.Inventory;
using Content.Shared.Verbs;
using Robust.Shared.Utility;

namespace Content.Server.Cinka.AdminVerbSystem;

public sealed partial class AdminVerbSystem : EntitySystem
{
    [Dependency] private readonly InventorySystem _inventorySystem = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<GetVerbsEvent<Verb>>(AddSmiteVerbs);
    }

    private void AddSmiteVerbs(GetVerbsEvent<Verb> args)
    {
        if (TryComp<InventoryComponent>(args.Target, out var inventory))
        {
            Verb nyanify = new()
            {
                Text = "Фуррифай",
                Category = VerbCategory.Smite,
                Icon = new SpriteSpecifier.Rsi(new ResourcePath("/Textures/Cinka/Clothing/OuterClothing/furrysuit.rsi"), "icon"),
                Act = () =>
                {
                    var ears = Spawn("ClothingOuterFurrySuit", Transform(args.Target).Coordinates);
                    EnsureComp<UnremoveableComponent>(ears);
                    _inventorySystem.TryUnequip(args.Target, "outerClothing", true, true, false, inventory);
                    _inventorySystem.TryEquip(args.Target, ears, "outerClothing", true, true, false, inventory);
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("Аха! Теперь он станет главным врагом бебса!")
            };
            args.Verbs.Add(nyanify);
        }
    }
}
