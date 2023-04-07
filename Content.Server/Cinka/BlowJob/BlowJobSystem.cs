using Content.Server.Cinka.BlowJob.Components;
using Content.Server.Fluids.EntitySystems;
using Content.Server.Popups;
using Content.Shared.Chemistry.Components;
using Content.Shared.Interaction;
using Robust.Server.GameObjects;

namespace Content.Server.Cinka.BlowJob
{
    public sealed class BlowJobSystem : EntitySystem
    {
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        [Dependency] private readonly SpillableSystem _spillableSystem = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<BlowJobComponent, ActivateInWorldEvent>(OnActivate);
        }

        /// <summary>
        /// monitors the number of сlick to entity and blow ass if click > 10
        /// </summary>
        private void OnActivate(EntityUid uid, BlowJobComponent component, ActivateInWorldEvent args)
        {
            component.Clicked=(component.Clicked + 1)% (component.BlowCount+1);
            if (component.Clicked < component.BlowCount) return;
            _popupSystem.PopupEntity("вас насильно ебут в жопу", args.User);

            Solution bloodSolution = new();
            bloodSolution.AddReagent("Blood", 50);
            _spillableSystem.SpillAt(args.User, bloodSolution, "PuddleBlood");

        }

    }
}
