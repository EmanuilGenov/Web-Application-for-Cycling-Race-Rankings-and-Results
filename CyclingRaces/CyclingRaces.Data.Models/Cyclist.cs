using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclingRaces.Data.Models
{
	public class Cyclist : ApplicationUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Foreign key to ApplicationUser
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string? TeamId { get; set; }
		public Team? Team { get; set; }
		public ICollection<Result>? Results { get; set; }
	}
}
