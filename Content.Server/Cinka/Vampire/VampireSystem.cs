using Content.Server.Body.Components;
using Content.Server.Cinka.Vampire.Components;
using Content.Shared.Verbs;

namespace Content.Server.Cinka.Vampire;


public sealed class VampireSystem : EntitySystem
{

    public override void Initialize()
    {
        SubscribeLocalEvent<BloodstreamComponent,GetVerbsEvent<Verb>>(OnVerb);
    }

    private void OnVerb(EntityUid uid, BloodstreamComponent component, GetVerbsEvent<Verb> args)
    {
        Verb v = new()
        {
            Text = "Высосать кровь",
            Message = "Высосать кровь из жертвы",
            Act = () =>
            {

            }
        };

    }
}
