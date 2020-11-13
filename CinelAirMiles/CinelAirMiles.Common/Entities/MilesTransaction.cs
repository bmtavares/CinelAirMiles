namespace CinelAirMiles.Common.Entities
{
    using System;

    public class MilesTransaction : IEntity
    {
        public int Id { get; set; }


        public Mile Mile { get; set; }


        public DateTime TransactionDate { get; set; }
        

        public Client Client { get; set; }


        public MilesTransactionType MilesTransactionType { get; set; }
    }
}
