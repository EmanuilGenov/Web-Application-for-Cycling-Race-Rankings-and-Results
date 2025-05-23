﻿namespace CyclingRaces.Data.Models
{
	public class Team
	{
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
		public string Country { get; set; } = null!;
		public string Manager { get; set; } = null!;
		public ICollection<Cyclist> Cyclists { get; set; } = null!;
	}
}