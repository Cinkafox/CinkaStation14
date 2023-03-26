namespace Content.Server.Cinka.NekoGirl.Components
{
    [RegisterComponent]
    public sealed class NekoGirlComponent : Component
    {
        [ViewVariables]
        [DataField("summonerUid")]
        public EntityUid SummonerUid;
    }
}
