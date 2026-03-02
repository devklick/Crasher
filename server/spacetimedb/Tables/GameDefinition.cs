using SpacetimeDB;

public static partial class Module
{
    /// <summary>
    /// Various configurations for a given game. 
    /// If GameDefinitions exist, they will each run in parallel.
    /// </summary>
    [Table(Public = true)]
    public partial struct GameDefinition
    {
        [PrimaryKey]
        public ulong Id;

        /// <summary>
        /// User-friendly name for this game definition.
        /// </summary>
        [Unique]
        public string Name;

        /// <summary>
        /// A percentage, presented in decimal form, of wagered wagered bets
        /// that the house expects to retain over the long run. 
        /// 
        /// Also can be considered as the reverse-RTP (return to player).
        /// 
        /// For a margin of 0.03 (3%) the house expects to retain 3% and the 
        /// remaining 97% is expected to be returned to the player(s).
        /// </summary>
        public double Margin;

        /// <summary>
        /// The frequency that the game ticks. 
        /// 
        /// Note that note every tick causes the multiplier to increment. 
        /// However, the multiplier cannot increment any faster than the specified 
        /// TickMilliseconds.
        /// </summary>
        public uint TickMilliseconds;

        /// <summary>
        /// The maximum multiplier that the game can reach. 
        /// 
        /// Any active bets at the point which the round reaches this multiplier
        /// are considered winning bets.
        /// </summary>
        public double MaxMultiplier;

        /// <summary>
        /// The number of seconds a new round waits before it starts to increment 
        /// the multiplier.
        /// </summary>
        public int CountdownSeconds;

        /// <summary>
        /// The number of seconds a round waits after the multiplier has crashed,
        /// before the round gets completed and a new round scheduled.
        /// </summary>
        public int CooldownSeconds;
    }

    public static List<GameDefinition> BuildGameDefinitions() => [
        new GameDefinition
        {
            Name = "Standard",
            Margin = 0.02,
            CountdownSeconds = 10,
            TickMilliseconds = 100,
            MaxMultiplier = 1000,
            CooldownSeconds = 3
        }
    ];
}