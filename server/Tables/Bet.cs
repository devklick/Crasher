using SpacetimeDB;

public static partial class Module
{
    [Table(Public = true)]
    public partial struct Bet
    {
        [PrimaryKey]
        [AutoInc]
        public ulong Id;

        public ulong RoundId;
        public Identity Player;
        public double Amount;
        public double? CashoutMultiplier;
    }
}