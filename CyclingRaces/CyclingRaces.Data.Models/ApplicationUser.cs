using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace CyclingRaces.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Nationality { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public Cyclist? CyclistProfile { get; set; }
        public Organiser? OrganiserProfile { get; set; }
    }
}