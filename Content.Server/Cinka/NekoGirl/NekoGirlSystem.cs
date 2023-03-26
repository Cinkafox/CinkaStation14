using Content.Server.Cinka.NekoGirl.Components;
using Content.Server.Popups;
using Content.Shared.Interaction.Events;
using Robust.Shared.Map;

namespace Content.Server.Cinka.NekoGirl
{
    public sealed class EvilNekoGirlSystem : EntitySystem
    {

        [Dependency] private readonly PopupSystem _popupSystem = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<NekoGirlSummonerComponent, UseInHandEvent>(OnSummonerUse);
        }

        private void OnSummonerUse(EntityUid uid, NekoGirlSummonerComponent component, UseInHandEvent args)
        {
            if (args.Handled)
                return;

            if (TryActivateSummoner(component, uid))
                args.Handled = true;
            else
                _popupSystem.PopupEntity("Вы израсходовали ману",args.User);
        }

        private bool TryActivateSummoner(NekoGirlSummonerComponent component,EntityUid uid)
        {
            if (component.UseCount <= 0 || !TryComp<TransformComponent>(uid, out var transform))
                return false;
            SummonNekoGirl(component,transform.MapPosition,uid);
            component.UseCount--;
            return true;
        }

        private void SummonNekoGirl(NekoGirlSummonerComponent component,MapCoordinates coordinates,EntityUid summonerUid)
        {
            var nekoGirlUid = Spawn(component.Prototype, coordinates);
            if (TryComp<NekoGirlComponent>(nekoGirlUid, out var nekoGirlComponent))
                nekoGirlComponent.SummonerUid = summonerUid;
        }

    }
}
