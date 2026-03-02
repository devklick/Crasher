using SpacetimeDB;
public static partial class Module
{
    /// <summary>
    /// Table with only one record per `GameDefinition` at all times.
    /// A round will progress from Countdown > Started > Ended > Completed, 
    /// and at the point it becomes Completed, gets moved to the HistoricRound table
    /// and the ActiveRound is rebuilt for the next round.
    /// </summary>
    [Table(Public = true)]
    public partial struct ActiveRound
    {
        /// <summary>
        /// The ID for the active round in this table.
        /// </summary>
        [PrimaryKey]
        [AutoInc]
        public ulong Id;

        /// <summary>
        /// The ID of the <see cref="GameDefinition"/> (configuration) that this round is using.
        /// 
        /// There can be only one <see cref="ActiveRound"/> per <see cref="GameDefinitionCols"/>
        /// </summary>
        [Unique]
        public ulong GameDefinitionId;

        /// <summary>
        /// The multiplier that an in-progress round is currently on.
        /// 
        /// Will be null while the round is in the Countdown state.
        /// </summary>
        public double? CurrentMultiplier;

        /// <summary>
        /// The multiplier at which the round crashed on.
        /// Will be null while the round is in Countdown and Started states.
        /// Will be populated once the round moves to Ended.
        /// </summary>
        public double? CrashMultiplier;

        /// <summary>
        /// The current round status. 
        /// While in the ActiveRound table, we should never expect to see a 
        /// status of Completed.
        /// </summary>
        public RoundStatus Status;

        /// <summary>
        /// The time at which the countdown started.
        /// 
        /// This will always have a value. At the exact time a round is created, 
        /// the countdown starts.
        /// </summary>
        public Timestamp CountdownStartTime;

        /// <summary>
        /// The time at which the round is scheduled to move from the Countdown
        /// state to the Started state.
        /// 
        /// This is computed when the round is created. It may differ slightly 
        /// from the StartedTime.
        /// </summary>
        public Timestamp TargetStartTime;

        /// <summary>
        /// The number of seconds remaining before the round moves from the 
        /// Countdown state to the Started state.
        /// </summary>
        public int CountdownSecondsRemaining;

        /// <summary>
        /// The time at which the round progressed from the Countdown state to
        /// the Started state.
        /// 
        /// Null while the round is still in the Countdown state.
        /// </summary>
        public Timestamp? StartTime;

        /// <summary>
        /// The time at which the round has reached the CrashMultiplier and moved 
        /// to the Ended state.
        /// 
        /// This will be null while the round is in the Countdown and Started states, 
        /// and will be populated in the Ended state.
        /// </summary>
        public Timestamp? CrashTime;
    }
}