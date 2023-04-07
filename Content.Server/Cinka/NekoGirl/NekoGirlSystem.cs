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

        /// <summary>
        /// Summon NekoMaid on summoner use
        /// </summary>
        private void OnSummonerUse(EntityUid uid, NekoGirlSummonerComponent component, UseInHandEvent args)
        {
            if (args.Handled)
                return;

            if (TryActivateSummoner(component, uid))
                args.Handled = true;
            else
                _popupSystem.PopupEntity("Вы израсходовали ману",args.User);
        }

        /// <summary>
        /// Trying to summon NekoMaid with summoner
        /// </summary>
        /// <param name="component"></param>
        /// <param name="uid">Summoner EntityUid</param>
        private bool TryActivateSummoner(NekoGirlSummonerComponent component,EntityUid uid)
        {
            if (component.UseCount <= 0 || !TryComp<TransformComponent>(uid, out var transform))
                return false;
            SummonNekoGirl(component,transform.MapPosition,uid);
            component.UseCount--;
            return true;
        }

        /// <summary>
        /// Trying to summon a NekoGirl
        /// </summary>
        /// <param name="component"></param>
        /// <param name="coordinates"></param>
        /// <param name="summonerUid"></param>
        public void SummonNekoGirl(NekoGirlSummonerComponent component,MapCoordinates coordinates,EntityUid summonerUid)
        {
            var nekoGirlUid = Spawn(component.Prototype, coordinates);
            if (TryComp<NekoGirlComponent>(nekoGirlUid, out var nekoGirlComponent))
                nekoGirlComponent.SummonerUid = summonerUid;
        }

    }
}
