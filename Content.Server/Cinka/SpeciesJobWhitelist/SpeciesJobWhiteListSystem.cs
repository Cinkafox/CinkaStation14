using System.Linq;
using Content.Server.Chat.Managers;
using Content.Server.GameTicking;
using Content.Server.Players;
using Content.Server.Preferences.Managers;
using Content.Server.Roles;
using Content.Server.Station.Systems;
using Content.Shared.CCVar;
using Content.Shared.Cinka.SpeciesJobWhitelist;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;


namespace Content.Server.Cinka.SpeciesJobWhitelist;

public sealed class SpeciesJobWhiteListSystem : EntitySystem
{
    [Dependency] private readonly IServerPreferencesManager _preferencesManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly StationSpawningSystem _stationSpawning = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawn);

    }

    private void OnPlayerSpawn(PlayerSpawnCompleteEvent args)
    {
        if (!_cfg.GetCVar(CCVars.SpeciesJobWhitelistEnabled) || args.JobId == null || !_prototypeManager.HasIndex<SpeciesJobWhiteListPrototype>(args.JobId))
            return;

        var whiteListRaces = _prototypeManager.Index<SpeciesJobWhiteListPrototype>(args.JobId);

        if(CheckSpecieInWhitelist(whiteListRaces.Values,args.Profile))
            return;

        _chatManager.DispatchServerMessage(args.Player,Loc.GetString(
            "species-is-not-allowed",("job",args.JobId)));
        var allowedCharacter = HumanoidCharacterProfile.RandomWithSpecies(_random.Pick(whiteListRaces.Values));

        foreach (var characterProfile in _preferencesManager.GetPreferences(args.Player.UserId).Characters)
        {
            var character = (HumanoidCharacterProfile) characterProfile.Value;
            if (!CheckSpecieInWhitelist(whiteListRaces.Values, character))
                continue;
            _chatManager.DispatchServerMessage(args.Player, Loc.GetString(
                "alternative-profile",("oldprofile",args.Profile.Name),("newprofile",character.Name)));
            allowedCharacter = character;
            break;

        }

        _chatManager.DispatchServerMessage(args.Player,
            Loc.GetString("species-awalible-for-job",("species",String.Join(" ", whiteListRaces.Values))));

        ReplacePlayerMob(args.Player,allowedCharacter,args.Mob,args.JobId,args.Station);

    }

    public void ReplacePlayerMob(IPlayerSession player,HumanoidCharacterProfile character,EntityUid oldMob,string jobId,EntityUid station)
    {
        var data = player.ContentData();
        data!.WipeMind();
        var newMind = new Mind.Mind(data.UserId)
        {
            CharacterName = character.Name
        };
        newMind.ChangeOwningPlayer(data.UserId);

        var jobPrototype = _prototypeManager.Index<JobPrototype>(jobId);
        var job = new Job(newMind, jobPrototype);
        newMind.AddRole(job);

        if(!TryComp<TransformComponent>(oldMob,out var pos))
            return;

        var mob = _stationSpawning.SpawnPlayerMob(pos.Coordinates, job, character, station);

        EntityManager.DeleteEntity(oldMob);
        newMind.TransferTo(mob);
    }


    public bool CheckSpecieInWhitelist(IReadOnlyList<string> whitelist, HumanoidCharacterProfile character)
    {
        return whitelist.Count == 0 || whitelist.Any(whiteListRace => character.Species == whiteListRace);
    }




}
