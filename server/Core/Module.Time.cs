using SpacetimeDB;

public static partial class Module
{
    /// <summary>
    /// Helpers for managing time
    /// </summary>
    public static class Time
    {
        public static readonly long MicrosecondsPerSecond = 1_000_000;

        /// <summary>
        /// Calculate the number of seconds, presented as a double, between two times.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static double DiffSeconds(Timestamp first, Timestamp second)
        {
            long diffMicroseconds = first.MicrosecondsSinceUnixEpoch - second.MicrosecondsSinceUnixEpoch;
            return diffMicroseconds / (double)MicrosecondsPerSecond;
        }
    }
}