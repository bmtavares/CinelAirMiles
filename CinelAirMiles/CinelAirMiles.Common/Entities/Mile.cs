namespace CinelAirMiles.Common.Entities
{
    using System;

    public class Mile : IEntity
    {
        public int Id { get; set; }


        public Client Client { get; set; }


        public int ClientId { get; set; }


        public int Miles { get; set; }


        public MilesType MilesType { get; set; }


        public int MilesTypeId { get; set; }


        public DateTime CreditDate { get; set; }


        public DateTime ExpiryDate { get; set; }


        public string Description { get; set; }
    }
}
