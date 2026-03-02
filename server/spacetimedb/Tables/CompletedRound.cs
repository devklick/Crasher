using SpacetimeDB;
public static partial class Module
{
    /// <summary>
    /// Essentially a historic round. 
    /// 
    /// While a round is active or in-play, it exists in the <see cref="ActiveRound"/>
    /// table, however once it completes, it's moved into <see cref="CompletedRound"/>.
    /// </summary>
    [Table(Public = true)]
    public partial struct CompletedRound
    {
        /// <summary>
        /// The ID for the active round in this table.
        /// When the ActiveRound is moved across to HistoricRound, this value 
        /// comes from ActiveRound.Id.
        /// </summary>
        [PrimaryKey]
        public ulong Id;

        /// <summary>
        /// The ID of the <see cref="GameDefinition"/> (configuration) that this round is using.
        /// </summary>
        public ulong GameDefinitionId;

        /// <summary>
        /// The multiplier at which the round crashed on.
        /// Determined at the point the round is created.
        /// </summary>
        public double CrashMultiplier;

        /// <summary>
        /// The time at which the countdown started.
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
        /// The time at which the round progressed from the Countdown state to
        /// the Started state.
        /// </summary>
        public Timestamp StartTime;

        /// <summary>
        /// The time at which the round has reached the CrashMultiplier and moved 
        /// to the Ended state.
        /// </summary>
        public Timestamp CrashTime;
    }
}