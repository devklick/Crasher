using SpacetimeDB;

public static partial class Module
{
    /// <summary>
    /// Essentially an extension of the <see cref="ActiveRound"/>, intended to
    /// store any data that should not be synced back to the client.
    /// </summary>
    [Table]
    public partial struct PrivateActiveRound
    {
        /// <summary>
        /// The ID for the active round in this table.
        /// 
        /// This matches the <see cref="ActiveRound.Id"/> 
        /// </summary>
        [PrimaryKey]
        public ulong Id;

        /// <summary>
        /// The multiplier at which the game will crash on. 
        /// 
        /// Note: Ideally the actual crash multiplier should not be stored
        /// while the game is active, but rather a secret seed stored, from which
        /// the crash multiplier can be calculated. However, `System.Security.Cryptography`
        /// is not available in WASI. Need to consider other alternatives.
        /// </summary>
        public double CrashMultiplier;
    }
}