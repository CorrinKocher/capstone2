using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// methods to make:
    /// </summary>
    public class Space
    { 
        public int SpaceId { get; set; }

        public string Name { get; set; }

        public string OpenDate { get; set; }

        public string CloseDate { get; set; }

        public int MaximumOccupancy { get; set; }

        public bool WheelChairAccessibility { get; set; }

        public decimal DailyRate { get; set; }

        public int VenueId { get; set; }

      
    }
}
