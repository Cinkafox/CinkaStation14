using Content.Server.Chemistry.Components.SolutionManager;
using Content.Server.Cinka.FireExtinguisherImpulse.Components;
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
        SubscribeLocalEvent<SprayComponent, SprayAttemptEvent>(OnSprayAttempt);
    }

    /// <summary>
    /// Gives impulse of entity on spray attempt
    /// </summary>
    private void OnSprayAttempt(EntityUid uid, SprayComponent component, SprayAttemptEvent args)
    {
        if (!TryComp<RiderComponent>(args.User, out var rider) || args.Cancelled ||
            !TryComp<SolutionContainerManagerComponent>(uid, out var scmc))
            return;

        if (scmc.Solutions["spray"].Volume > 0)
        {
            TryThrow(rider.Vehicle,args.User);
        }
    }

    /// <summary>
    /// Trying to give impulse
    /// </summary>
    private void TryThrow(EntityUid? vehicle,EntityUid rider)
    {
        if(vehicle != null && TryComp<PhysicsComponent>(vehicle.Value, out var physics) && TryComp<FireExtinguisherImpulseComponent>(vehicle.Value,out _))
            _throwing.TryThrow(vehicle.Value, physics.LinearVelocity, user: rider, pushbackRatio: 50f);
    }
}
