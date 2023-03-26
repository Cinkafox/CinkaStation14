using Content.Server.Cinka.GunSilencerByCat.Components;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;

namespace Content.Server.Cinka.GunSilencerByCat;

public sealed class GunSilencerByCatSystem : EntitySystem
{
    [Dependency] private readonly AudioSystem _audioSystem = default!;
    public override void Initialize()
    {
        base.Initialize();

        //SubscribeLocalEvent<GunComponent,  GetVerbsEvent<AlternativeVerb>>(AddAlternativeVerbs);
        SubscribeLocalEvent<GunSilencerByCatComponent, AmmoShotEvent>(ShotEvent);
    }

    private void ShotEvent(EntityUid uid, GunSilencerByCatComponent component, AmmoShotEvent args)
    {
        _audioSystem.PlayPvs(new SoundPathSpecifier("/Audio/Animals/cat_meow.ogg"), uid);
    }



}
