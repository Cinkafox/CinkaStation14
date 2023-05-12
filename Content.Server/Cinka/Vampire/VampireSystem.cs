using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.Chemistry.EntitySystems;
using Content.Server.Chemistry.ReagentEffects;
using Content.Server.Cinka.Vampire.Components;
using Content.Server.Popups;
using Content.Shared.Verbs;

namespace Content.Server.Cinka.Vampire;


public sealed class VampireSystem : EntitySystem
{

    [Dependency] private readonly BloodstreamSystem _bloodstreamSystem = default!;
    [Dependency] private readonly SolutionContainerSystem _solutionContainerSystem = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<BloodstreamComponent,GetVerbsEvent<Verb>>(OnVerb);
    }

    private void OnVerb(EntityUid uid, BloodstreamComponent component, GetVerbsEvent<Verb> args)
    {
        if (!args.CanAccess || !args.CanInteract || !TryComp<VampireComponent>(args.User, out _))
            return;

        args.Verbs.Add(new Verb
        {
            Text = "Высосать кровь",
            Message = "Высосать кровь из жертвы",
            Act = () =>
            {
                if (TrySuck(args.User, uid))
                {
                    _popupSystem.PopupEntity("Вам сделали кусь!", uid);
                    _popupSystem.PopupEntity("Вы сделали кусь!",args.User);
                }
            }
        });

    }

    private bool TrySuck(EntityUid entityUid,EntityUid target,float amount = 15f)
    {
        if (!TryComp<VampireComponent>(entityUid, out var vampireComponent) ||
            !TryComp<BloodstreamComponent>(target, out var bloodstreamComponent))
        {
            _popupSystem.PopupEntity("Фэ!",entityUid);
            return false;
        }

        if (bloodstreamComponent.BloodReagent != vampireComponent.BloodType)
        {
            _popupSystem.PopupEntity("Фэ! это не та кровь!",entityUid);
            return false;
        }

        if (!TryAddBloodSuckedCount(entityUid, amount))
            return false;

        var targetBloodLevel = bloodstreamComponent.BloodSolution.Volume;
        if (!_bloodstreamSystem.TryModifyBloodLevel(target, targetBloodLevel - amount, bloodstreamComponent))
        {
            Logger.Debug(targetBloodLevel + "");
            _popupSystem.PopupEntity("Фэ! не хочу!!",entityUid);
            return false;
        }

        return true;
    }

    private bool TryAddBloodSuckedCount(EntityUid entityUid, float amount)
    {
        if (!TryComp<VampireComponent>(entityUid, out var vampireComponent))
            return false;
        vampireComponent.BloodSuckedCount += amount;
        return true;
    }
}
