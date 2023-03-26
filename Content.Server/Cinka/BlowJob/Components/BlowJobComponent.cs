namespace Content.Server.Cinka.BlowJob.Components
{
    [RegisterComponent]
    public sealed class BlowJobComponent : Component
    {
        [DataField("BlowCount")]
        public int BlowCount = 10;

        [DataField("Clicked")]
        public int Clicked = 0;
    }
}
