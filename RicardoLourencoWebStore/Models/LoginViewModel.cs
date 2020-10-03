using RicardoLourencoWebStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }


        [Required]
        [MinLength(6)]
        public string Password { get; set; }


        public bool IsPersistent { get; set; }
    }
}
