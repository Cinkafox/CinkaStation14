using Content.Server.Cinka.GunSilencerByCat.Components;
using Content.Server.Hands.Components;
using Content.Server.Speech.Components;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Containers;

namespace Content.Server.Cinka.GunSilencerByCat;

public sealed class GunSilencerByCatSystem : EntitySystem
{
    [Dependency] private readonly AudioSystem _audioSystem = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlotsSystem = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ReplacementAccentComponent,GetVerbsEvent<Verb>>(OnCatPutVerbs);
        SubscribeLocalEvent<GunSilencerByCatComponent,GetVerbsEvent<Verb>>(OnCatPutAwayVerb);
        SubscribeLocalEvent<GunSilencerByCatComponent,AmmoShotEvent>(ShotEvent);
        SubscribeLocalEvent<GunSilencerByCatComponent,EntRemovedFromContainerMessage>(OnEntRemovedFromContainer);
        SubscribeLocalEvent<GunSilencerByCatComponent,ComponentShutdown>(OnComponentShutdown);
        SubscribeLocalEvent<GunSilencerByCatComponent,ComponentInit>(OnComponentInit);
    }

    private void OnEntRemovedFromContainer(EntityUid uid, GunSilencerByCatComponent component, EntRemovedFromContainerMessage args)
    {
        if (args.Entity.Equals(component.CatUid))
            RemComp<GunSilencerByCatComponent>(component.GunUid);
    }

    private void OnComponentInit(EntityUid uid, GunSilencerByCatComponent component, ComponentInit args)
    {
        _itemSlotsSystem.AddItemSlot(uid,GunSilencerByCatComponent.CatSlotId,component.CatSlot);
    }


    private void OnComponentShutdown(EntityUid uid, GunSilencerByCatComponent component, ComponentShutdown args)
    {
        _itemSlotsSystem.RemoveItemSlot(uid,component.CatSlot);
    }


    private void OnCatPutAwayVerb(EntityUid uid, GunSilencerByCatComponent component, GetVerbsEvent<Verb> args)
    {
        Verb v = new()
        {
            Text = "Убрать кота",
            Message = "Пощадить кота и перестать использовать его как глушитель",
            Act = () =>
            {
                _itemSlotsSystem.TryEject(uid, component.CatSlot, null, out var _);
            }
        };

        args.Verbs.Add(v);
    }

    private void OnCatPutVerbs(EntityUid uid, ReplacementAccentComponent component, GetVerbsEvent<Verb> args)
    {
        if (component.Accent != "cat" || !TryComp<HandsComponent>(args.User,out var hands) ||
            hands.ActiveHand?.Container?.ContainedEntity == null)
            return;
        var gunUid = hands.ActiveHand.Container.ContainedEntity.Value;

        if(!TryComp<GunComponent>(gunUid,out _) || TryComp<GunSilencerByCatComponent>(gunUid,out _))
            return;

        Verb v = new()
        {
            Text = "Насадить кота",
            Message = "Использовать кота как глушитель",
            Act = () =>
            {
                TryPutCatOnGun(gunUid, uid);
            }
        };

        args.Verbs.Add(v);
    }

    public void TryPutCatOnGun(EntityUid gunId,EntityUid catId)
    {
        var c = AddComp<GunSilencerByCatComponent>(gunId);
        c.CatUid = catId;
        c.GunUid = gunId;

        if (TryComp<ItemSlotsComponent>(gunId, out _))
            _itemSlotsSystem.TryInsert(gunId, c.CatSlot, catId, null);
        _audioSystem.PlayPvs(new SoundPathSpecifier("/Audio/Animals/cat_hiss.ogg"), catId);
    }


    private void ShotEvent(EntityUid uid, GunSilencerByCatComponent component, AmmoShotEvent args)
    {
        _audioSystem.PlayPvs(new SoundPathSpecifier("/Audio/Animals/cat_meow.ogg"), uid);
    }



}
