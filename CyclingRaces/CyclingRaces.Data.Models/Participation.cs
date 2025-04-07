namespace CyclingRaces.Data.Models
{
	public class Participation
	{
		public string Id { get; set; } = null!;
		public string RaceId { get; set; } = null!;
		public Race Race { get; set; } = null!;
		public string CyclistId { get; set; } = null!;
		public Cyclist Cyclist { get; set; } = null!;
		public TimeSpan? OverallTime { get; set; }
		public int OverallRank { get; set; }
	}
}