using SpacetimeDB;

public static partial class Module
{
    /// <summary>
    /// Invoked by the tick schedule to handle the core game logic each tick.
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="tick"></param>
    [Reducer]
    public static void ProcessGameTick(ReducerContext ctx, TickSchedule tick)
    {
        var round = ctx.Db.ActiveRound.GameDefinitionId.Find(tick.GameDefinitionId)
            ?? throw new KeyNotFoundException($"No ActiveRound for GameDefinition {tick.GameDefinitionId}");


        switch (round.Status)
        {
            case RoundStatus.Countdown:
                ProcessCountdownTick(ctx, round);
                break;
            case RoundStatus.InProgress:
                ProcessInProgressTick(ctx, round);
                break;
            case RoundStatus.Ended:
                ProcessEndedTick(ctx, round);
                break;
            case RoundStatus.Complete:
                Log.Error($"ActiveRound found in unexpected state {round}");
                break;
        }
    }

    private static void ProcessCountdownTick(ReducerContext ctx, ActiveRound round)
    {
        var secondsRemaining = (int)Math.Ceiling(Time.DiffSeconds(round.TargetStartTime, ctx.Timestamp));

        if (round.CountdownSecondsRemaining != secondsRemaining)
        {
            round.CountdownSecondsRemaining = secondsRemaining;
            ctx.Db.ActiveRound.Id.Update(round);
            Log.Debug($"Game Tick - Round {round.Id}, countdown seconds remaining {secondsRemaining}");
        }

        if (round.CountdownSecondsRemaining <= 0)
        {
            round.CurrentMultiplier = 1;
            round.Status = RoundStatus.InProgress;
            round.StartTime = ctx.Timestamp;
            ctx.Db.ActiveRound.Id.Update(round);
            Log.Debug($"Game Tick - Round {round.Id}, started");
        }
    }

    private static void ProcessInProgressTick(ReducerContext ctx, ActiveRound round)
    {
        ArgumentNullException.ThrowIfNull(round.StartTime, nameof(round.StartTime));

        var privateActiveRound = ctx.Db.PrivateActiveRound.Id.Find(round.Id)
            ?? throw new KeyNotFoundException($"No PrivateActiveRoundFound with Id {round.Id}");

        var elapsedSeconds = Time.DiffSeconds(ctx.Timestamp, round.StartTime.Value);

        round.CurrentMultiplier = Multiplier.GenerateMultiplierTick(elapsedSeconds, privateActiveRound.CrashMultiplier);

        Log.Debug($"Game Tick - Round {round.Id}, multiplier increased to {round.CurrentMultiplier}");

        if (round.CurrentMultiplier == privateActiveRound.CrashMultiplier)
        {
            round.CrashTime = ctx.Timestamp;
            round.Status = RoundStatus.Ended;
            Log.Debug($"Game Tick - Round {round.Id} ended, multiplier crashed at {round.CurrentMultiplier}");
        }

        ctx.Db.ActiveRound.Id.Update(round);
    }

    private static void ProcessEndedTick(ReducerContext ctx, ActiveRound round)
    {
        ArgumentNullException.ThrowIfNull(round.StartTime, nameof(round.StartTime));
        ArgumentNullException.ThrowIfNull(round.CrashTime, nameof(round.CrashTime));

        var gameDefinition = ctx.Db.GameDefinition.Id.Find(round.GameDefinitionId)
            ?? throw new KeyNotFoundException($"No GameDefinition found with Id {round.GameDefinitionId}");

        var elapsed = (int)Math.Floor(Time.DiffSeconds(ctx.Timestamp, round.CrashTime.Value));

        if (elapsed < gameDefinition.CooldownSeconds)
        {
            // Nothing to do on this tick.
            return;
        }

        var privateActiveRound = ctx.Db.PrivateActiveRound.Id.Find(round.Id)
            ?? throw new KeyNotFoundException($"No PrivateActiveRoundFound with Id {round.Id}");

        Log.Debug($"Game Tick - Round {round.Id} complete");

        // Archive old round
        ctx.Db.CompletedRound.Insert(new CompletedRound
        {
            Id = round.Id,
            GameDefinitionId = round.GameDefinitionId,
            StartTime = round.StartTime.Value,
            CountdownStartTime = round.CountdownStartTime,
            TargetStartTime = round.TargetStartTime,
            CrashTime = round.CrashTime.Value,
            CrashMultiplier = privateActiveRound.CrashMultiplier,
        });

        // Delete old active round
        ctx.Db.ActiveRound.Delete(round);
        ctx.Db.PrivateActiveRound.Delete(privateActiveRound);

        // Insert new ActiveRound
        Engine.InitActiveRound(ctx, gameDefinition, ctx.Timestamp);
    }
}