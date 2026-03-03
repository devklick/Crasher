using SpacetimeDB;

public static partial class Module
{
    /// <summary>
    /// Table that schedules the game tick.
    /// 
    /// Every <see cref="GameDefinition"/> will have it's own schedule.
    /// </summary>
    [Table(Scheduled = nameof(ProcessGameTick), ScheduledAt = nameof(ScheduledAt))]
    public partial struct TickSchedule
    {
        [PrimaryKey]
        [AutoInc]
        public ulong Id;

        /// <summary>
        /// The frequency of the tick.
        /// </summary>
        public ScheduleAt ScheduledAt;

        /// <summary>
        /// The ID of the <see cref="GameDefinition"/> that this schedule relates to.
        /// </summary>
        public ulong GameDefinitionId;
    }
}