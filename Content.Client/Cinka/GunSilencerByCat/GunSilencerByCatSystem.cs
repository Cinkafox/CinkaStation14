using Content.Client.Cinka.GunSilencerByCat.Components;
using Robust.Client.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Utility;

namespace Content.Client.Cinka.GunSilencerByCat;

public sealed class GunSilencerByCatSystem : EntitySystem
{

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<GunSilencerByCatComponent,ComponentStartup>(OnSilencerByCatAdded);
        SubscribeLocalEvent<GunSilencerByCatComponent,ComponentShutdown>(SilencerByCatRemoved);
    }

    private void SilencerByCatRemoved(EntityUid uid, GunSilencerByCatComponent component, ComponentShutdown args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        if (!sprite.LayerMapTryGet(GunSilencerByCatKey.Key, out var layer))
            return;

        sprite.RemoveLayer(layer);
    }

    private void OnSilencerByCatAdded(EntityUid uid, GunSilencerByCatComponent component, ComponentStartup args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        if (sprite.LayerMapTryGet(GunSilencerByCatKey.Key, out var _))
            return;

        var adjh = sprite.Bounds.Width / 2;
        var adjw = sprite.Bounds.Width - 0.2f;
        var rsi = new SpriteSpecifier.Rsi(new ResourcePath("Mobs/Pets/cat.rsi"),"cat2_rest");

        var layer = sprite.AddLayer(rsi);
        sprite.LayerMapSet(GunSilencerByCatKey.Key, layer);


        sprite.LayerSetOffset(layer, new Vector2(adjw, adjh));
        sprite.LayerSetShader(layer, "unshaded");
    }

    private enum GunSilencerByCatKey
    {
        Key,
    }
}
