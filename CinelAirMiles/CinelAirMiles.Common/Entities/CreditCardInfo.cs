using System.ComponentModel.DataAnnotations;

namespace CinelAirMiles.Common.Entities
{
    public class CreditCardInfo : IEntity
    {
        public int Id { get; set; }


        public Client Client { get; set; }


        public int ClientId { get; set; }


        [Required]
        public string FirstName { get; set; }


        [Required]
        public string LastName { get; set; }


        [Required]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Insert 12 digits")]
        public string Number { get; set; }


        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Insert {1} digits")]
        public string CVC { get; set; }


        [Required]
        public string Month { get; set; }


        [Required]
        public string Year { get; set; }
    }
}
