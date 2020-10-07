using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RicardoLourencoWebStore.Data.Entities
{
    public class User : IdentityUser
    {
        //[Required]
        [Display(Name = "First names")]
        public string FirstNames { get; set; }


        //[Required]
        [Display(Name = "Last names")]
        public string LastNames { get; set; }


        [Display(Name = "Full name")]
        public string FullName { get => $"{FirstNames} {LastNames}"; }


        public string Address { get; set; }


        [NotMapped]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
