using SpacetimeDB;

public static partial class Module
{
    /// <summary>
    /// Initialization function called when the DB is started up.
    /// We use this to schedule the first game round(s) and set up the recurring tick.
    /// </summary>
    /// <param name="ctx"></param>
    [Reducer(ReducerKind.Init)]
    public static void Init(ReducerContext ctx)
    {
        if (ctx.Db.TickSchedule.Count == 0)
        {
            foreach (var gameDefinition in BuildGameDefinitions())
            {
                ctx.Db.GameDefinition.Insert(gameDefinition);

                Log.Debug($"Created GameDefinition {gameDefinition}");

                // Create the initial round which starts the countdown immediately 
                Engine.InitActiveRound(ctx, gameDefinition, ctx.Timestamp);

                // Set up a schedule for the game to tick every X milliseconds. 
                // This can maybe be tweaked to reduce the amount processing.
                var tickSchedule = new TickSchedule
                {
                    ScheduledAt = new ScheduleAt.Interval(TimeDuration.FromMilliseconds(gameDefinition.TickMilliseconds)),
                    GameDefinitionId = gameDefinition.Id
                };

                ctx.Db.TickSchedule.Insert(tickSchedule);

                Log.Info($"Created TickSchedule {tickSchedule}");
            }
        }
    }
}