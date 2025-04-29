namespace CyclingRaces.Data.Models
{
	public class Result
	{
        public string Id { get; set; } = null!;

		public string CyclistId { get; set; } = null!;
		public Cyclist Cyclist { get; set; } = null!;

        public string RaceId { get; set; } = null!;
        public Race Race { get; set; } = null!;

        public TimeSpan? Time { get; set; }
        public int Rank { get; set; }
        public bool IsVolunteer { get; set; }
    }
}