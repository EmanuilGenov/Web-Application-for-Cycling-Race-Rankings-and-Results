using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclingRaces.Web.ViewModels.Models
{
    public class RaceResultViewModel
    {
        public string Id { get; set; } = null!;
        public string Race { get; set; } = null!;
        public string CyclistName { get; set; } = null!;
        public TimeSpan? OverallTime { get; set; }
        public int OverallRank { get; set; }
        public bool IsVolunteer { get; set; }
    }
}
