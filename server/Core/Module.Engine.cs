using SpacetimeDB;

public static partial class Module
{
    public static class Engine
    {
        public static (ActiveRound ActiveRound, PrivateActiveRound PrivateActiveRound) InitActiveRound(ReducerContext ctx, GameDefinition gameDefinition, Timestamp countdownStartTime)
        {
            var activeRound = ctx.Db.ActiveRound.Insert(new ActiveRound
            {
                GameDefinitionId = gameDefinition.Id,
                CountdownSecondsRemaining = gameDefinition.CountdownSeconds,
                CountdownStartTime = countdownStartTime,
                Status = RoundStatus.Countdown,
                TargetStartTime = countdownStartTime + TimeSpan.FromSeconds(gameDefinition.CountdownSeconds),
                CrashMultiplier = null,
            });

            var privateActiveRound = ctx.Db.PrivateActiveRound.Insert(new PrivateActiveRound
            {
                Id = activeRound.Id,
                CrashMultiplier = Multiplier.GenerateCrashMultiplier(ctx.Rng, gameDefinition.Margin, gameDefinition.MaxMultiplier),
            });

            Log.Debug($"Next round round created, {activeRound.Id}, starts at {activeRound.TargetStartTime}");

            return (activeRound, privateActiveRound);
        }
    }
}