public static partial class Module
{
    public static class Multiplier
    {

        public static double GenerateCrashMultiplier(Random rng, double margin, double maxMultiplier)
        {
            // Uniform(0,1) random variable.
            // This is the input for inverse transform sampling.
            var u = rng.NextDouble();

            // Classic Crash / Pareto distribution (inverse CDF sampling).
            //
            // Formula:
            //   crash = (1 - margin) / (1 - u)
            //
            // This is the inverse cumulative distribution function (CDF)
            // of a Pareto-like heavy-tailed distribution.
            //
            // Properties:
            //   - Many small multipliers near 1.0
            //   - Rare but very large multipliers (heavy tail)
            //   - Strict mathematical house edge equal to `margin`
            //
            // The (1 - margin) term directly encodes the house edge.
            // Expected payout multiplier = 1 / (1 - margin)
            //
            // This is the standard formula used in most provably-fair crash games.
            var crash = (1 - margin) / (1 - u);

            // Floor to 2 decimals for payout precision consistency.
            // Clamp to:
            //   - Minimum 1.0 (cannot crash below 1x)
            //   - Maximum maxMultiplier (hard cap safety limit)
            return Math.Min(Math.Max(1.0, Floor2Decimals(crash)), maxMultiplier);
        }


        public static double GenerateMultiplierTick(double elapsedSeconds, double crashMultiplier)
        {
            // Time-based growth curve (power growth function).
            //
            // multiplier(t) = 1 + k * t^p
            //
            // k = 0.12  -> overall growth speed coefficient
            // p = 2.2   -> acceleration exponent (super-quadratic growth)
            //
            // This creates:
            //   - Slow early movement
            //   - Gradual acceleration
            //   - Faster late-stage climb
            //
            // Unlike exponential growth, this remains tunable and avoids runaway explosion.
            var multiplier = 1.0 + 0.12 * Math.Pow(elapsedSeconds, 2.2);

            // Clamp visual multiplier to precomputed crash point.
            // Ensures we never exceed the predetermined crash multiplier.
            multiplier = Math.Min(multiplier, crashMultiplier);

            // Round to 2 decimal places using AwayFromZero.
            // Ensures financial consistency and avoids floating-point drift bias.
            return Math.Round(multiplier, 2, MidpointRounding.AwayFromZero);
        }

        private static double Floor2Decimals(double value)
        {
            // Shift two decimals into integer part, drop extra decimals, convert back
            return Math.Floor(value * 100) / 100.0;
        }
    }
}