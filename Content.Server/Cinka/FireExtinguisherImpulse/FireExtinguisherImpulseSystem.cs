using Content.Server.Chemistry.Components.SolutionManager;
using Content.Server.Fluids.Components;
using Content.Server.Fluids.EntitySystems;
using Content.Shared.Throwing;
using Content.Shared.Vehicle.Components;
using Robust.Shared.Physics.Components;

namespace Content.Server.Cinka.FireExtinguisherImpulse;

public sealed class FireExtinguisherImpulseSystem : EntitySystem
{
    [Dependency] private readonly ThrowingSystem _throwing = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SprayComponent, SprayAttemptEvent>(OnPshikAttempt);
    }

    private void OnPshikAttempt(EntityUid uid, SprayComponent component, SprayAttemptEvent args)
    {
        if (TryComp<RiderComponent>(args.User, out var rider) && !args.Cancelled && TryComp<SolutionContainerManagerComponent>(uid,out var scmc))
        {
            if (rider.Vehicle != null && TryComp<PhysicsComponent>(rider.Vehicle.Value, out var physics) && scmc.Solutions["spray"].Volume > 0)
            {
                _throwing.TryThrow(rider.Vehicle.Value, physics.LinearVelocity, user: args.User, pushbackRatio: 50f);
            }

        }
    }
}
