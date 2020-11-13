namespace CinelAirMiles.Common.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Mile : IEntity
    {
        public int Id { get; set; }


        public Client Client { get; set; }


        public int ClientId { get; set; }


        [Display(Name = "Quantity")]
        public int Miles { get; set; }

        public int Balance { get; set; }

        public MilesType MilesType { get; set; }


        public int MilesTypeId { get; set; }


        [Display(Name = "Credit date")]
        public DateTime CreditDate { get; set; }


        [Display(Name = "Expiry date")]
        public DateTime ExpiryDate { get; set; }


        public string Description { get; set; }
    }
}
