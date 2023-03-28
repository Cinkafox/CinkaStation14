using Content.Server.Cinka.Traits.Components;
using Content.Shared.Buckle.Components;
using Content.Shared.Movement.Components;
using Content.Shared.Stunnable;
using Robust.Shared.Containers;
using Robust.Shared.Physics.Systems;

namespace Content.Server.Cinka.Traits;

public sealed class DisabilitySystem : EntitySystem
{
    [Dependency] private readonly SharedStunSystem _stun = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DisabilityComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<DisabilityComponent, ComponentShutdown>(OnComponentShutDown);
        SubscribeLocalEvent<DisabilityComponent, EntRemovedFromContainerMessage>(OnDeattach);
    }

    private void OnDeattach(EntityUid uid, DisabilityComponent component, EntRemovedFromContainerMessage args)
    {
        _stun.TryParalyze(uid, TimeSpan.FromSeconds(3), true);
    }


    private void OnComponentShutDown(EntityUid uid, DisabilityComponent component, ComponentShutdown args)
    {
        RemComp<MovementSpeedModifierComponent>(uid);
    }

    private void OnComponentInit(EntityUid uid, DisabilityComponent component, ComponentInit args)
    {
        var movementSpeed = EnsureComp<MovementSpeedModifierComponent>(uid);
        (movementSpeed.BaseSprintSpeed, movementSpeed.BaseWalkSpeed) = (0, movementSpeed.BaseWalkSpeed*0.7f);
    }
}
