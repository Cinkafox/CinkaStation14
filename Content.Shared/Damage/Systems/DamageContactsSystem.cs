using Content.Shared.Damage.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.Damage.Systems;

public sealed class DamageContactsSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<DamageContactsComponent, StartCollideEvent>(OnEntityEnter);
        SubscribeLocalEvent<DamageContactsComponent, EndCollideEvent>(OnEntityExit);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        foreach (var damaged in EntityQuery<DamagedByContactComponent>())
        {
            var ent = damaged.Owner;
            if (_timing.CurTime < damaged.NextSecond)
                continue;
            damaged.NextSecond = _timing.CurTime + TimeSpan.FromSeconds(1);

            if (damaged.Damage != null)
                _damageable.TryChangeDamage(ent, damaged.Damage, interruptsDoAfters: false);
        }
    }

    private void OnEntityExit(EntityUid uid, DamageContactsComponent component, ref EndCollideEvent args)
    {
        var otherUid = args.OtherEntity;

        if (!TryComp<PhysicsComponent>(uid, out var body))
            return;

        var damageQuery = GetEntityQuery<DamageContactsComponent>();
        foreach (var ent in _physics.GetContactingEntities(uid, body))
        {
            if (ent == uid)
                continue;

            if (damageQuery.HasComponent(ent))
                return;
        }

        RemComp<DamagedByContactComponent>(otherUid);
    }

    private void OnEntityEnter(EntityUid uid, DamageContactsComponent component, ref StartCollideEvent args)
    {
        var otherUid = args.OtherEntity;

        if (HasComp<DamagedByContactComponent>(otherUid))
            return;

        if (component.IgnoreWhitelist?.IsValid(otherUid) ?? false)
            return;

        var damagedByContact = EnsureComp<DamagedByContactComponent>(otherUid);
        damagedByContact.Damage = component.Damage;
    }
}
