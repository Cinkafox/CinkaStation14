using Content.Server.Extinguisher;
using Content.Server.Fluids.Components;
using Content.Server.Fluids.EntitySystems;
using Content.Shared.Interaction;

namespace Content.Server.Cinka.FireExtinguisherImpulse;

public sealed class FireExtinguisherImpulseSystem : EntitySystem
{
    //[Dependency] private readonly ThrowingSystem _throwing = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SprayComponent, SprayAttemptEvent>(OnPshikAttempt);
    }

    private void OnPshikAttempt(EntityUid uid, SprayComponent component, SprayAttemptEvent args)
    {

        Logger.Debug("Pshik");
    }
}
