namespace CyclingRaces.Data.Models
{
	public class Race
	{
		public string Id { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string Location { get; set; } = null!;
		public DateTime Date { get; set; }
		public string Type { get; set; } = null!;
		public double Distance { get; set; }
		public ICollection<Cyclist>? Volunteers { get; set; }
		public ICollection<Result>? Results { get; set; }
		public string OrganiserId { get; set; } = null!;
		public Organiser Organiser { get; set; } = null!;
	}
}