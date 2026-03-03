using SpacetimeDB;

public static partial class Module
{
    /// <summary>
    /// Enum representing the various states a round can be observed in. 
    /// 
    /// A round can be considered a finite state machine, where it will always 
    /// progress through each state in the same order:
    /// 
    /// <list type="number">
    /// <item><see cref="Countdown"/></item>
    /// <item><see cref="InProgress"/></item>
    /// <item><see cref="Ended"/></item>
    /// <item><see cref="Complete"/></item>
    /// </list>
    /// </summary>
    [Type]
    public enum RoundStatus
    {
        /// <summary>
        /// The state that a round initially starts off in. 
        /// 
        /// During this state, we waiting for the scheduled round start time, 
        /// and we count down the seconds remaining until this time.
        /// </summary>
        Countdown,

        /// <summary>
        /// Once a round reaches the scheduled start time, it moves to InProgress.
        /// 
        /// During this state, the multiplier increases until it crashes out.
        /// </summary>
        InProgress,

        /// <summary>
        /// When a round reaches it's final multiplier and crashes, it moves 
        /// to the Ended state. 
        /// 
        /// This is a cooldown period before the round is marked as complete
        /// and a new round is scheduled.
        /// </summary>
        Ended,

        /// <summary>
        /// After an ended game has cooled down, it moves to Complete and 
        /// essentially gets archived.
        /// </summary>
        Complete
    }
}