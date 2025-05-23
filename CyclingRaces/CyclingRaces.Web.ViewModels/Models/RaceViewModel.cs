﻿using CyclingRaces.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyclingRaces.Web.ViewModels.Models
{
    public class RaceViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!;
        public double Distance { get; set; }

    }
}
