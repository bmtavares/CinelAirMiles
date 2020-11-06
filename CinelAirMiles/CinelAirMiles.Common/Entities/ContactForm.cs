using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CinelAirMiles.Common.Entities
{
    public class ContactForm : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }


        [Required]
        public string Message { get; set; }
    }
}
